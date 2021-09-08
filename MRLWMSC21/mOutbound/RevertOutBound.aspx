<%@ Page Title=" .:  Revert Outbound :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="RevertOutbound.aspx.cs" Inherits="MRLWMSC21.mOutbound.RevertOutbound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="smngrRevertOutbound" SupportsPartialRendering="true"></asp:ScriptManager>


    <script type="text/javascript">
        var OBDData = null;
        var outboundid = 0;
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }


        function ClearText(TextBox) {
            if (TextBox.value == "Delv. Doc.#...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function ClearTextTenant(TextBox) {
            if (TextBox.value == "Tenant...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Delv. Doc.#...";

            TextBox.style.color = "#A4A4A4";
        }

        function focuslostTenant(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Tenant...";

            TextBox.style.color = "#A4A4A4";
        }
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }

        function RevertPNCDate(id) {
            outboundid = id;
           
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/RevertOutbound.aspx/getOBDDetailsForRevert") %>',
                data: "{ 'outboundid': '" + id+ "'}",//<=cp.TenantID%>
                  dataType: "json",
                  type: "POST",
                  contentType: "application/json; charset=utf-8",
                  success: function (data) {
                      debugger;
                      obj = JSON.parse(data.d);
                      OBDData = obj;
                      console.log(obj);
                      var s = '<table class="table table-striped"><tr><th>S.No</th><th>OBD No.</th><th>OBD Date</th><th>SO No.</th><th>Line No.</th><th>SKU</th><th>SO Qty.</th><th>Picked Qty.</th><th></th><th></th></tr>';
                      for (var i = 0; i < obj.length; i++) {
                          s += '<tr><td>' + (i + 1) + '</td><td>' + obj[i].OBDNumber + '</td><td>' + obj[i].OBDDate + '</td><td>' + obj[i].sonumber + '</td><td>' + obj[i].LineNumber + '</td><td>' + obj[i].MCode + '</td><td>' + obj[i].SOQuantity + '</td><td>' + obj[i].GOODSQTY + '</td><td><input type="text" onkeypress="return isNumber(event)" id=\'' + obj[i].SODetailsID + '\'  value=\'' + obj[i].GOODSQTY + '\'/></td><td><button type="button" class="btn btn-xs btn-primary" style="height:30px;" onclick="btnrevert(' + obj[i].TransactionDocID + ',' + obj[i].SODetailsID + ',' + obj[i].GOODSQTY+')">Revert</button></td></tr>';
                      }
                      s += '</table>';
                      $('#divKitItems').html(s);
                  }
            });
            //$.ajax({
            //    type: "POST",
            //    url: "Service.asmx/GetDetails",
            //    data: "{ name: '" + name + "', age: " + age + "}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (r) {
            //        alert(r.d);
            //    },
            //    error: function (r) {
            //        alert(r.responseText);
            //    },
            //    failure: function (r) {
            //        alert(r.responseText);
            //    }
            //});
           // InventraxAjax.AjaxResultExecute("RevertOutbound.aspx/getOBDDetailsForRevert", data:{'outboundid': 1}, 'GetItemDetailsOnSuccess', 'GetItemDetailsOnError', null);
        }
        function btnrevert(obdid, sodetailsid, pickedqty) {
            debugger;
            if (confirm("Are you sure want to revert ")) {
                var revertqty = $('#' + sodetailsid).val();
                if (revertqty == "") {
                    showStickyToast(false, 'Please enter Revert Qty. ');
                    return false;
                }
                else if (parseInt(revertqty) == 0) {
                    showStickyToast(false, 'Please enter valid Revert Qty. ');
                    return false;
                }
                $.ajax({
                    url: '<%=ResolveUrl("~/mOutbound/RevertOutbound.aspx/RevertOutbounDetails") %>',
                data: "{ 'outboundid': '" + obdid + "','SodetailsId': '" + sodetailsid + "','Qty': '" + pickedqty + "'}",//<=cp.TenantID%>
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.d == -1) {
                        showStickyToast(false, 'No Line Items in given outbound to revert');
                        return false;
                    }
                    else if (data.d == -2) {
                        showStickyToast(false, 'Error While Revert');
                        return false;
                    }
                    else {
                        showStickyToast(false, 'Successfully Reverted');
                        location.reload();
                        return false;
                    }

                   
                    

                }
            });
            }
           
            
        }
        function RevertAllItems() {
            debugger;
           //outboundid
            if (confirm("Are you sure want to revert all Items")) {
                $.ajax({
                    url: '<%=ResolveUrl("~/mOutbound/RevertOutbound.aspx/RevertALLLineItemsForOutbound") %>',
                     data: "{ 'outboundid': '" + outboundid + "'}",//<=cp.TenantID%>
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     success: function (data) {
                         debugger;
                         if (data.d == -1) {
                             showStickyToast(false, 'No Line Items in given outbound to revert');
                             return false;
                         }
                         else if (data.d == -2) {
                             showStickyToast(false, 'Error While Revert');
                             return false;
                         }
                         else {
                             showStickyToast(false, 'Successfully Reverted');
                             location.reload();
                             return false;
                         }
                         
                         

                     }
                 });
            }
           
        }
        function revertclear() {

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

                var textfieldname = $('#<%= this.txtsearchText.ClientID %>');
                    DropdownFunction(textfieldname);

                    $('#<%= this.txtsearchText.ClientID %>').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRevertDelvDocNo") %>',
                                data: "{ 'prefix': '" + request.term + "','Tenant':'" + document.getElementById('<%= this.txtTenant.ClientID %>').value + "'}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "" || data.d == "/") {
                                        showStickyToast(false, 'No \'Delivery\' is available for \'Revert\'');
                                    }
                                    else {
                                        response($.map(data.d, function (item) {
                                            return {
                                                label: item,
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
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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
                    },
                    minLength: 0
                });

                });
            }
            fnMCodeAC();
    </script>
    <style>
        /*#MainContent_OBContent_pnlSearchTextCat {
            position: absolute;
             right: 0;
        }*/

        .btn-primary {
            height:fit-content;
            align-self: center;
        }
    </style>
    <div class="dashed"></div>
    <div class="pagewidth">
                       <asp:UpdateProgress ID="uprgRevertOB" runat="server" AssociatedUpdatePanelID="upnlRevertOB">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                                <%--<div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>--%>
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
                        <asp:UpdatePanel ID="upnlRevertOB" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="dashed"></div>
                                <table border="0" cellpadding="0" cellspacing="0" align="center" width="90%">

                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBlukStatus" CssClass="ErrorMsg" runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>

                                            <asp:Panel ID="pnlSearchTextCat" DefaultButton="lnkSearchtext" runat="server">
                                                <table border="0" cellpadding="3" cellspacing="3" width="100%">
                                                    <tr>
                                                        <td class="FormLabels" valign="top" align="right">
                                                            <div style="float:right;">
                                                            <div class="flex__">
                                                                <div class="flex">
                                                            <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required="" ></asp:TextBox>
                                                                        <asp:HiddenField  ID="hifTenant" runat="server" Value="0"/>
                                                                    <label>Tenant</label>
                                                                    </div>
                                                                    &nbsp;&nbsp;
                                                                <div class="flex">
                                                            <asp:TextBox ID="txtsearchText" runat="server" SkinID="txt_Hidden_Req_Auto" required="" />
                                                                    <label>Delv. Doc.#...</label>
                                                                    </div>
                                                            &nbsp;&nbsp;
                                                            <asp:LinkButton ID="lnkSearchtext" CausesValidation="True" runat="server" OnClick="lnkSearchtext_Click" CssClass="btn btn-primary"> Search <span class="space fa fa-search"></span></asp:LinkButton>
                                                                </div>
                                                                </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>


                                            <asp:Label ID="lblStatusMessage" runat="server" CssClass="ErrorMsg" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>&nbsp;<br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>



                                            <asp:GridView ShowFooter="false" CellPadding="1" CellSpacing="1" GridLines="None" ID="gvDeliveryResults" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightOrangeNew" HorizontalAlign="Left" OnSorting="gvDeliveryResults_Sorting" OnPageIndexChanging="gvDeliveryResults_PageIndexChanging" OnRowCommand="gvDeliveryResults_RowCommand">
                                                <Columns>

                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. #">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" Visible="false" ID="ltOBDNumber" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") %>' />

                                                            <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="P.Note" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkPNote" runat="server" CssClass="GvLink" Visible="true" NavigateUrl='<%# Eval("OutboundID", "DeliveryPickNote.aspx?obdid={0}") %>' Text="<nobr>P.Note<img src='../Images/redarrowright.gif' border='0' /></nobr>" ToolTip="Get Pick Note." />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center">

                                                        <HeaderTemplate>
                                                            <nobr>Delv. Doc. Type</nobr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>' /><br />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="Customer">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField ItemStyle-Width="150" >
                                                        <HeaderTemplate>
                                                            <nobr>Delv. Doc. Date</nobr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField ItemStyle-Width="180" HeaderText="PNC Date">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltSentForPGI_DT" Text='<%# DataBinder.Eval(Container.DataItem, "PNCDate","{0: dd/MM/yy}") %>' />
                                                            <%--<asp:LinkButton ID="lnkRevertVerification" runat="server" OnClientClick="RevertPNCDate()" CssClass="GvLink" Text='<%# DataBinder.Eval(Container.DataItem, "PNCdata") %>' ToolTip="Revert PNC"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") +  "&" + DataBinder.Eval(Container.DataItem, "OutboundID")+ "&" + DataBinder.Eval(Container.DataItem, "OB_RefWarehouse_DetailsID") %>' />--%>
                                                            <a onclick="RevertPNCDate(<%#DataBinder.Eval(Container.DataItem, "OutboundID").ToString() %>);" data-toggle="modal" data-target="#SupModal"><%# DataBinder.Eval(Container.DataItem, "PNCdata") %></a>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>




                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="PGI Date">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltPGIDate" Text='<%# DataBinder.Eval(Container.DataItem, "PGIDate","{0: dd/MM/yy}") %>' />
                                                            <asp:LinkButton ID="lnkRevertPGI" runat="server" CssClass="GvLink" Text='<%# GetRevertPGI(DataBinder.Eval(Container.DataItem, "PGIDate","{0:dd/MM/yy}").ToString())%>' ToolTip="Revert PGI" CommandName="RevertPGI" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") + "&" + DataBinder.Eval(Container.DataItem, "OutboundID") + "&" + DataBinder.Eval(Container.DataItem, "OB_RefWarehouse_DetailsID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Stores">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDeliveryDate" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNamesWithDeliveryStatus(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryStatusID").ToString(), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsReservationDelivery")), DataBinder.Eval(Container.DataItem, "OB_RefWarehouse_DetailsID").ToString(),DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(), DataBinder.Eval(Container.DataItem,"OutboundID").ToString(),cp.TenantID.ToString()) %>' />

                                                            <b>
                                                                <asp:LinkButton ID="lnkRevertDelivery" runat="server" CssClass="GvLink" Text='<%# GetRevertDelivery(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryDate","{0:dd/MM/yy}").ToString())%>' ToolTip="Revert Delivery" CommandName="RevertDelivery" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") + "&" + DataBinder.Eval(Container.DataItem, "OutboundID")  + "&" + DataBinder.Eval(Container.DataItem, "OB_RefWarehouse_DetailsID") %>' /></b>
                                                            <br />
                                                            <b>
                                                                <asp:LinkButton ID="lnkRevertandReleaseDelivery" runat="server" CssClass="GvLink" OnClientClick="return confirm('Are you certain you want to revert and release the  delivery. ?');" Text='<%# GetRevertandReleaseDelivery(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryDate","{0:dd/MM/yy}").ToString())%>' ToolTip="Revert Delivery" CommandName="RevertandRelease" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") + "&" + DataBinder.Eval(Container.DataItem, "OutboundID")  + "&" + DataBinder.Eval(Container.DataItem, "OB_RefWarehouse_DetailsID") %>' /></b>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "DeliveryStatus").ToString() %>' />
                                                            <asp:LinkButton ID="lnkCloseCancel" runat="server" CssClass="GvLink" Text='<%# GetRevertCloseCancel(DataBinder.Eval(Container.DataItem, "DeliveryStatusID").ToString())%>' ToolTip="Revert Delivery" CommandName="RevertCloseCancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") + "&" + DataBinder.Eval(Container.DataItem, "OutboundID")  + "&" + DataBinder.Eval(Container.DataItem, "OB_RefWarehouse_DetailsID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>

                                            </asp:GridView>



                                        </td>
                                    </tr>


                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>
<br />


    <div id="SupModal" class="modal fade">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <h4 class="modal-title" style="display: inline !important;">OBD Items</h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" onclick="revertclear();" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                  <div id="divKitItems">

                                                 </div>   

                                                
                                                <div class="modal-footer">
                                                   
                                                   
                                                  <%--  <button type="button" class="btn btn-secondary" style="color:#fff !important;" onclick="myKitclear();">Clear</button>
                                                    <button type="button" class="btn btn-secondary" style="color:#fff !important;" data-dismiss="modal">Close</button>--%>
                                                    <button type="button" class="btn btn-warning" onclick="RevertAllItems();">Revert All</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
        </div>


</asp:Content>
