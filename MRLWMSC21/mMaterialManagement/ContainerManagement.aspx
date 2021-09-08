<%@ Page Title=" Container Management :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="ContainerManagement.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.ContainerManagement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="IBContent" runat="server">
    <asp:ScriptManager runat="server" ID="smngrDPN" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>


     <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
        <script src="ContainerManagement.js"></script>

<%--    <script src="Scripts/BrowserPrint-1.0.4.js"></script>

    <script src="Scripts/DevDemo.js"></script>--%>

    <script language="javascript" type="text/JavaScript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadAutocompletes();
            }
        }

        function fnLoadAutocompletes() {
            var TextFieldName = $("#txtContainertype");
            DropdownFunction(TextFieldName);
            $("#txtContainertype").autocomplete({
                source: function (request, response) {
                    if ($("#txtContainertype").val() == '') {
                        $("#hifContainertype").val('0');
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetContainerType") %>',
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
                        }
                    });
                },
                select: function (e, i) {

                    $("#hifContainertype").val(i.item.val);

                },
                minLength: 0
            });



            var TextFieldName = $("#txtWareHouse");
            DropdownFunction(TextFieldName);
            $("#txtWareHouse").autocomplete({
               
                source: function (request, response) {
                    if ($("#txtContainertype").val() == '') {
                        $("#hifContainertype").val('0');
                    }
                    $.ajax({
                       <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetWareHouse") %>',
                            data: "{ 'prefix': '" + request.term + "'}",--%>
                      <%--  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseForCyccleCount") %>',--%>                       
                  <%--      url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouse") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" +<%=cp1.AccountID%>+ "'}",//<=cp.TenantID%>--%>


                        url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                        data: "{ 'prefix': '" + request.term + "'  }",  // added by Ganesh @sep 28 2020

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

                    $("#hifWareHouse").val(i.item.val);

                },
                minLength: 0
            });



            var TextFieldName = $("#<%= this.txtNetWorkPrinter.ClientID %>");
            DropdownFunction(TextFieldName);
            $("#<%= this.txtNetWorkPrinter.ClientID %>").autocomplete({
                source: function (request, response) {
                    if ($("#<%= this.txtNetWorkPrinter.ClientID %>").val() == '') {
                        $("#<%=hifNetworkPrinter.ClientID %>").val('0');
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetClientResource") %>',
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
                        }
                    });
                },
                select: function (e, i) {

                    $("#<%=hifNetworkPrinter.ClientID %>").val(i.item.val);

                },
                minLength: 0
            });
        }
        $(document).ready(function () {


            fnLoadAutocompletes();
        });
        function check_uncheck(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement
                if (this != null) {

                    if (ValId.indexOf('CheckAll') != -1) {

                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes
                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsPrint') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkIsPrint') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }
                } // if
            } // for
        } // function

        function Succes() {
            showStickyToast(true, "Successfully Created", false);
            setTimeout(function () {
                // location.reload();
            }, 400);
        }

        function DelSuccess() {
            if (confirm("Are you sure do you want to delete ?")) {

                showStickyToast(true, "Successfully Deleted", false);
                setTimeout(function () {
                    //location.reload();
                }, 400);
            }
            else {
                return false
            }
        }



    </script>

    <script type="text/javascript">

        var wcppGetPrintersTimeout_ms = 10000; //10 sec
        var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec

        function wcpGetPrintersOnSuccess()
        {
            // Display client installed printers
            if (arguments[0].length > 0) {
                var p = arguments[0].split("|");
                var options = '';
                for (var i = 0; i < p.length; i++) {
                    options += '<option>' + p[i] + '</option>';
                }
                $('#installedPrinterName').html(options);
                $('#installedPrinterName').focus();
                $('#loadPrinters').hide();
            } else {
                // alert("No printers are installed in your system.");
                showStickyToast(false, "Please install Web Client Print Software in your PC", false);
                return false;
            }
        }

        function wcpGetPrintersOnFailure() {
            // Do something if printers cannot be got from the client 
            //alert("No printers are installed in your system.");
            showStickyToast(false, "No printers are installed in your system", false);
            return false;
        }
    </script>
    <script type="text/javascript">

        function doClientPrint() {
            //debugger;
            //collect printer settings and raw commands
            var printerSettings = $("#myForm :input").serialize();

            //store printer settings in the server cache...
            $.post('ContainerWebClientPrintDemo.ashx',
                printerSettings
            );

            // Launch WCPP at the client side for printing...
            var sessionId = $("#sid").val();
            jsWebClientPrint.print('sid=' + sessionId);

        }


        $(document).ready(function () {

            //jQuery-based Wizard
            $("#myForm").formToWizard();

            //change printer options based on user selection
            $("#pid").change(function () {
                var printerId = $("select#pid").val();
                hidePrinters();
                if (printerId == 2) {
                    $("#installedPrinter").show();
                    // $("#installedPrinterName").removeAttr("disabled");
                    javascript: jsWebClientPrint.getPrinters();


                }
                else if (printerId == 3) {
                    $("#installedPrinter").hide();
                    $("#netPrinter").show();
                }
                else if (printerId == 4) {
                    $("#installedPrinter").hide();
                    $("#parallelPrinter").show();
                }
                else if (printerId == 5) {
                    $("#installedPrinter").hide();
                    $("#serialPrinter").show();
                }
            });

            hidePrinters();
        });

        function hidePrinters() {
            $("#installedPrinter").hide();
            $("#netPrinter").hide();
            $("#parallelPrinter").hide();
            $("#serialPrinter").hide();
        }




        /* FORM to WIZARD */
        /* Created by jankoatwarpspeed.com */

        (function ($) {
            $.fn.formToWizard = function () {

                var element = this;

                var steps = $(element).find("fieldset");
                var count = steps.size();


                // 2
                $(element).before("<ul id='steps'></ul>");

                steps.each(function (i) {
                    $(this).wrap("<div id='step" + i + "'></div>");
                    $(this).append("<p id='step" + i + "commands'></p>");

                    // 2
                    var name = $(this).find("legend").html();
                    $("#steps").append("<li id='stepDesc" + i + "'>Step " + (i + 1) + "<span>" + name + "</span></li>");

                    if (i == 0) {
                        createNextButton(i);
                        selectStep(i);
                    }
                    else if (i == count - 1) {
                        $("#step" + i).hide();
                        createPrevButton(i);
                    }
                    else {
                        $("#step" + i).hide();
                        createPrevButton(i);
                        createNextButton(i);
                    }
                });

                function createPrevButton(i) {
                    var stepName = "step" + i;
                    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Prev' class='prev btn btn-info'>< Back</a>");

                    $("#" + stepName + "Prev").bind("click", function (e) {
                        $("#" + stepName).hide();
                        $("#step" + (i - 1)).show();

                        selectStep(i - 1);
                    });
                }

                function createNextButton(i) {
                    var stepName = "step" + i;
                    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Next' class='next btn btn-info'>Next ></a>");

                    $("#" + stepName + "Next").bind("click", function (e) {
                        $("#" + stepName).hide();
                        $("#step" + (i + 1)).show();

                        selectStep(i + 1);
                    });
                }

                function selectStep(i) {
                    $("#steps li").removeClass("current");
                    $("#stepDesc" + i).addClass("current");
                }

            }
        })(jQuery);

    </script>

        <style>
         /* Absolute Center Spinner */
        .loading
        {
            position: fixed;
            z-index: 9999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            .loading:before
            {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required)
            {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after
                {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>

    <div class="module_yellow">
              <div class="ModuleHeader" height="35px">
                   <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <a href="#">Administration</a> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Container Management </span></div>
                  
                  
              </div>
             
          </div>
    <div class="container" ng-app="myApp" ng-controller="ContainerManagement">
        <div class="loading" id="divLoading" style="display: none;"></div>
           <asp:HiddenField runat="server" ID="hifWareHouse" Value="0" ClientIDMode="Static"/>
         <asp:HiddenField runat="server" ID="hifContainertype" Value="0" ClientIDMode="Static"/>
         <asp:HiddenField runat="server" ID="hifNetworkPrinter" Value="0" ClientIDMode="Static"/>


        <div class="row">
            <div class="col m3 s3">
                <div class="flex">
                    <asp:DropDownList ID="ddlwarehouse1" Style="display: none;" runat="server" CssClass="NoPrint" required="" />
                    <asp:TextBox ID="txtWareHouse" runat="server" SkinID="txt_Hidden_Req_Auto" required="" ClientIDMode="Static"></asp:TextBox>
                    <label><%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3 s3">
                <div class="flex">
                    <asp:DropDownList ID="ddlContainerType1" runat="server" Style="display: none;" CssClass="NoPrint" required="" />
                    <asp:TextBox ID="txtContainertype" runat="server" SkinID="txt_Hidden_Req_Auto" required="" ClientIDMode="Static"></asp:TextBox>
                    <label><%= GetGlobalResourceObject("Resource", "ContainerType")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3 s3" hidden>
                <div class="flex">
                    <asp:TextBox ID="txtNetWorkPrinter" runat="server" SkinID="txt_Hidden_Req_Auto" required="" ClientIDMode="Static"></asp:TextBox>
                    <label><%= GetGlobalResourceObject("Resource", "Printer")%></label>
                </div>
            </div>
            <div class="col m2 s3">
                <div class="flex">
                    <button type="button" id="btnCreate" class="btn btn-primary" ng-click="getData()">Search</button>
                  <%--  <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkSearch_Click"><%= GetGlobalResourceObject("Resource", "Search")%> <span class="space fa fa-search"></span></asp:LinkButton>--%>
                    <asp:LinkButton ID="lnkGenerateNewContainer" runat="server" CssClass="btn btn-primary" ToolTip="Create 25 New containers" OnClientClick="this.disabled='true';return true;" OnClick="lnkGenerateNewContainer_Click1"> <%= GetGlobalResourceObject("Resource", "New")%><%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="row" id="myForm">
                 <div class="col m3">
                     <div class="flex">
                         <input type="hidden" id="sid" name="sid" value="containerManagement" />
                         <select id="pid" name="pid" class="form-control">
                             <option selected="selected" value="0">Use Default Printer</option>
                             <option value="2">Use an installed Printer</option>
                             <option value="3">Use an IP/Ethernet Printer</option>
                         </select>
                         <label><%= GetGlobalResourceObject("Resource", "Printer")%></label>
                         <span class="errorMsg"></span>
                     </div>
                 </div>
                 <div class="col m3" id="installedPrinter">
                     <div class="flex">
                         <select name="installedPrinterName" id="installedPrinterName" class="form-control"></select>
                         <label for="installedPrinterName">Select an installed Printer:</label>
                         <span class="errorMsg"></span>

                         <div id="loadPrinters" name="loadPrinters" hidden>
                             <a onclick="javascript:jsWebClientPrint.getPrinters();" class="btn btn-success">Load installed printers...</a>
                         </div>
                     </div>
                 </div>
                 <div class="col m6" id="netPrinter">
                     <div class="col m6">
                         <div class="flex">
                             <input type="text" name="netPrinterHost" id="netPrinterHost" class="form-control" required="" />
                             <label for="netPrinterHost">Printer's IP Address:</label>
                             <span class="errorMsg"></span>
                         </div>
                     </div>
                     <div class="col m6">
                         <div class="flex">
                             <input type="text" name="netPrinterPort" id="netPrinterPort" class="form-control" required="" />
                             <label for="netPrinterPort">Printer's Port:</label>
                             <span class="errorMsg"></span>
                         </div>
                     </div>
                 </div>
                 <div class="col m12" hidden>
                     <div class="flex">
                         <textarea id="printerCommands" name="printerCommands" rows="10" cols="80" class="form-control" style="min-width: 100%"></textarea>
                     </div>
                 </div>
                                  
             </div>





        <table style="width:100%" hidden>
            
            <tr>
                <td>
                  <asp:UpdatePanel runat="server">
                      <ContentTemplate>
                          <div class="">
                              

                          </div>
                          </ContentTemplate>
                         <Triggers>
<%--                   <asp:PostBackTrigger ControlID="lnkSearch" />
                   <asp:PostBackTrigger ControlID="lnkGenerateNewContainer" />--%>
                 </Triggers>
                        </asp:UpdatePanel>   
               </td>

            </tr>
            <tr>
                <td colspan="2">
                 
                    <asp:GridView Width="100%" EnableViewState="true" Visible="false" ShowFooter="true" ShowHeaderWhenEmpty="true" CellPadding="5" CellSpacing="5" GridLines="None" CssClass="NoLeftBorder" ID="gvPallet" runat="server" PagerSettings-Position="TopAndBottom" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnPageIndexChanging="gvPallet_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="S.No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="home">
                                <ItemTemplate>

                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center" CssClass="home"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Warehouse" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>

                                    <asp:Literal runat="server" ID="ltWarehouse" Text='<%# DataBinder.Eval(Container.DataItem, "WHCode") %>' />

                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                
                            </asp:TemplateField>
                            
                              <%-- <asp:TemplateField HeaderText="Container Type" HeaderStyle-HorizontalAlign="Center">--%>
                             <asp:TemplateField HeaderText="<%$Resources:Resource,ContainerType%>" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>

                                    <asp:Literal runat="server" ID="ltcontainerType" Text='<%# DataBinder.Eval(Container.DataItem, "ContainerType") %>' />

                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Container" HeaderStyle-HorizontalAlign="Center">--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,Container%>"  HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>

                                    <asp:Literal runat="server" ID="ltcontainerno" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode") %>' />

                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                
                            </asp:TemplateField>                            

                            <asp:TemplateField ItemStyle-CssClass="NoPrint" HeaderStyle-HorizontalAlign="Center" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" 
                                FooterStyle-CssClass="NoPrint" HeaderText="Print Labels" ItemStyle-HorizontalAlign="center">
                                <HeaderTemplate>
                                    <div class="checkbox"> <asp:CheckBox ID="CheckAll" Text="Print Label" onclick="return check_uncheck (this );" runat="server" CssClass="NoPrint" /></div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="checkbox"><asp:CheckBox ID="chkIsPrint" Checked="false" runat="server" CssClass="NoPrint" /><label></label></div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkPrintBarCodeLabel" runat="server" Text="<i class='material-icons'>print</i>" CssClass="ButEmpty" CausesValidation="false" OnClick="lnkPrintBarCodeLabel_Click" />

                                </FooterTemplate>

                                <ControlStyle CssClass="NoPrint"></ControlStyle>

                                <FooterStyle CssClass="NoPrint"></FooterStyle>

                                <HeaderStyle HorizontalAlign="Center" CssClass="NoPrint"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center" CssClass="NoPrint" Width="120px"></ItemStyle>
                            </asp:TemplateField>
                          <%--  <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">--%>
                              <asp:TemplateField HeaderStyle-Width="15%" HeaderText="<%$Resources:Resource,Delete%>" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:ImageButton ID="btnDelete" text="delete" runat="server" Height="15" Width="15" OnClick="btnDelete_Click" />--%>
                                    
                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="<i class='material-icons ss'>delete</i>"   OnClick="lnkDelete_Click"/>
                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                         <EmptyDataTemplate>
                                    <%--    <div align="center" style="font-size:12px">No Data Found</div>--%>
                                 <div align="center" style="font-size:12px"><%= GetGlobalResourceObject("Resource", "NoDataFound")%> </div>
                                    </EmptyDataTemplate>
                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
                        <FooterStyle CssClass="gvSilver_footerGrid" />
                        <RowStyle CssClass="gvSilver_DataCellGrid" />
                        <EditRowStyle CssClass="gvSilver_DataCellGridEdit" />
                        <PagerStyle CssClass="gvBlue_pager" />
                        <HeaderStyle CssClass="gvSilver_headerGrid" />
                        <AlternatingRowStyle CssClass="gvSilver_DataCellGridAlt" />
                    </asp:GridView>
                </td>
            </tr>
        </table>

        <div class="row">
            <div class="col m12">
                <div class="scrollable" style="width: 100%">
                    <table class=" table-striped">
                        <thead>
                            <tr>
                                <th>Warehouse</th>
                                <th>Container Type</th>
                                <th>Container</th>
                                <th center>
                                    <div class="checkbox" style="position: relative; left: 26px;">
                                        <input type="checkbox" id="chkParent" ng-click="selectAllCheckBoxs()" />
                                        <label for="chkParent">Print Label</label>
                                    </div>
                                </th>
                            </tr>

                        </thead>
                        <tbody>
                          

                            <tr dir-paginate="con in ContainerData|itemsPerPage:25">                                
                                <td>{{con.WHCode}}</td>
                                <td>{{con.ContainerType}}</td>
                                <td>
                                   <%-- <img width="70" src="../mInbound/Code39HandlerWithSID.ashx?code={{con.CartonCode}}&SIDETID=0" />
                                    <br />--%>
                                    <b>{{con.CartonCode}}</b>
                                </td>                                
                                <td style="text-align:center;">
                                    <div class="checkbox">
                                        <input type="checkbox" id="{{$index}}" class="checkedone checkselectall" data-obj='{"ContainerCode":"{{con.CartonCode}}"}'/>
                                        <label for="chk{{$index}}"></label>
                                    </div>
                                </td>
                            </tr>

                              <tr>
                            <td ng-show="ContainerData.length==0" colspan="9">
                                <div align="center" style="font-size:13px">No data Found. </div>                
                            </td>
                        </tr>
                        </tbody>
                    </table>
                    <br /><div flex end><dir-pagination-controls direction-links="true" boundary-links="true"></dir-pagination-controls> &emsp; <button type="button" class="btn btn-primary" ng-click="getPrintObjects()"><i class="material-icons">print</i></button></div>
                </div>
            </div>
        </div>

    </div>
   

      <div class="loaderforCurrentStock" style="display:none;">
             <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                 <div style="align-self: center;">
                     <div class="spinner">
                         <div class="bounce1"></div>
                         <div class="bounce2"></div>
                         <div class="bounce3"></div>
                     </div>

                 </div>

             </div>

         </div>

     </div>

 <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mMaterialManagement/ContainerWebClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mMaterialManagement/ContainerWebClientPrintDemo.ashx", "containerManagement")%>
    <%-- <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/MRLWMSC21_SL/mMaterialManagement/ContainerWebClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +  "/MRLWMSC21_SL/mMaterialManagement/ContainerWebClientPrintDemo.ashx", "containerManagement")%>--%>

</asp:Content>
