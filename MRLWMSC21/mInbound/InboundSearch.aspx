     <%@ Page Title=" Search Inbound :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="InboundSearch.aspx.cs" Inherits="MRLWMSC21.mInbound.InboundSearch" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="IBContent" runat="server">

    <asp:ScriptManager ID="sMngrIbSearch" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>

    <script type="text/javascript">
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }





    </script>



    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadAutocompletes();
            }
        }

        function fnLoadAutocompletes() {



            //$('#<%= this.txtFromDate.ClientID%>').attr('readonly','readonly'); 


            $("#<%= this.txtFromDate.ClientID%>").datepicker({

                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%=this.txtToDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" });
                    this.focus();
                    
                    $('.hasDatepicker').datepicker({minDate:-1,maxDate:-2}).attr('readonly','readonly'); 
                },
                onClose: function (){ this.focus(); $('.hasDatepicker').datepicker({minDate:-1,maxDate:-2}).attr('readonly','readonly');     }
            });
                //$("#<%= this.txtToDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy", maxDate: new Date() });
            $("#<%= this.txtToDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date()
            });

         <%--   $('#<%= this.txtFromDate.ClientID%>, #<%=this.txtToDate.ClientID%>').keypress(function () {
                return false;
            });--%> 
          <%--  $("#<%= this.txtFromDate.ClientID%>").keydown(function () {
                return false;
            });--%>
          <%--  $("#<%= this.txtToDate.ClientID%>").keydown(function () {
                return false;
            });--%>
        }
        fnLoadAutocompletes();

    </script>

    <script type="text/javascript">


        $(document).ready(function () {


            fnLoadAutocompletes();



            $("#<%= this.txtFromDate.ClientID%>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%=this.txtToDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" });



                }

            });
            $("#<%= this.txtToDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy", maxDate: new Date() });

            $("#<%= this.txtTenant.ClientID %>").focusout(function () {
                if ($("#<%= this.txtTenant.ClientID %>").val() == '')
                    $("#<%= this.hifTenant.ClientID %>").val('0');
            })

            var TextFieldName = $("#<%= this.txtTenant.ClientID %>");
            DropdownFunction(TextFieldName);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH") %>',
                        data: "{ 'prefix': '" + request.term + "','whid': '" + $("#ddlStore").val() + "'}",
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
                    $("#<%= this.txtFromDate.ClientID%>").val('');
                    $("#<%= this.txtToDate.ClientID%>").val()

                    },
                    minLength: 0
            });
        });


        
        function ClearText(TextBox) {
            if (TextBox.value == "Search...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search...";

            TextBox.style.color = "#A4A4A4";
        }

        $(function () {
            $('.isvisibleNow').on('click', function () {
                $('.ishideNow').slideToggle();
            });
        });

    </script>

    <style>
        /*body {
            overflow: scroll;
        }*/

        .pager {
            margin-top: 10px;
            margin-bottom: 10px;
        }


            .page-numbers.next, .page-numbers.prev {
                border: 1px solid white;
            }

            .page-numbers.desc {
                border: none;
                margin-bottom: 10px;
            }

        #MainContent_IBContent_pnlDateStoreShipmentSearch table {
            width: 100%;
        }

        #MainContent_IBContent_pnlSearchTextCat table {
            margin-left: auto;
        }

        .page-numbers {
             box-shadow: var(--z1);
             border: 1px groove #ffffff;
        }

        #MainContent_IBContent_dlPagerupper td{    padding: 0px 2px;
        }

        #MainContent_IBContent_dlPager td{padding: 0px 2px;
        }

        .centereddf {
            text-align: center;

        }

        #MainContent_IBContent_dlPagerupper{
            display:none;
        }
        .row{
            margin-bottom:2px;
        }
 
 .aspNetDisabled {
    display: flex;
    display: table-cell;
    height: 20px;
    width: 17px;
    background-color: #cad5e0;
    border-radius: 14%;
    color: #fff;
    font-weight: normal;
    border: 1px solid #B0C4DE;
    text-align: center;
    text-decoration: none;
    vertical-align: middle;
    box-shadow: var(--z1);
    padding: 0px;
    display: inline-block !important;
    border: 2px solid var(--sideNav-bg) !important;
    background-color: var(--sideNav-bg) !important;
    width: 20px !important;
    height: 20px !important;
    line-height: 20px;
}

        .gvLightSteelBlueNew tr td {
            white-space: normal !important;


        }

        .gvLightSteelBlueNew {
            overflow:hidden;
        }

        .ishideNow {
            display:none;
        }

    

         

        .module_login [cellspacing="2"] {
                white-space: nowrap;
        }



    </style>

    <div class="container">
        <div>
            <div class="row">
                <div class="">
                    <asp:Label runat="server" ID="lblIbSearchStatus" CssClass="errorMsg"></asp:Label>
                </div>
            </div>
           <div class="row">

                <div>
                    <div class="row">
                        <div class="FormLabels">
                            <asp:Panel ID="pnlDateStoreShipmentSearch" runat="server">
                                 <div class="col m3 s3" >
                                    <div class=" flex">
                                        <asp:DropDownList ID="ddlStore" runat="server" ClientIDMode="Static" />
                                         <span class="errorMsg">*</span>  
                                    </div>
                                </div>
                                

                                <div class="col m3 s3" >
                                        <div class=" flex">
                                            <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                            <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                        </div>
                                    </div>
                                <div class="col m3 s3">
                                    <div class=" flex">
                                        <asp:TextBox ID="txtFromDate" runat="server" placeholder="" required="" />
                                        <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                                    </div>
                                </div>
                                <div class="col m3 s3">
                                    <div class=" flex">
                                        <asp:TextBox ID="txtToDate" runat="server" required="" />
                                        <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                                    </div>
                                </div>
                                <div class="col m3 s3" >
                                        <div class=" flex">
                                            <asp:DropDownList ID="ddlShipmentStatus" runat="server" />
                                        </div>
                                    </div>
                                <%--<div class="col m3 s3" >
                                    <div class=" flex">
                                        <asp:DropDownList ID="ddlStore" runat="server" />
                                    </div>
                                </div>

                                <div class="col m3 s3" >
                                    <div class=" flex">
                                        <asp:DropDownList ID="ddlShipmentType" runat="server" />
                                    </div>
                                </div>--%>
                            </asp:Panel>
                        </div>



                        <div class="row ishideNow">
                            <asp:Panel ID="pnlSearchTextCat" DefaultButton="lnkSearchtext" runat="server">
                                   <%-- <div class="col m3 s3" style="display:none">
                                        <div class=" flex">
                                            <asp:DropDownList ID="ddlShipmentStatus" runat="server" />
                                        </div>
                                    </div>--%>
                                 <div class="col m3 s3">
                                        <div class=" flex">
                                            <asp:DropDownList ID="ddlCategory" runat="server" required="" />
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class=" flex">
                                            <asp:TextBox ID="txtsearchText" runat="server" SkinID="txt_Hidden_Req" required="required" />
                                            <label><%= GetGlobalResourceObject("Resource", "Search")%></label>
                                        </div>
                                    </div>
                                   <%-- <div class="col m3 s3" >
                                    <div class=" flex">
                                        <asp:DropDownList ID="ddlStore" runat="server" />
                                    </div>
                                </div>--%>

                                <div class="col m3 s3" >
                                    <div class=" flex">
                                        <asp:DropDownList ID="ddlShipmentType" runat="server" />
                                    </div>
                                </div>
                                    <%--<div class="col m3 s3" style="display:none">
                                        <div class=" flex">
                                            <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                            <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                        </div>
                                    </div>--%>

                                    <div class="flex" style="display: none;">
                                        <asp:DropDownList ID="ddlClearanceCompany" runat="server" required="" />
                                        <label><%= GetGlobalResourceObject("Resource", "ClearanceCompany")%> </label>
                                    </div>
                            </asp:Panel>


                        </div>

                        <div class="row">
                            <div class="col m4 offset-m8">
                                <button type="button" class="btn btn-primary isvisibleNow"><%= GetGlobalResourceObject("Resource", "AdvancedSearch")%> <i class="material-icons vl isVisibleNow">youtube_searched_for</i></button>
                                 <asp:LinkButton ID="lnkSearchtext" CausesValidation="True" CssClass="btn btn-sm btn-primary" runat="server" OnClick="lnkSearchtext_Click">
                                          <%= GetGlobalResourceObject("Resource", "Search")%>   <%= MRLWMSC21Common.CommonLogic.btnfaSearch %>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btninboundsearchlist" runat="server" CssClass="btn btn-primary" OnClick="btninboundsearchlist_Click"> <%= GetGlobalResourceObject("Resource", "ExportExcel")%>  <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

            </div>






            <asp:UpdatePanel ID="upnlInboundSearch" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>


                    <%--  <tr>
                    <td class="FormLabels" align="left">
                    <asp:Panel runat="server" ID="sss">
                            <asp:Label runat="server" ID="lblRecordCount" CssClass="SubHeading3"></asp:Label>
                            <asp:ImageButton ID="btninboundsearchlist" runat="server" ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="btninboundsearchlist_Click" ToolTip="Export To Excel" />
                        </asp:Panel>
                    </td>
                </tr>--%>
                    <div class="row">
                        <div>
                            <div>
                                <div class="row">
                                    <div class="col m6" hidden>
                                        <div class="" flex >
                                            <div class='mr10'>
                                                <asp:Label runat="server" ID="lblRecordCount" CssClass="SubHeading3"></asp:Label>
                                            </div>

                                            <div class="" flex>
                                                <div>
                                                    <asp:Literal runat="server" ID="lblstringperpage" Text="Display " />
                                                </div>
                                                <asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="drppagesize_SelectedIndexChanged" ID="drppagesize">
                                                    <asp:ListItem Text="25" Value="25" Selected="True" />
                                                    <asp:ListItem Text="50" Value="50" />
                                                    <asp:ListItem Text="80" Value="80" />
                                                    <asp:ListItem Text="100" Value="100" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class=" ">
                                            <div>
                                                <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ID="dlPagerupper" OnItemCommand="dlPagerupper_ItemCommand">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" class="page-numbers" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>

                                            <div>
                                                <div class="gap5"></div>
                                                
                                            </div>
                                        </div>

                                    </div>


                                </div>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row">
                <div align="center">
                    <div class="wrapper1">
                        <div class="divUP">
                        </div>
                    </div>

                    <div class="wrapper2">
                        <div class="divdown" style="width: 100%; overflow-x: auto;">
                            <asp:GridView ShowFooter="false" CssClass="table-striped" VerifyRenderingInServerForm="true"
                                ID="gvShipmentResults" runat="server"
                                ShowHeader="true" ShowHeaderWhenEmpty="true"
                                AllowPaging="false" PageSize="25" AllowSorting="True"
                                SkinID="gvLightSteelBlueNew"
                                HorizontalAlign="Left" OnRowDataBound="gvShipmentResults_RowDataBound" OnSorting="gvShipmentResults_Sorting" OnPageIndexChanging="gvShipmentResults_PageIndexChanging">

                                <Columns>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,StoreRef%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink( DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,ShipmentType%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' /><br />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,DocRcvdDt%>">

                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,ShipRcvdDtOffloadTime%>">

                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltShipRcvdOn" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd-MMM-yyyy}") %>' />
                                            <asp:Literal runat="server" ID="ltOffloadTime" Text='<%# DataBinder.Eval(Container.DataItem, "Offloadtime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant" >--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Tenant%>">

                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltTenant" Text='<%#    DataBinder.Eval(Container.DataItem, "CompanyName")  %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Supplier%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField ItemStyle-Width="200" HeaderText="Store(s)">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Stores%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetSearchStoreNamesWithVerificationStatus(DataBinder.Eval(Container.DataItem, "ReferedStoreIDs").ToString(),"<br/>", DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="PO Number">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,PONumber%>">

                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--  <asp:TemplateField ItemStyle-Width="250" HeaderText="Invoice #">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Invoice%>">

                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltInvoice" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <%--  <asp:TemplateField ItemStyle-Width="250" HeaderText="GRN #">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,GRN%>">


                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltGRNNumbers" Text='<%# DataBinder.Eval(Container.DataItem, "GRNNumber") %>' /><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField ItemStyle-Width="180" HeaderText="GRN Done By">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,GRNDoneBy%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltGRNDoneBy" Text='<%#  DataBinder.Eval(Container.DataItem, "GRNDoneBy").ToString() %>' /><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Verified Dt.">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,VerifiedDt%>">

                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltShipVrfyOn" Text='<%#    DataBinder.Eval(Container.DataItem, "ShipmentVerifiedOn","{0: dd-MMM-yyyy}")  %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField ItemStyle-Width="250" HeaderText="Status">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Status%>">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltRevertLog" Text='' />
                                            <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InboundStatus").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <nobr> &nbsp;&nbsp; RTS  &nbsp;&nbsp;&nbsp; </nobr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <a style="padding-right: 50px;" id="A1" class="helpWTitle vip" Title="MRLWMSC21® - Receiving Tally Sheet(RTS) | Receiving Tally Sheet with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("RTReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RTS
                                            <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                <img  src="../Images/redarrowright.gif" /> </a>
                                           
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <nobr> &nbsp;&nbsp; RCR &nbsp;&nbsp;&nbsp; </nobr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <a target="_blank" style="padding-right: 30px;" id="A1" class="helpWTitle vip" title="MRLWMSC21® - Receipt Confirmation Report(RCR)" runat="server" visible="true" href='<%# String.Format("~/mReports/ReceiptConfirmationReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>'>RCR
                                                <img src="../Images/redarrowright.gif" />
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--  <asp:TemplateField HeaderText="Suggested Location" Visible="false" ItemStyle-HorizontalAlign="Center">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,SuggestedLocation%>" Visible="false" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hykSuggestedPicknote" runat="server" NavigateUrl='<%#String.Format( "SuggestedLocation.aspx?ibdid={0}",DataBinder.Eval(Container.DataItem,"InboundID") )%>' Text="Receive" CssClass="ButEmpty" />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <%--  <asp:TemplateField HeaderText="Modify" ItemStyle-CssClass="centereddf" >--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Modify%>" ItemStyle-CssClass="centereddf">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditInbound" Font-Underline="false" Visible="false" runat="server" CssClass="GvLink" PostBackUrl='<%# Eval("InboundID", "InboundDetails.aspx?ibdid={0}") %>' Text="<nobr> Modify <img src='../Images/redarrowright.gif' border='0' /></nobr>" />

                                            <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}")  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                        </ItemTemplate>

                                    </asp:TemplateField>


                                </Columns>
                                <EmptyDataTemplate>
                                    <%-- <div align="center">No Data Found</div>--%>
                                    <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                    <%--</asp:Panel>--%>
                        

                 

                 

                  
                         
                </div>
            </div>

            <tr>
                <td align="right">
                    <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPagerupper_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CssClass="page-numbers" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>

        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfpageId" />

</asp:Content>
