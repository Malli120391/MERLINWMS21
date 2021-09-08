<%@ Page Title=" Revert Inbound :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="RevertInbound.aspx.cs" Inherits="MRLWMSC21.mInbound.RevertInbound" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="content1" ContentPlaceHolderID="IBContent" runat="server">

    <asp:ScriptManager runat="server" ID="smngrRevertInbound" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

<%--    <script type="text/javascript" src="Scripts/clueTip/cluetipMain.js"></script>
    <link rel="stylesheet" href="Scripts/clueTip/jquery.cluetip.css" type="text/css" />
    <script type="text/javascript" src="Scripts/clueTip/jquery.cluetip.js"></script>--%>

    <script type="text/javascript">
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }

        function ClearText(TextBox) {
            if (TextBox.value == "Search Store Ref.#...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function ClearTextTenant(TextBox) {
            if (TextBox.value == "Search Tenant...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }
        
        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Store Ref.#...";

            TextBox.style.color = "#A4A4A4";
        }

        function focuslostTenant(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Tenant...";

            TextBox.style.color = "#A4A4A4";
        }

    </script>



    <script type="text/javascript">

        $(document).ready(function () {
            $("#divGRNDetails").dialog({
                autoOpen: false,
                modal: true,
                width: 650,
                height: 450,
                resizable: false,
                draggable: false,
                overflow: 'auto',
                position: ["center top", 40],
                 
                close: function () {

                    $(".ui-dialog").fadeOut(500);

                    $(document).unbind('scroll');

                    $('body').css({ 'overflow': 'visible' });

                },
                title: "Pending Goods-IN List",
                open: function (event, ui) {
                    $(".ui-dialog").hide().fadeIn(500);

                    $('body').css({ 'overflow': 'hidden' });

                    $('body').width($('body').width());

                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });

                    $(this).parent().appendTo("#divGRNDetailsContainer");
                }
                
            });
        });
        function ProdCloseDialog() {

            //Could cause an infinite loop because of "on close handling"
            $("#divGRNDetails").dialog('close');

        }

        function ProdOpenDialog() {
            $("#divGRNDetails").dialog("option", "title", 'GRN Details');
            $("#divGRNDetails").dialog('open');
            blockDialog();

        }

        function blockDialog() {

            //block it to clean out the data
            $("#divGRNDetails").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }

        function unblockDialog() {
            $("#divGRNDetails").unblock();
        }

        function validate() {
            if ($('#<%=txtTenant.ClientID%>').val() != $("#hiTenantName").val()) {
                showStickyToast(false, 'Select Valid Tenant');
                return false;
            }
            return true;
        }

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnMCodeAC();
            }
        }
        function fnMCodeAC() {
            $(document).ready(function () {

                var textfieldname = $('#<%=txtsearchText.ClientID%>');
            DropdownFunction(textfieldname);

            $('#<%=txtsearchText.ClientID%>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRevertStoreRefNumbers") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "" || data.d == null) {
                                showStickyToast(false, "No Store ref.# is available for \'Revert\'");
                            }
                            else {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                    }

                                }))
                            }
                        }

                    });
                },
                minLength: 0
            });

                var textfieldname = $('#<%=txtTenant.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtTenant.ClientID%>').autocomplete({
                    source: function (request, response) {
                         if ($("#<%= this.txtTenant.ClientID %>").val() == '') {
                            $("#<%=hifTenant.ClientID %>").val("0");
                             $("#hiTenantName").val("");
                         }
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH") %>',
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
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    select: function (e, i) {
                        $("#<%=hifTenant.ClientID %>").val(i.item.val);
                        $("#hiTenantName").val(i.item.label);
                    },
                    minLength: 0
                });
        });
    }
    fnMCodeAC();
    </script>




    
        
        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnRevertIB" UpdateMode="Always" runat="server">
            <ContentTemplate>
               
                <div class="container">
                   
                <table border="0" cellpadding="0" cellspacing="0" class="" align="center" width="100%" >
                   
                    <tr>
                        <td class="FormLabels" align="left">
                            <asp:Label ID="lblBulkStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabels" align="left">

                            <asp:Label ID="lblStatusMessage" runat="server" CssClass="errorMsg" />
                          
                        </td>
                    </tr>
                    <tr class="absolute">
                        <td class="FormLabels " >
                            <asp:Panel ID="pnlSearchTextCat" DefaultButton="lnkSearchtext" runat="server">

                                <div class="">

                                    <div class="row">
                                        <div class="col m3 s3 offset-m5 offset-s5">
                                            <div class="flex">
                                                <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                <label><%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                <asp:HiddenField  ID="hifTenant" runat="server" Value="0"/>
                                                <input type="hidden" id="hiTenantName" />
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <asp:TextBox ID="txtsearchText" SkinID="txt_Hidden_Req_Auto" runat="server" required=""></asp:TextBox>
                                                <label><%= GetGlobalResourceObject("Resource", "Number")%> </label>
                                            </div>
                                        </div>
                                        <div class="col m1 s1 p0">
                                            <gap5></gap5>
                                            <asp:LinkButton ID="lnkSearchtext" CausesValidation="True" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lnkSearchtext_Click">Search<i class="material-icons">search</i></asp:LinkButton>
                                        </div>
                                    </div>

                                </div>
                            </asp:Panel>
                        </td>
                    </tr>

                    <tr>
                        <td>

                            <asp:GridView Width="100%" ShowFooter="false" ShowHeader="true" ShowHeaderWhenEmpty="true" ID="gvShipmentResults" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightSeaBlueNew" HorizontalAlign="Left" OnSorting="gvShipmentResults_Sorting" OnPageIndexChanging="gvShipmentResults_PageIndexChanging" OnRowCommand="gvShipmentResults_RowCommand" OnRowDataBound="gvShipmentResults_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,StoreRef%>">
                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Store Ref. #" >--%>
                                        <ItemTemplate>

                                            <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Type">--%>
                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentType%>" >
                                        <ItemTemplate>
                                            <%--<asp:Literal runat="server" ID="ltShipType" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentType(DataBinder.Eval(Container.DataItem, "ShipmentTypeID").ToString()) %>' /><br />--%>
                                            <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' /><br />
                                            <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt.">--%>
                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DocRcvdDt%>" >
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy hh:mm:ss tt}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">--%>
                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" >
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                            <asp:Literal runat="server" ID="ltTenantID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "TenantID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier">--%>
                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Supplier%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    

                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Expected Date">--%>
                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentExpectedDate%>"  >
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltShimentExpectedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentExpectedDate","{0: dd-MMM-yyyy}") %>' />
                                            <asp:LinkButton ID="lnkRevertExpected" runat="server" CssClass="GvLink" Text='<%# GetRevertExpectedLink(DataBinder.Eval(Container.DataItem, "ShipmentExpectedDate","{0: dd/MM/yy}").ToString() )%>' ToolTip="Revert Shipment Expected" CommandName="RevertShipmmentExpected" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StoreRefNo") + "&" + DataBinder.Eval(Container.DataItem, "InboundID") + "&" + DataBinder.Eval(Container.DataItem, "IB_RefWarehouse_DetailsID")  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Received Date">--%>
                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentReceivedDate%>" >
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltShipmentReceivedOn" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd-MMM-yyyy}") %>' />
                                            <asp:LinkButton ID="lnkRevertReceived" runat="server" CssClass="GvLink" Text='<%# GetRevertReceivedLink(DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd/MM/yy}").ToString())%>' ToolTip="Revert Shipment Received" CommandName="RevertShipmmentReceived" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StoreRefNo") + "&" + DataBinder.Eval(Container.DataItem, "InboundID") + "&" + DataBinder.Eval(Container.DataItem, "IB_RefWarehouse_DetailsID")  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%-- <asp:TemplateField ItemStyle-Width="150" Visible="false" HeaderText="Shipment Verification Date">--%>
                                     <asp:TemplateField ItemStyle-Width="150" Visible="false" HeaderText="<%$Resources:Resource,ShipmentVerificationDate%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltShipmentVerifiedOn" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentVerifiedOn","{0: dd-MMM-yyyy}") %>' />
                                            <asp:LinkButton ID="lnkRevertVerification" runat="server" CssClass="GvLink" Text='<%# GetRevertVerificationLink(DataBinder.Eval(Container.DataItem, "ShipmentVerifiedOn","{0:dd-MMM-yyyy}").ToString())%>' ToolTip="Revert Shipment Receipt Confirmation/Verification" CommandName="RevertShipmmentVerification" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StoreRefNo") + "&" + DataBinder.Eval(Container.DataItem, "InboundID") + "&" + DataBinder.Eval(Container.DataItem, "IB_RefWarehouse_DetailsID")  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <%--<asp:TemplateField ItemStyle-Width="200" HeaderText="Store">--%>
                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Store%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetSearchStoreNamesWithVerificationStatus(DataBinder.Eval(Container.DataItem, "IB_RefWarehouse_DetailsID").ToString(),"<br/>", DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "InboundID").ToString() ,DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />
                                            <asp:Literal runat="server" ID="ltStoreShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Status">--%>
                                     <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Status%>" >
                                        <ItemTemplate>
                                            <%--<asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentStatus((DataBinder.Eval(Container.DataItem, "InBoundStatusID").ToString())) %>' />--%>
                                            <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InboundStatus").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-Width="450"  HeaderStyle-Width="300" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <nobr><span >&nbsp;&nbsp; Revert GRN &nbsp;&nbsp;</span></nobr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            
                                            <i class="material-icons vls">rotate_90_degrees_ccw</i>
                                            <span class="vip"><asp:LinkButton  Font-Underline="false" ID="lnkRevertGRN" runat="server" Text="Revert GRN" CommandName="EditChildItems" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "InboundID") %> ' /></span>

                                        </ItemTemplate>
                                    </asp:TemplateField>


                                   <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Close">--%>
                                     <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Close%>" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkCloseShipment" runat="server" CssClass="GvLink" OnClientClick="return confirm('Are you sure you want to close this shipment? This is an irreversible transaction, hence check before you confirm the closure.');" Text="<nobar><i class='material-icons vls'>cancel</i></nobar>" ToolTip="Close Shipment" CommandName="Close" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StoreRefNo") + "&" + DataBinder.Eval(Container.DataItem, "InboundID") + "&" + DataBinder.Eval(Container.DataItem, "IB_RefWarehouse_DetailsID")  %>' />
                                            <a class="helpWTitle" Title="Close Shipment| Closes the Shipment and its linked transactions. However the shipment should be reverted to its initiated status and release any inventory received with this shipment.This is an irreversable process, hence a carefull use of this feature is recommended.">
                                                <i class="material-icons vls">help_outline</i></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
	                                <%--<div align="center">No Data Found</div>--%>
                                    <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </td>
                    </tr>



                </table>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        



    <div id="divGRNDetailsContainer">
        <div id="divGRNDetails" style="display: block;">
            <asp:UpdatePanel ID="upnlGRNDetails" runat="server">
                <ContentTemplate>

                    <asp:Label ID="lblGRNStatus" runat="server"></asp:Label>

                    <asp:Panel runat="server">

                         <br />
                                                               <div style="padding-left:10px;padding-right:10px;">
                    

                    <asp:GridView ID="gvGRNDetails" HorizontalAlign="Center" SkinID="gvLightSteelBlueNew" runat="server" CellPadding="0" AllowPaging="true"
                        AllowSorting="false"
                        ShowHeader="true" ShowHeaderWhenEmpty="true"
                        OnPageIndexChanging="gvGRNDetails_PageIndexChanging"
                        PagerStyle-HorizontalAlign="Right"
                        PageSize="10"
                        CellSpacing="2">

                        <Columns>


                            <asp:TemplateField HeaderText="PO Number" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <i class="material-icons vls">rotate_90_degrees_ccw</i>
                                    <asp:Literal ID="lthidInboundID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "InboundID") %>' />
                                    <asp:Literal ID="ltPOHeaderID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "POHeaderID") %>' />
                                    <asp:Literal ID="ltSupplierInvoiceID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierInvoiceID") %>' />

                                    <asp:HyperLink ID="HyperLink2" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' NavigateUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Invoice#" ItemStyle-Width="100">--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,Invoice%>" ItemStyle-Width="100">
                                <ItemTemplate>

                                    <asp:Literal ID="ltHidInbound_GRNUpdateID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GRNUpdateID") %>' />

                                     <i class="material-icons vls">rotate_90_degrees_ccw</i>
                                    <asp:LinkButton Font-Underline="false" ID="lnkEditPOItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' PostBackUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' />
                                    <asp:HyperLink ID="HyperLink3" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' NavigateUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                </ItemTemplate>

                            </asp:TemplateField>

                         <%--   <asp:TemplateField HeaderText="GRN Number">--%>
                               <asp:TemplateField HeaderText="<%$Resources:Resource,GRNNumber%>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltGRNNumber" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "GRNNumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                           <%-- <asp:TemplateField HeaderText="UpdatedBy">--%>
                             <asp:TemplateField HeaderText="<%$Resources:Resource,UpdatedBy%>" >
                                <ItemTemplate>
                                    <asp:Literal ID="ltGRNUpdatedBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GRNUpdatedBy") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>

                           <%-- <asp:TemplateField HeaderText="GRN Date">--%>
                             <asp:TemplateField HeaderText="<%$Resources:Resource,GRNDate%>" >
                                <ItemTemplate>
                                    <asp:Literal ID="ltGRNDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GRNDate","{0: dd-MMM-yyyy}") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>

                           <%-- <asp:TemplateField HeaderText="Revert GRN" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">--%>
                             <asp:TemplateField HeaderText="<%$Resources:Resource,RevertGRN%>"  ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                <HeaderTemplate>
                                    <%--<nobr>Revert GRN</nobr>--%>
                                    <nobr>  <%= GetGlobalResourceObject("Resource", "RevertGRN")%></nobr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRevertGRNItems" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkRevertGRNItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Revert <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkRevertGRNItem_Click" OnClientClick="return confirm('Are you sure you want to revert?');" />
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
	                       <%-- <div align="center">No Data Found</div>--%>
                             <div align="center"> <%= GetGlobalResourceObject("Resource","NoDataFound")%></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                                                                   </div>

                    </asp:Panel>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
