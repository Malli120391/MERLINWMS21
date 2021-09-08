<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StandardCycleCount.aspx.cs" Inherits="MRLWMSC21.mInventory.StandardCycleCount" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="mySManager" EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
    <style type="text/css" media="print">
        .OnlyForPrint {
            visibility: hidden;
        }

        .tablealignment {
            table-layout: fixed;
            overflow: hidden;
        }

        .auto-style1 {
            width: 142px;
        }
       

        @page
        {
        size: landscape;
        margin: 1cm;
        }
        .flex input[type="text"], input[type="number"], textarea {
            width:92% !important;
        }


    </style>


    <script type="text/javascript" src="Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/jQuery2/ben_Print.js"></script>
    <script type="text/javascript">

        function printDiv(divName)
        {

            //$("#PrintCCReport").print();
            var panel = document.getElementById(divName);
            var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,resizable=1');
            printWindow.document.write('<html><head><title>Cycle Count Note</title>');
            printWindow.document.write('<style>@page{size:landscape;}</style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
           
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();// printWindow.close();
               
            }, 500);


        }
        function test() {
            document.getElementById("testdiv").style.visibility = "visible";
            $("#testdiv").dialog('open');
            $("#testdiv").dialog('option', 'title', "Create New Locations");

        }
        //Script for PrinPrinting CCReport By clickin on Monthly links
        function PrintCCTallyReport(ccid) {

            document.getElementById('<%=txtCyclecountID.ClientID%>').value = ccid;
           $("#reportDailog").dialog("option", "position", [100, 200]);
           $("#reportDailog").dialog('open');
           
          
           __doPostBack("<%= lnkgo.UniqueID %>", "OnClick");
           $("#reportDailog").dialog('close');

       }
       function PrintFinalReport() {
          
           $("#printreport").print();
       }

       function ValidateClientSide()
       {
           var value = document.getElementById('<%=ddlWarehouse.ClientID%>').value;
           if (value == 0) {
               alert("Please Check Fro Mandatory Fields");
               return false;
           } else
           {
               return true;
           }
           //alert(value);
       }
    </script>
    <script type="text/javascript">


        $(document).ready(function ()
        {
            var TextFieldName = $("#<%= this.txtSupplier.ClientID %>");
            DropdownFunction(TextFieldName);
            $("#<%= this.txtSupplier.ClientID %>").autocomplete({
                source: function (request, response) {
                 
                    $.ajax({
                        url: '<%=ResolveUrl("../mWebServices/FalconWebService.asmx/LoadSupplierData") %>',
                        data: "{ 'prefix': '" + request.term + "',TenantID:'1'}",
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

                    $("#<%=hidSupplier.ClientID %>").val(i.item.val);
                        },
                        minLength: 0
            });


            
            $("#divEditCC").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 600,
                width: 600,
                resizable: false,
                position: ["center top", 40],
                draggable: true,
                open: function (event, ui) { $(this).parent().appendTo("#divEditCCDlgContainer"); }


            });
            $("#testdiv").dialog({
                autoOpen: false,
                width: 550,
                height: 400,
                hide: "close",
                modal: true,
                draggable: true,
                resizable: false,
                position: {
                    my: "center",
                    at: "center",
                    of: window
                }
            });


            $("#divCCPrintList").dialog(
               {
                   autoOpen: false,
                   height: '700',
                   width: '1000',
                   modal: true,
                   resizable: false,
                   draggable: true,
                   position: ["center top", 40],
                   open: function (event, ui) { $(this).parent().appendTo("#divCCPrintListContainer"); }
               });
            $("#reportDailog").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 600,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                open: function (event, ui) { $(this).parent().appendTo("#divCCPrintListContainer"); }


            });
        });
        function openPrintCCListDialog(title, linkID) {

            //var pos = $("#" + linkID).position();
            //var top = pos.top - 100;
            //var left = pos.left + $("#" + linkID).width() - 800;


            $("#divCCPrintList").dialog("option", "title", title);
            //$("#divCCPrintList").dialog("option", "position", [left, top]);
            $("#divCCPrintList").dialog('open');
        }

        function openPrintCCListDialogAndBlock(title, linkID) {

            openPrintCCListDialog(title, linkID);
            $("#divCCPrintList").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_blue.gif" />',
               css: { border: '0px' },
               fadeIn: 0,
               fadeOut: 0,
               overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
           });
       }

       function closePrintCCListDialog() {
           //Could cause an infinite loop because of "on close handling"
           $("#divCCPrintList").dialog('close');
       }


       function unblockPrintCCListDialog() {
           $("#divCCPrintList").unblock();
       }

       function onTest() {
           $("#divCCPrintList").block({
               message: '<h1>Processing</h1>',
               css: { border: '3px solid #a00' },
               overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
           });
       }


       function closeDialog() {
           //Could cause an infinite loop because of "on close handling"
           $("#divEditCC").dialog('close');
       }

       function openDialog(title, linkID) {

           //var pos = $("#" + linkID).position();
           //var top = pos.top - 100;
           //var left = pos.left + $("#" + linkID).width() - 800;


           $("#divEditCC").dialog("option", "title", title);
           //$("#divEditCC").dialog("option", "position", [left, top]);

           $("#divEditCC").dialog('open');
       }


       function openDialogAndBlock(title, linkID)
       {
           if (title == 'Add Cycle Count') {
               var WareHouse = $("#<%=ddlWarehouse.ClientID%>").attr("value");
               if (WareHouse == 0) {
                   showStickyToast(false, "Please select 'Warehouse'");
                   return false;
               }
              

           }

           openDialog(title, linkID);
           //block it to clean out the data
           $("#divEditCC").block({
               message: '<img src="<%=ResolveUrl("~") %>Images/async_blue.gif" />',
               css: { border: '0px' },
               fadeIn: 0,
               fadeOut: 0,
               overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
           });
       }


       function unblockDialog() {
           $("#divEditCC").unblock();
       }

       function onTest() {
           $("#divEditCC").block({
               message: '<h1>Processing</h1>',
               css: { border: '3px solid #a00' },
               overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
           });
       }
       function go() {
           alert("hello");
       }

    </script>

    <script language="javascript" type="text/JavaScript">
        function check_uncheck(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement
                if (this != null) {

                    if (ValId.indexOf('chkSelforCCAll') != -1) {

                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes
                        if (ValChecked)
                            frm.elements[i].checked = true;
                        else
                            frm.elements[i].checked = false;
                    }
                    else if (ValId.indexOf('chkSelforCC') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }
                } // if
            } // for
        } // function
    </script>
<div class="dashed"></div>
<div class="pagewidth">


    <div id="divCCPrintListContainer">
        <div id="divCCPrintList" style="display: none;">
            <asp:UpdatePanel ID="upnlCCPrintList" runat="server">

                <ContentTemplate>
                    <asp:PlaceHolder ID="phrPrintCCList" runat="server">
                        <div class="ui-dailog-body" style="height: 592px">
                            <div id="PrintCCReport" class="PrintListcontainer" style="height: 1000px; width: 100%;">

                                <link href="../PrintStyle.css" type="text/css" rel="stylesheet" media="print" />

                                <table cellpadding="1" cellspacing="2" border="0" width="100%" style="padding: 10px">
                                    <tr>
                                        <%-- <td> <img src="../Images/RT_Logo_icon.jpg" border="0" class="OnlyForPrint"  /></td>--%>
                                        <td>
                                            <img src="../Images/Logo_Header_falcon.jpg" border="0" class="OnlyForPrint" /></td>

                                        <td><span class="OnlyForPrint"><font class="SubHeading3">Cycle Count Note</font></span></td>
                                        <td style="text-align: right;"></td>

                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Literal ID="ltCCDetails" runat="server" />
                                        </td>
                                        <td align="right"></td>
                                        <tr>
                                            <td>&nbsp</td>

                                        </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView Font-Size="Smaller" Width="100%" CellPadding="5" ShowFooter="false" GridLines="Both" ID="gvCCPrintCCList" ShowHeaderWhenEmpty="true" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True" HorizontalAlign="Left" OnRowCommand="gvCCPrintCCList_OnRowCommand" OnSorting="gvCCPrintCCList_Sorting" OnPageIndexChanging="gvCCPrintCCList_PageIndexChanging" OnRowDataBound="gvCCPrintCCList_RowDataBound" OnRowEditing="gvCCPrintCCList_RowEditing" OnRowUpdating="gvCCPrintCCList_RowUpdating" OnRowCancelingEdit="gvCCPrintCCList_RowCancelEditing">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="90" HeaderText="Part Number" ItemStyle-CssClass="BarCodeCell" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                               <asp:Literal runat="server" ID="ltBarCodeMCode" Text='<%# String.Format("<img src=\"../mInbound/Code39Handler.ashx?code={0}\"",DataBinder.Eval(Container.DataItem, "MCode")) %>'/><br /><br />
                                                            <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="90" HeaderText="OEM No." ItemStyle-CssClass="BarCodeCell" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" CssClass="BarCodetext" ID="lblOEMNo" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField ItemStyle-Width="30" HeaderText="Base UoM" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltBMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0}").ToString() )%>' />
                                                            <asp:Literal runat="server" ID="ltMMID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltSplitLocation" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation") %>' />
                                                            <asp:Literal runat="server" ID="ltLocation" Visible="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;">
                                <%--<asp:ImageButton ID="lmbWorkOrderReport"  OnClientClick="printDiv('divPrintListcontainer');" ToolTip="Print JRP" CssClass="btn btn-primary NoPrint"  runat="server"></asp:ImageButton>--%>
                                <a id="lnkCloseCCPrint" class="btn btn-primary NoPrint" onclick="closePrintCCListDialog()">Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a><a id="lnkWorkOrederReport" onclick="printDiv('divCCPrintListContainer');" class="btn btn-primary NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a><%--<asp:LinkButton ID="lnkCloseCCPrint" runat="server" CssClass="btn btn-primary" OnClick="lnkCloseCCPrint_Click" > Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>--%><%--<asp:LinkButton ID="lnkWorkOrederReport" OnClientClick="printDiv('divPrintListcontainer');" CssClass="btn btn-primary NoPrint" runat="server">
                                    Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %>
                                </asp:LinkButton>--%></div>
                        </div>
                    </asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div id="divEditCCDlgContainer">
        <div id="divEditCC" style="display: none;">
            <asp:UpdatePanel ID="upnlEditCC" runat="server">

                <ContentTemplate>

                    <asp:PlaceHolder ID="phrEditCC" runat="server">
                        <div class="ui-dailog-body" style="height: 472px; padding: 10px;">

                            <table width="100%">
                                <tr>
                                    <td class="FormLabels">
                                        <asp:RequiredFieldValidator ID="rfvtxtCCName" SetFocusOnError="true" ControlToValidate="txtCCName" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="CCCreation" />
                                        CC Name :
              <br />
                                        <asp:TextBox runat="server" ID="txtCCName" CssClass="txt_slim" Width="200" />
                                    </td>

                                    <td class="FormLabels">
                                        <asp:RequiredFieldValidator ID="rfvddlHandlerID" SetFocusOnError="true" ControlToValidate="ddlHandlerID" InitialValue="0" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="CCCreation" />
                                        CC Assigned To :<br />
                                        <asp:DropDownList ID="ddlHandlerID" runat="server" CssClass="ddl_slim_req" Width="140" Height="34" />
                                        <asp:Literal ID="lthidCCID" runat="server" Visible="false" />
                                    </td>

                                </tr>

                                <tr>
                                    <td colspan="2">

                                        <b>Select Material for this Cycle Count</b><br />
                                        <br />
                                        <asp:Panel ID="pnlCCMatList" runat="server">
                                            <asp:GridView ShowFooter="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvCCMatList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="30" AllowSorting="True" SkinID="gvLightGreenNew" HorizontalAlign="Left" OnSorting="gvCCMatList_Sorting" OnPageIndexChanging="gvCCMatList_PageIndexChanging" OnRowDataBound="gvCCMatList_RowDataBound" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20" HeaderText="S.No." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="80" HeaderText="Part Number" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                            &nbsp;&nbsp;
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="80" HeaderText="OEM No." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOEMCode" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                            &nbsp;&nbsp;
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField ItemStyle-Width="40" HeaderText="Qty.on Hand" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltQtyOnHand" Text='<%# DataBinder.Eval(Container.DataItem, "QtyOnHand") %>' /><br />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField ItemStyle-Width="30" HeaderText="Sel. for CC" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelforCCAll" runat="server" onclick="return check_uncheck (this );" />
                                                            Sel. for CC.
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Literal ID="lthidMMID" Visible="false" Text='<%# DataBinder.Eval (Container.DataItem, "MaterialMasterID") %>' runat="server" />
                                                            <asp:CheckBox ID="chkSelforCC" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsInCCDetails")) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
                                                <FooterStyle CssClass="gvLightGreen_footerGrid" />
                                                <RowStyle CssClass="gvLightGreen_DataCellGrid" />
                                                <EditRowStyle CssClass="gvLightGreen_DataCellGridEdit" />
                                                <PagerStyle CssClass="gvLightGreen_pager" />
                                                <HeaderStyle CssClass="gvLightGreen_headerGrid" />
                                                <AlternatingRowStyle CssClass="gvLightGreen_DataCellGridAlt" />
                                            </asp:GridView>
                                        </asp:Panel>

                                    </td>
                                </tr>


                            </table>

                        </div>

                    </asp:PlaceHolder>
                    <div class="ui-dailog-footer">
                        <div style="padding: 15px 13px 15px 5px;">
                            <a id="lnkCancleCCWithMMLis" class="btn btn-primary" onclick="closeDialog();">Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a><!--OnClick="lnkCancleCCWithMMLis_Click"--><asp:LinkButton ID="lnkSaveCCWithMMList" runat="server" CssClass="btn btn-primary" OnClick="lnkSaveCCWithMMList_Click" ValidationGroup="CCCreation"> 
                       Save<%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>


            </asp:UpdatePanel>
        </div>
    </div>

    <table align="center">
        <tr>
            <td>
                <br />
                <span class="SubHeading">Standard Cycle Count Manager  </span></td>
        </tr>
        <tr>
            <td>
                <table cellpadding="5" cellspacing="5" border="0" width="100%">
                    <tr>

                        <td>
                          <%-- Header Part--%>

                            <div>
                                <div class="row">
                                    <div class="col m2">
                                        <div class="flex">
                                            <%--<asp:Literal ID="lclSupplier" runat="server" Text="Supplier" />--%>
                                            <asp:TextBox ID="txtSupplier" runat="server"  SkinID="txt_Req" required="">
                                            </asp:TextBox>
                                            <label><asp:Literal ID="lclSupplier" runat="server" Text="Supplier" /></label>
                                            <asp:HiddenField runat="server" ID="hidSupplier" />
                                            </div>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <asp:UpdatePanel ID="pnlUpdate" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>

                                    <div style="">
                                        <div class="row">
                                            <div class="col m3" valign="bottom" halign="left">
                                                <div class="flex">
                                                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="ddl_slim" OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged" AutoPostBack="true" required="" />
                                                <span class="errorMsg">*</span>       
                                                <label>Warehouse</label></div>
                                            </div>

                                            <div class="col m2" valign="bottom">
                                                <div class="flex">
                                                  <asp:DropDownList ID="ddlLocationCode" runat="server" required="" />
                                                  <label>Zone</label>
                                                 </div>
                                            </div>
                                            <div class="col m2" valign="bottom">
                                                 <div class="flex">
                                                    <asp:DropDownList ID="ddlRack" runat="server" required=""/>
                                                    <label>Rack</label></div>
                                            </div>
                                              <div class="col m2" valign="bottom">
                                                  <div class="flex">
                                                    <asp:DropDownList ID="ddlbay" runat="server" required="" />
                                                    <label>Bay</label></div>
                                            </div>
                                              <div class="col m2" valign="bottom">
                                                  <div class="flex">
                                                    <asp:DropDownList ID="ddlBin" runat="server" required="" />
                                                      <label>Bin</label></div>
                                                  </div>
                                            <div class="auto-style1" valign="bottom">
                                                <asp:LinkButton ID="lnkGetCycleClassData" runat="server" OnClientClick="if(!ValidateClientSide()){return false;}" OnClick="lnkGetCycleClassData_Click" CssClass="btn btn-primary right" ValidationGroup="GetCCData">Get Details<%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>

                            </asp:UpdatePanel>






                        </td>

                    </tr>

                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="ltStatus" /><br />
                            <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Always" runat="server">

                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>

                                            <td class="TitleHead" valign="top">&nbsp;
                                                    <font color='#00A382'>
													Cycle Count Manager
													 </font>
                                            </td>
                                            <td align="right" valign="top" class="box_top">
                                                <asp:LinkButton runat="server" ID="lnkAddNewCC" CssClass="btn btn-primary right" OnClick="lnkAddNewCC_Click">Add Cycle Count<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="3">
                                                <br />
                                                <asp:Literal runat="server" ID="Literal2" />
                                                <asp:Panel ID="Panel3" runat="server" ScrollBars="Vertical" Width="99%" Height="250px">
                                                    <asp:GridView Width="100%" ShowFooter="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvCycleCountManager" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" CssClass="overallGrid" HorizontalAlign="Left" OnSorting="gvCycleCountManager_Sorting" OnPageIndexChanging="gvCycleCountManager_PageIndexChanging" OnRowDataBound="gvCycleCountManager_RowDataBound" OnRowCommand="gvCycleCountManager_RowCommand" SkinID="gvLightGreenNew" Visible="true">
                                                        <Columns>

                                                            <asp:TemplateField ItemStyle-Width="20" HeaderText="CC.ID." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCycleCountID" Text='<%# DataBinder.Eval(Container.DataItem, "CycleCountID") %>' />
                                                                    &nbsp;&nbsp;
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCCName" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                                                                    &nbsp;&nbsp;
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Handler" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltHandler" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>' /><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Created On/By" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCreatedOn" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedOn","{0: dd/MM/yy}") %>' /><br />
                                                                    <asp:Literal runat="server" ID="ltCreatedBy" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedByUserName") %>' /><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="Init. Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltInitDate" Text='<%# DataBinder.Eval(Container.DataItem, "CCDateInitiated","{0: dd/MM/yy | hh:mm}") %>' /><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="Close Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCloseDate" Text='<%# DataBinder.Eval(Container.DataItem, "CCDateClosed","{0: dd/MM/yy | hh:mm}") %>' /><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="20" HeaderText="Active" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltActive" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="Print CC List" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPrintCCJobList" CssClass="GvLink" runat="server" Text="<nobr> Print <img src='../Images/redarrowright.gif' border='0' /></nobr>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CycleCountID") %>' CommandName="PrintCCList" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="Edit CC List" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkModify" runat="server" CssClass="GvLink" Text="<nobr> Modify <img src='../Images/redarrowright.gif' border='0' /></nobr>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CycleCountID") %>' CommandName="ModifyCCMatList" />

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="Copy CC" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCopy" runat="server" CssClass="GvLink" Text="<nobr> Copy <img src='../Images/redarrowright.gif' border='0' /></nobr>" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CycleCountID") %>' CommandName="CopyCCMatList" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Control" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltControlFlag" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsOn") %>' />
                                                                    <asp:ImageButton ID="lnkStartStop" runat="server" ImageUrl="../Images/start.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CycleCountID") %>' CommandName="StartStop" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                        </Columns>
                                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                                                            Mode="NumericFirstLast" PageButtonCount="15" />
                                                        <FooterStyle CssClass="gvLightGreen_footerGrid" />
                                                        <RowStyle CssClass="gvLightGreen_DataCellGrid" />
                                                        <EditRowStyle CssClass="gvLightGreen_DataCellGridEdit" />
                                                        <PagerStyle CssClass="gvLightGreen_pager" />
                                                        <HeaderStyle CssClass="gvLightGreen_headerGrid" />
                                                        <AlternatingRowStyle CssClass="gvLightGreen_DataCellGridAlt" />
                                                    </asp:GridView>
                                                </asp:Panel>

                                            </td>
                                            <td class="box_right"></td>
                                        </tr>
                                    </table>
                                    <asp:LinkButton ID="btnRefreshgvCycleCountManager" CausesValidation="false" OnClick="btnRefreshgvCycleCountManager_Click" Style="display: none" runat="server"></asp:LinkButton>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvCycleCountManager" />
                                </Triggers>
                            </asp:UpdatePanel>

                            <br />
                            <asp:UpdatePanel ID="upnlgvCCABC" UpdateMode="Always" runat="server" RenderMode="Block" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCCItemSearch" />
                                    <asp:AsyncPostBackTrigger ControlID="gvCCABC"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvCCABC"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvCCABC"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvCCABC"></asp:AsyncPostBackTrigger>
                                </Triggers>
                                <ContentTemplate>

                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>

                                            <td>
                                                <div id="divCCRecordCount" runat="server"></div>
                                                <asp:Panel ID="pnlCCSearch" runat="server" CssClass="SearchPanel" HorizontalAlign="right" DefaultButton="lnkCCItemSearch">
                                                  <div class="flex__ right">
                                                        <div class="flex">
                                                          <div class="fixselect">
                                                            <asp:DropDownList runat="server" ID="ddlCycleClass" required="" >
                                                                <asp:ListItem Text="A" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                              <label> Cycle Class</label></div>
                                                      </div>
                                                        <div class="flex__">
                                                            <div class="flex">
                                                                <asp:TextBox ID="txtMCode" CssClass="txt_slim_small" runat="server" required=""/>
                                                                <label>Part Number</label>
                                                            </div>
                                                            <asp:LinkButton ID="lnkCCItemSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkCCItemSearch_Click1">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton>
                                                        </div>
                                                     </div>
                                                </asp:Panel>
                                                </nobr>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal runat="server" ID="ltShipmentExpectedStatus" />
                                                <br />
                                                <asp:Panel ID="pnlgvCCABC" runat="server" ScrollBars="Vertical" Width="99%" HorizontalAlign="Left">

                                                    <asp:GridView Width="100%" ShowFooter="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvCCABC" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="30" AllowSorting="True" SkinID="gvLightGreenNew" CssClass="overallGrid" HorizontalAlign="Left" OnSorting="gvCCABC_Sorting" OnPageIndexChanging="gvCCABC_PageIndexChanging" OnRowDataBound="gvCCABC_RowDataBound">


                                                        <Columns>

                                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="S.No." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="140" HeaderText="Part Number" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                                    &nbsp;&nbsp;
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField ItemStyle-Width="140" HeaderText="OEM Number" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltOEMPart" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                                    &nbsp;&nbsp;
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="No.of Movements" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltTotGoodMov" Text='<%# DataBinder.Eval(Container.DataItem, "TotGoodMov") %>' /><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Qty.on Hand" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltQtyOnHand" Text='<%# DataBinder.Eval(Container.DataItem, "QtyOnHand") %>' /><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Jan" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltmothnsdata" Text='<%# DataBinder.Eval(Container.DataItem, "CCMonths") %>' runat="server" Visible="false"></asp:Literal>

                                                                    <asp:Literal runat="server" ID="ltJan"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Feb" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltFeb"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Mar" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltMar"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Apr" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltApr"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="May" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltMay"></asp:Literal>


                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Jun" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltJun"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Jul" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltJul"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Aug" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltAug"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Sep" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltSep"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Oct" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltOct"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Nov" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltNov"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Dec" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltDec"></asp:Literal>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>




                                                        </Columns>
                                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                                                            Mode="NumericFirstLast" PageButtonCount="15" />
                                                        <FooterStyle CssClass="gvLightGreen_footerGrid" />
                                                        <RowStyle CssClass="gvLightGreen_DataCellGrid" />
                                                        <EditRowStyle CssClass="gvLightGreen_DataCellGridEdit" />
                                                        <PagerStyle CssClass="gvLightGreen_pager" />
                                                        <HeaderStyle CssClass="gvLightGreen_headerGrid" />
                                                        <AlternatingRowStyle CssClass="gvLightGreen_DataCellGridAlt" />
                                                    </asp:GridView>
                                                </asp:Panel>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="box_right"></td>
                                        </tr>
                                    </table>
                                    <asp:LinkButton ID="btnRefreshGrid" CausesValidation="false" OnClick="btnRefreshGrid_Click" Style="display: none" runat="server"></asp:LinkButton>
                                </ContentTemplate>

                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvCCABC" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!--For Printing CycleCOunt Tally Report-->
    <div id="reportDailog">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <table align="center">
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCyclecountID" ValidationGroup="validateccid" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:Label ID="Label1" runat="server" Text="Label" CssClass="SubHeading">CyccleCountID</asp:Label>
                            <asp:TextBox ID="txtCyclecountID" runat="server" onblur="validateCC()" Height="22px"></asp:TextBox>
                            <asp:LinkButton ID="lnkgo" runat="server" CssClass="ButSearch" OnClick="lnkgo_Click">GO</asp:LinkButton>
                            <asp:LinkButton ID="lnkreportprint" Style="visibility: hidden;" runat="server" CssClass="ButPrint" OnClientClick="PrinForm()">Print</asp:LinkButton>
                            <!--<a href="javascript:printDiv('PrinForm');" class="ButPrint" style="visibility:hidden;" runat="server" id="lnkprint">Print</a>-->
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server">
                    <div id="printreport">
                        <link href="PrintStyle.css" type="text/css" rel="stylesheet" media="print">
                        <table cellpadding="1" cellspacing="1" border="0" align="center">
                            <tr>
                                <td>
                                    <img src="images/atc_logo_w100.jpg" border="0" class="OnlyForPrint" />

                                </td>
                                <td><span class="OnlyForPrint"><font size="18">Cycle Count Report</font></span>

                                </td>
                                <td style="text-align: right;">
                                    <img src="images/inventrax_logo_100x41.gif" border="0" class="OnlyForPrint" align="right" />

                                </td>

                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblCCDetails" runat="server" Width="610" Font-Size="10" CssClass="ccdetails"></asp:Label>
                                    <asp:Label ID="lblmsg" runat="server" Width="610" Font-Size="10" CssClass="errorMsg"></asp:Label>
                                </td>
                            </tr>


                            <tr>
                                <td colspan="3">


                                    <asp:GridView Width="100%" ShowHeaderWhenEmpty="true" CellPadding="5" ShowFooter="false" GridLines="Both" ID="gvCCreport" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True" HorizontalAlign="Left" OnRowDataBound="gvCCreport_RowDataBound">
                                      
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Part Number" ItemStyle-CssClass="BarCodeCell" HeaderStyle-HorizontalAlign="Center">

                                                <ItemTemplate>
                                                    <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                    <br />
                                                </ItemTemplate>



                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="BUOM/Qty" ItemStyle-CssClass="BarCodeCell" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltUOM" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BUoM") %>' />
                                                    <asp:Literal ID="text" runat="server" Text="/" />
                                                    <asp:Literal ID="ltQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BUoMQty") %>' />
                                                    <br />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSplitLocationforReport" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation") %>' />
                                                    <asp:Literal runat="server" ID="ltLocationforReport" Visible="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>

                        </table>
                    </div>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="testdiv">
    </div>
    </div>

</asp:Content>
