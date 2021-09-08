<%@ Page Title="Active Stock:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.Master" AutoEventWireup="true" CodeBehind="ActiveStock.aspx.cs" Inherits="MRLWMSC21.mInventory.ActiveStock" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="InvContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="Scripts/jquery.blockUI.js"></script>

    <script>
        function check_FacDetailsuncheck(val) {
            var ValChecked = val.checked;
            var ValId = val.id;
            var frm = document.forms[0];
            for (i = 0; i < frm.length; i++) {
                if (this != null) {
                    if (ValId.indexOf('FacDetailsCheckAll') != -1) {
                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].className == 'print') {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                }
            }
        }

        function check_All() {
            var cbs = document.getElementsByClassName("print");
            var count = 0;
            for (var i = 0; i < cbs.length; i++) {
                if (cbs[i].checked)
                    count++;
                else
                    break;
            }
            if (count == cbs.length)
                document.getElementById('FacDetailsCheckAll').checked = true;
            else
                document.getElementById('FacDetailsCheckAll').checked = false;
        }
    </script>

    <script type = "text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadAutocompletes();
            }
        }

        function fnLoadAutocompletes() {
            buildfixedHeaders();
        }
        var GridId = "<%=gvActiveStock.ClientID %>";
        var ScrollHeight = 850;
        window.onload = function () {

            buildfixedHeaders();
        }

        function buildfixedHeaders() {
            var grid = document.getElementById(GridId);
            if (grid != null) {
                var gridWidth = grid.offsetWidth;
                var gridHeight = grid.offsetHeight;
                var headerCellWidths = new Array();
                for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                    if (i == 0)
                        headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth + 10;
                    else
                        headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
                }
                grid.parentNode.appendChild(document.createElement("div"));
                var parentDiv = grid.parentNode;

                var table = document.createElement("table");
                for (i = 0; i < grid.attributes.length; i++) {
                    if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                        table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                    }
                }
                table.style.cssText = grid.style.cssText;
                table.style.width = gridWidth - 2 + "px";
                table.appendChild(document.createElement("tbody"));
                table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
                var cells = table.getElementsByTagName("TH");

                var gridRow = grid.getElementsByTagName("TR")[0];
                for (var i = 0; i < cells.length; i++) {
                    var width;
                    if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                        width = headerCellWidths[i];
                    }
                    else {
                        width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                    }
                    cells[i].style.width = parseInt(width - 3) + "px";
                    gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
                }
                parentDiv.removeChild(grid);

                var dummyHeader = document.createElement("div");
                dummyHeader.appendChild(table);
                parentDiv.appendChild(dummyHeader);
                var scrollableDiv = document.createElement("div");
                if (parseInt(gridHeight) > ScrollHeight) {
                    gridWidth = parseInt(gridWidth) + 17;
                }
                scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
                scrollableDiv.appendChild(grid);
                parentDiv.appendChild(scrollableDiv);
            }
        }

        </script>

    <style>
        /*td {
            padding-right:2px;
            padding-left:2px;
        }*/
        img
        {  
            border-style: none;
            
        }
        .HeaderDesccss {
            min-width:75px;
            max-width:75px;
        }
        .Desccss {
            min-width:75px;
            max-width:75px;
            word-wrap: break-word;
        }
        .BaseCss {
            min-width:64px;
        }
        .Headerpartcodecss {
            min-width:158px;
            max-width:158px;
        }
        .partcodecss {
            min-width:150px;
            max-width:150px;
        }
       
    </style>

    <style type="text/css">
        .gvLightGreenNew table {
        table-layout:fixed;
        }
        .gvLightGreenNew table td {
            overflow: hidden;
        }
    </style>

    <script>

        $(document).ready(function () {
            $(".DateBoxCSS_small").datepicker({ dateFormat: "dd/mm/yy" });
            ConfigureMSP(document.getElementById('<%=this.atcMateialCode.ClientID%>'));

            try {

                var TextFieldName = $("#<%= this.txtTenant.ClientID %>");

                DropdownFunction(TextFieldName);

                $("#<%= this.txtTenant.ClientID %>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
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

                },
                minLength: 0
                });

                var textfieldname = $('#<%=this.atcMateialCode.ClientID%>')
                DropdownFunction(textfieldname);
                $('#<%=this.atcMateialCode.ClientID%>').autocomplete({
                    source: function (request, response) {
                        if ($("#<%=hifTenant.ClientID %>").val() == '0' || $("#<%= this.txtTenant.ClientID %>").val() == "") {
                            showStickyToast(false, "Please select Tenant");
                        }
                        else {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=hifTenant.ClientID%>').val() + "'}",
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

                        }
                        //  alert('ssss');

                    },
                    select: function (e, i) {


                        $("#<%=this.hifMaterialCode.ClientID%>").val(i.item.val);
                        ConfigureMSP(document.getElementById('<%=this.atcMateialCode.ClientID%>'));
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


            var textfieldname = $('#<%=this.txtGRNNumber.ClientID%>')
            DropdownFunction(textfieldname);
            $("#<%=this.txtGRNNumber.ClientID%>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetGRNNumberList") %>',
                        data: "{ 'GRNNumber': '" + request.term + "'}",
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
            });

            var textfieldname = $("#<%= this.txtcarton.ClientID %>");
            DropdownFunction(textfieldname);
            $('#<%=this.txtcarton.ClientID%>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetContainers") %>',
                        data: "{'Prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //if (data.d == "" || data.d == "/") {
                            //    showStickyToast(false, 'No Containers');
                            //}
                            //else response(data.d)
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

                    $("#<%=hdncarton.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

        });
    </script>

    <script>
        function ConfigureMSP(textbox) {

            var mmid = 0;
            try {

                mmid = document.getElementById('<%=this.hifMaterialCode.ClientID%>').value;
                if (mmid == '' || mmid == null)
                    mmid = 0;
            } catch (err) {
            }
            // alert(mmid);

            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/MaterialConfigurationServiceForActiveStock") %>',
                data: "{ 'MaterialId': '" + mmid + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //alert(data.d);

                    if (textbox.value != "") {
                        // alert('TextBox Not Empty');
                        configure(data.d);
                    }
                    else {
                        //alert('TextBox Empty');
                        ShowAllMSps(data.d);
                    }
                    //response(data.d);
                }
            });
        }
        function ShowAllMSps(data) {
            var paramNames = data.split('|');
            var listOfparames = paramNames[0].split(',');


            for (var item = 0; item < listOfparames.length; item++) {
                //alert('block   ' + document.getElementById('lb' + listOfparames[item]));
                try {
                    document.getElementById(listOfparames[item]).style.display = 'block';
                    document.getElementById('lb' + listOfparames[item]).style.display = 'block';
                    //"inline-table";
                    // document.getElementById(listOfparames[item]).width = "15%";
                } catch (err) {
                }
            }


        }


        function configure(data) {

            var paramNames = data.split('|');
            var listOfparames = paramNames[0].split(',');


            for (var item = 0; item < listOfparames.length; item++) {
                // alert('none   ' + document.getElementById('lb' + listOfparames[item]));
                try {
                    document.getElementById(listOfparames[item]).style.display = "none";
                    document.getElementById('lb' + listOfparames[item]).style.display = "none";
                } catch (err) {
                }

            }
            listOfparames = paramNames[1].split(',');
            var percentage = 100 / listOfparames.length;
            for (var item = 0; item < listOfparames.length; item++) {
                try {
                    // alert(percentage);
                    document.getElementById(listOfparames[item]).style.display = 'block';
                    document.getElementById('lb' + listOfparames[item]).style.display = 'block';
                    //"inline-table";
                    //document.getElementById(listOfparames[item]).width = "15%";
                } catch (err) {
                }
            }

        }
    </script>

    <style type="text/css"> 
			.showonhover .hovertext { display: none;}
            .showonhover:hover .hovertext {display: inline;}
            div.viewdescription {color:#999;}
            div.viewdescription:hover {background-color:#999; color: White;}
            .hovertext {position:absolute;z-index:1000;border:2px solid #ffd971;background-color:#9cb70f;padding:5px;width:200px;font-size: 15px;}
        
         
	</style>
    <!-- This is for Attachments-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divViewAttachment").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: '300',
                width: '300',
                resizable: false,
                draggable: false,
                open: function (event, ui) { $(this).parent().appendTo("#divViewAttachmentContainer"); }


            });
        });
        function closeAttachment() {

            //Could cause an infinite loop because of "on close handling"
            $("#divViewAttachment").dialog('close');

        }
        function OpenAttachment(title) {
            $("#divViewAttachment").dialog("option", "title", title);
            $("#divViewAttachment").dialog('open');
            NProgress.start();
        }
        function showAycBlock() {
            $.blockUI({
                message: $('#displayBox'),
                css:
                        {
                            border: 'none',
                            padding: '15px',
                            backgroundColor: '#fff',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: .5,
                            color: '#333',
                            '-ms-filter': 'progid:DXImageTransform.Microsoft.Alpha(Opacity=50)',
                            filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=50)',
                            '-moz-opacity': '.70',
                            opacity: .70
                        }
            });
        }

        function unblockAycDialog() {
            $.unblockUI();
            NProgress.done();
        }

       </script>

    <script  type="text/JavaScript">

        function check_uncheck(Val) {

            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('chkIsDeleteRFItemsAll') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox') {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }

                }

            } // for
        } // function

    </script>
        
    <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }     
    </style> 
    
    <script type="text/javascript">
        function openPartNumberDialogAndBlock(title) {
            OpenAttachment(title);

            NProgress.start();
            $("#divViewAttachment").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inv.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }
        function unblockPartNumberDialog() {
            $("#divViewAttachment").unblock();
            CloseProcess();
            NProgress.done();
        }
    </script>
    <div class="dashed"></div>
    <div  class="pagewidth">
    <div id="divViewAttachmentContainer">  
        <div id="divViewAttachment" style="display:block;">  
            <asp:UpdatePanel ID="upviewattachment" runat="server" >
            <ContentTemplate>
                    <br/><br/><br/>
             
                    <asp:TreeView ID="trvmaterialattachment" Target="_blank" runat="server"  >
                        <Nodes>
                            <asp:TreeNode  Expanded="false"></asp:TreeNode>      
                        </Nodes>
                </asp:TreeView>
                 
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- This is for Attachments End-->
    <table width="1550px" align="center" border="0" style="padding-left:10px;">
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr align="center">
            <td align="left">
                <table width="1000px" border="0" cellspacing="0">
                    <tr>
                        <td>
                             <asp:Literal ID="ltTenant" runat="server" Text="Tenant:" /><br />
                             <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Auto"></asp:TextBox>
                             <asp:HiddenField runat="server" ID="hifTenant" Value="0"/>
                        </td>
                        <td align="left" >
                            <asp:Literal ID="ltMaterialCode" runat="server" Text="Part Number:" /><br />
                            <asp:TextBox ID="atcMateialCode" onblur="ConfigureMSP(this)" skinid="txt_Auto" runat="server"   />
                            <asp:HiddenField ID="hifMaterialCode" runat="server"/>
                        </td>
                       
                        <td align="left" id="tdrightpart" runat="server" width="25%">
                            <asp:Literal ID="ltGRNNumber" runat="server" Text="GRN Number:" /><br />   
                            <asp:TextBox ID="txtGRNNumber" SkinID="txt_Auto" runat="server" /><br />
                            <asp:HiddenField ID="hifGRNNumber" runat="server" />
                       
                        </td>
                       
                        <td>
                            <asp:Literal ID="ltMaterialType" runat="server" Text="Material Type:" /><br />
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="200" >
                            </asp:DropDownList>
                        </td>
                         
                    </tr>
                   
                    <tr>
                        <td>
                            <asp:Literal ID="itWarehose" runat="server" Text="Warehouse:" /><br />
                            <asp:DropDownList ID="ddlWarehouse" runat="server" Width="200" >
                            </asp:DropDownList>
                        </td>
                        <td>
                              <asp:Literal ID="ltcarton" runat="server" Text="Container:" /><br />
                            
                                 <asp:TextBox ID="txtcarton" SkinID="txt_Auto" runat="server" /><br />
                            <asp:HiddenField ID="hdncarton" runat="server" />
                        </td>
                        <td >
                            <asp:Literal ID="ltStorageLocationID" runat="server" Text="Storage Location:" /><br />
                           <asp:DropDownList ID="ddlStorageLocationID" runat="server" Width="200" >
                               <%--<asp:ListItem Value="0" Selected="True">--Please select--</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                           
                           <table border="0" width="76%" cellspacing="0"   runat="server" id="tbMaterialStorageParameter">
                                
                            </table>
                          
                       
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="90%">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkReplenishment" runat="server" Text="Replenishment Due" />
                                    </td>
                                    <td>
                                         <asp:CheckBox ID="chkDamaged" runat="server" Text="Damaged Items" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkDiscripancy" runat="server" Text="Discrepancy Items" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkNonConformity" runat="server" Text="Non Conformity Items" />
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                   
                    <tr>
                        <td colspan="4">
                            <br />
                            <table width="100%">
                                <tr>
                                    <td width="12%">
                                        <asp:Literal ID="ltSite" runat="server" Text="Zone:" /><br />
                                        <asp:DropDownList ID="ddlSite" runat="server" Width="60" />
                                    </td >
                                     <td width="12%">
                                        <asp:Literal ID="ltAisle" runat="server" Text="Rack:" /><br />
                                        <asp:DropDownList ID="ddlAisle" runat="server" Width="60"/>
                                    </td>
                                    
                                     <td width="12%">
                                        <asp:Literal ID="ltBay" runat="server" Text="Bay:" /><br />
                                       <asp:DropDownList ID="ddlBay" runat="server" Width="60"/>
                                    </td>
                                     <td width="11%">
                                        <asp:Literal ID="ltBeam" runat="server" Text="Beam:" /><br />
                                        <asp:DropDownList ID="ddlBeam" runat="server" Width="60"/>
                                    </td>

                                    <td >
                                        <asp:Literal ID="ltPrinter" runat="server" Text="Printer :" />
                                        <br />
                                        <asp:DropDownList ID="ddlNetworkPrinter" runat="server" CssClass="NoPrint" /> &nbsp;&nbsp;&nbsp;
                                    </td>

                                    <td>
                                        <asp:LinkButton ID="lnkGetData"  runat="server" OnClientClick="showAsynchronus();" OnClick="lnkGetData_Click" CssClass="ui-btn ui-button-large">
                                         Get Details <%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
                                        </asp:LinkButton>

                                        &nbsp;&nbsp;&nbsp;&nbsp;

                                        <%--<asp:LinkButton ID="lnkclear" SkinID="lnkButCancel" runat="server" Text="Clear"  />--%>
                                        
                                        <a class="ui-btn ui-button-large" href="ActiveStock.aspx">Clear <span class="space fa fa-ban"></span> </a>


                                    </td>
                                   
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td >
                <table width="99%"  >
                    <tr >
                        <td width="50px">
                            <br />
                           
                            <div style="height:15px;width:50px;background-color:pink;position:relative;"></div>
                        </td>
                        <td>
                           <br />
                            Expired Materials


                        </td>
                        <td align="left" valign="bottom">
                               <asp:ImageButton ID="btnASExprtExcel" runat="server"  ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="btnASExprtExcel_Click" ToolTip="Export To Excel" />
                        </td>
                    </tr>
                </table>
                               
            </td>
        </tr> 

        <tr>
            <td>
                <table width="100%" cellpadding="0">
                    <tr>
                        <td >
                            <asp:Label runat="server" ID="lblRecordCount" CssClass="SubHeading3" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                            <asp:Label ID="lbMateialsStatus" runat="server" ></asp:Label>
                             <table width="100%">
                                    <tr>
                                        <td >
                                            Display &nbsp;
                                            <asp:DropDownList runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="drppagesize_SelectedIndexChanged" ID="drppagesize">
                                                <asp:ListItem Text="10" Value="10" Selected="True"/>
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="30" Value="30"  />
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                </table>
                            
                            <asp:UpdatePanel ID="upnlActiveStock" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:DataList CellPadding="10" RepeatDirection="Horizontal" HorizontalAlign="Right" runat="server" ClientIDMode="Static" ID="dlPagerupper" OnItemCommand="dlPager_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" class="page-numbers" OnClientClick="showAsynchronus();" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                                                </ItemTemplate>
                                            </asp:DataList>

                                      

                                           <asp:GridView  SkinID="gvLightGreenNew" PageSize="100"   ID="gvActiveStock"  runat="server"  AutoGenerateColumns="false" OnRowDataBound="gvActiveStock_RowDataBound" OnPageIndexChanging="gvActiveStock_PageIndexChanging" AllowPaging="true" ShowHeader="true" OnRowCommand="gvActiveStock_RowCommand" >
                                                <Columns>
                                   
                                  
                                                    <asp:TemplateField HeaderText="Part Number" HeaderStyle-HorizontalAlign="left" ItemStyle-CssClass="partcodecss" HeaderStyle-CssClass="Headerpartcodecss">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltMaterialCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' /><br/>
                                            
                                            
                                                            <span class="showonhover">
                                                                    <asp:LinkButton runat="server" Font-Underline="false" ID="viewattachment" CommandName="viewpics" CommandArgument='<%# String.Format("{0},{1},{2}",DataBinder.Eval(Container.DataItem, "MaterialMasterID"),DataBinder.Eval(Container.DataItem,"MCode"),DataBinder.Eval(Container.DataItem, "TenantID") )%>'  OnClientClick="openPartNumberDialogAndBlock('View Attachments');NProgress.start();">     
                                                                        <asp:Literal runat="server"  ID="ltMCodeExcel" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>'/>
                                                                        </asp:LinkButton><br>
                                                                        <asp:Literal ID="ltOEMPartNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                                    <span class="hovertext">Available Quantity: <%# DataBinder.Eval(Container.DataItem, "AvailableQuantity") %><br/>Replenishment Quantity: <%# DataBinder.Eval(Container.DataItem, "ReorderPoint") %></span></span><br /></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderStyle-CssClass="HeaderDesccss" ItemStyle-CssClass="Desccss" HeaderText="Tenant" HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltTenantName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="30" ItemStyle-Width="52" HeaderText="In Act." HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <a target="_blank" Title="Goods In Details" style="text-decoration:none;" href='<%# String.Concat("ActivityReport.aspx?mmid=",DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString()+"&TransactionType=1"+"&tid=",DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>'><image src="../Images/GoodsIn.gif"></image></a>
                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="30" ItemStyle-Width="52"  HeaderText="Out Act." HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <a target="_blank" Title="Goods Out Details" style="text-decoration:none;" href='<%# String.Concat("ActivityReport.aspx?mmid=",DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString(),"&TransactionType=2"+"&tid=",DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>'><image src="../Images/GoodsOut.gif"></image></a>
                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="HeaderDesccss" ItemStyle-CssClass="Desccss" HeaderText="Desc." HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltMDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="BaseCss" ItemStyle-CssClass="BaseCss" HeaderText="BUoM / Qty." HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltBUoMQty" runat="server" Text='<%# string.Format("{0} / {1}",DataBinder.Eval(Container.DataItem, "BUoM"),DataBinder.Eval(Container.DataItem, "BUoMQty")) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="BaseCss" ItemStyle-CssClass="BaseCss" HeaderText="MUoM / Qty." HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltMUoMQty" runat="server" Text='<%# string.Format("{0} / {1}",DataBinder.Eval(Container.DataItem, "MUoM"),DataBinder.Eval(Container.DataItem, "MUoMQty")) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                      <asp:TemplateField HeaderStyle-CssClass="BaseCss" ItemStyle-CssClass="BaseCss" HeaderText="Storage Location" HeaderStyle-HorizontalAlign="left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltStorageLocationID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StorageLocationID") %>' />
                                                            <asp:Literal ID="ltstoragecode"  runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:TemplateField FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Underline="false">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltSplitLocation" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation") %>'/>
                                                            <asp:Literal ID="ltMSPs" runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton Text="Print" ID="lnkPrint" OnClick="lnkPrint_Click" CssClass="ButEmpty" runat="server"></asp:LinkButton></FooterTemplate></asp:TemplateField></Columns></asp:GridView><asp:DataList CellPadding="10" RepeatDirection="Horizontal" HorizontalAlign="Right" runat="server" ClientIDMode="Static" ID="dlPager" OnItemCommand="dlPager_ItemCommand">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CssClass="page-numbers" OnClientClick="showAsynchronus();" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ContentTemplate>

                            </asp:UpdatePanel>
                            <br />
                               
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        </div>
    <asp:HiddenField runat="server" ID="hfpageId" Value="1" />
    <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
    	    <ContentTemplate>
            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
	    </ContentTemplate>
    </asp:UpdatePanel> 
       
    
</asp:Content>
