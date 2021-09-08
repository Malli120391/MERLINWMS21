<%@ Page Title="Receipt Tally Sheet" Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="RTReport.aspx.cs" Inherits="MRLWMSC21.mInbound.RTReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <style>
        .redcolor
        {
            background:#fbe3b9;

        }
        .divAlign {
            display: flex;
            justify-content: space-between;
        }
        .greencolor
        {
            color:green;
        }
        .hideFooter {
            display: none !important;
        }
        .dimensions {
            border: 1px solid black !important;min-width:40px;max-width:50px;padding:5px;text-align: right !important;word-break:break-all !important;
        }
        @media print {
            .hideFooter {
                display:block !important;
            }
            .table-striped th { border : none !important;
            }         

        }
        .SubHeading3 {
            font-size: 14pt !important;         
        }
        .wordbreak {
            word-break:break-all !important;
        }
    </style>

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

    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <script src="RTReport.js"></script>
        <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
      <script type="text/javascript">

          function printDiv(divName) {
              debugger;
              var Gcount = $("#hdnCount").val();
              //var time = 1000;
              //if (Gcount > 100) {
              //    time = 75
              //} else if (Gcount > 500) {
              //    time = 50
              //}
              //else if (Gcount > 20) {
              //    time = 80;
              //}
              var time = 100;
              if (Gcount > 100) { time = 75 } else
                  if (Gcount > 500) { time = 50 }
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            //var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,');
            //var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,resizable=1');
            var printWindow = window.open('', '', 'location=0, status=0, resizable=1, scrollbars=1, width=800, height=400');
            printWindow.document.write('<html><head><title></title>');
            printWindow.document.write('<style type="text/css"> @page { size: landscape;margin-right:15px;margin-left:15px;;margin-top:10px;}  @media print { @page{ size: landscape;}}</style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.write('<LINK href="../PrintStyle.css?v=4.0"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print(); 
                printWindow.close();
 
            }, time*Gcount);


        }
      </script>
    <script type="text/javascript">

        var wcppGetPrintersTimeout_ms = 10000; //10 sec
        var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec
        
        function wcpGetPrintersOnSuccess() {
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
            $.post('RTRClientPrintDemo.ashx',
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
                @page { margin: 0px;size:landscape; }        

    </style>
     <LINK href="../PrintStyle.css?v=4.0"  type="text/css" rel="stylesheet" media="print">
    <div class="container" ng-app="myApp" ng-controller="RTReport">
        <div class="loading" id="divLoading" style="display: none;"></div>
        <div id="printArea" class="PrintListcontainer">
            <asp:Panel runat="server" ID="PrintPanel">

        <div class="row">
            <div class="col m12">
               <%-- <input type="hidden" id="hdnCount" value="0" />--%>
                <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%" id="tdDDRPrintArea">                    
                    <!-- Start Pending Area  -->
                        <tr>
                            <td style="width:38% !important;">
                                <%--<img src="../Images/inventrax.jpg" width="140"/>--%>
                                 <asp:Image runat="server" ID="imgLogo" width="140"/>
                            </td>
                            <td colspan="2">
                                 <asp:Label runat="server" Text="Receiving Tally Report" ID="lblHeader" CssClass="SubHeading3" Visible="false" />
                                <div class="divAlign">
                               <div class="SubHeading3 RTRHeader">RECEIVING TALLY SHEET</div>
                                <div flex end>                                   
                                    <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary NoPrint" PostBackUrl="../mInbound/InboundTracking.aspx"><i class="material-icons">keyboard_backspace</i>Back to List</asp:LinkButton>
                                    <a href="#" style="text-decoration: none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a>
                                </div>
                                </div>
                            </td>
                        </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="height: 0.2px; color: #CCC; border-color: #CCC; background-color: #000;"/>
                        </td>
                    </tr>



                    <!-- End Pending Area  -->


                       <%-- <tr>
                            <td colspan="3">
                               
                                <br />
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="3">
                                <table cellpadding="2" cellspacing="2" border="0" width="100%" class="tableSize">

                                    <tr style="box-shadow: var(--z1)">
                                        <td class="FormLabels" valign="middle" width="50%" style="vertical-align:top;padding-top:5px;">
                                            <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Store&nbsp;Ref.# :                                    
                                    <asp:Literal runat="server" ID="ltbarStoreRefNo" /></b>
</td>
                                        <td valign="top" class="FormLabels" colspan="2" align="right" style="padding-right:0%;">
                                            <table cellpadding="4" cellspacing="1" border="0" width="100%" align="right" class="indp__">
                                                <tr>

                                                    <td style="width: 50px;vertical-align:top !important;">
                                                        <strong>Tenant&nbsp;</strong>
                                                    </td>
                                                    <td style="width: 200px;vertical-align:top !important;">
                                                        <div>
                                                            <b>:</b>&nbsp;<asp:Literal runat="server" ID="ltTenant" /></div>
                                                    </td>
                                                   
                                                    
                                                    <td style="vertical-align:top !important;">
                                                        <strong>Supplier&nbsp;</strong>
                                                    </td>
                                                    <td style="vertical-align:top !important;">
                                                        <div>
                                                            <b>:</b>&nbsp;<asp:Literal runat="server" ID="ltSupplier" /></div>
                                                    </td>

                                                </tr>
                                                <tr>

                                                     <td style="width: 60px;vertical-align:top !important;">
                                                        <strong>Store&nbsp;</strong>
                                                    </td>
                                                    <td style="width: 120px;vertical-align:top !important;">
                                                        <div>
                                                            <b>:</b>&nbsp;<asp:Literal runat="server" ID="ltbarWarehouse" /></div>
                                                    </td>

                                                    
                                                    <td style="vertical-align:top !important;">
                                                        <%--<strong>AWB/BL# <span class="pull-right">:</span></strong>--%>
                                                        <strong>Dock&nbsp;</strong>
                                                    </td>
                                                    <td style="vertical-align:top !important;">
                                                        <div>
                                                           <%-- <asp:Literal runat="server" ID="ltAWBBLNo" />--%>
                                                             <b>:</b>&nbsp;<asp:Literal runat="server" ID="ltDockLocation" />

                                                        </div>
                                                    </td>                                                    
                                                </tr>
                                                <tr>

                                                    <td style="vertical-align:top !important;">
                                                        <strong>Doc.&nbsp;Date&nbsp;</strong>
                                                    </td>
                                                    <td style="vertical-align:top !important;">
                                                        <div>
                                                            <b>:</b>&nbsp;<asp:Literal runat="server" ID="ltDocDate" /></div>
                                                    </td>


                                                    
<%--                                                    <td>
                                                        <strong>No. of Pkgs <span class="pull-right">:</span></strong>
                                                    </td>--%>
                                                    <td colspan="2">
                                                        <div style="position: relative;right: 20px;">
                                                           <%-- <asp:Literal runat="server" ID="ltNoofPackages" />--%>
                                                            <asp:Literal runat="server" ID="ltDockBarcode" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                        </tr>



                </table>
            </div>
        </div><gap></gap>


        <div class="row NoPrint" id="myForm">
                 <div class="col m3">
                     <div class="flex">
                         <input type="hidden" id="sid" name="sid" value="receivingTallyReport" />
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
                             <input type="text" name="netPrinterPort" id="netPrinterPort" class="form-control" value="9100" required="" />
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
                 <div class="col m3 s3">
                     <div class="flex">
                         <select ng-model="ddlPrintLabel" id="selPrintLabel" class="" ng-options="label.BarCodeId as label.BarCode for label in labels" required="">
                             <option value=""></option>
                         </select>
                         <label><%= GetGlobalResourceObject("Resource", "Label")%> </label>
                         <span class="errorMsg"></span>
                     </div>
                 </div>                 
             </div>

        <div class="row NoPrint">
            <div class=" col m3">
                    <div class="flex">
                        <input type="text" required="" id="txtMcode" />
                        <label>Search Part# ...</label>
                    </div>
            </div>
            <div class="col m3">
                <div class="flex col m1 NoPrint">
                   <button type="button" class="btn btn-primary" ng-click="GetRTR();">Search</button>
                    <input type="hidden" value="{{RTRData.length}}" id="hdnCount" />
                </div>
            </div>
        </div>
                            <style>
                .fixed-scroll  {
                    width: 85vw;
                    overflow:auto;
                }

                @media print{
                      .fixed-scroll  {
                    width:auto;
                    overflow:unset;
                }
                }
            </style>
                <div class="fixed-scroll">
        <div class="row">
            <div class="col m12">
                <div class="scrollable" style="width: 100%">
                    <table class="table-striped PrintRTRTable" style="width:100% !important">
                        <thead>
                            <tr>
                                <th>Line#</th>
                                <th style="width:130px !important;text-align:left !important;padding-left:4px;">Item&nbsp;Code</th>
                                <th hidden>Kit ID</th>
                                <th style="width:100px !important;text-align:left !important;padding-left:4px;">PO/Invoice</th> 
                                <th style="width:100px !important;text-align:left !important;padding-left:4px;">GRN #</th> 
                                <th style="text-align:left;">Batch#</th>
                                <th style="text-align:left;">Serial#</th>
                                <th center>Mfg.&nbsp;Date</th>
                                <th center>Exp.&nbsp;Date</th>
                                <th>Project&nbsp;Ref.#</th>
                                <th>MRP</th>
                                <th>HU Size</th>
                                <th style="text-align:right !important;">Length</th>
                                <th style="text-align:right !important;">Width</th>
                                <th style="text-align:right !important;">Height</th>
                                <th style="text-align:right !important;">Volume</th>
                                <th style="text-align:left !important;">BUoM/Qty.</th>
                                <th style="text-align:left !important;">IUoM/Qty.</th>
                                <%--<th hidden>IUoM/MoP.</th>--%>
                                <th number style="text-align:right !important;">Rcvd. Qty./Inv. Qty.</th>
                                
                                <th class="NoPrint">Receive</th>
                                <th number class="NoPrint">Print Qty.</th>
                                <th center class="NoPrint" style="text-align:center !important;">Print Label<gap></gap>
                                     <%--<button type="button" id="btnSelectAll" class="btn btn-primary" ng-click="selectAllCheckBoxs()">Select All</button>--%>
                                    <div class="checkbox">
                                        <input type="checkbox" id="chkParent" ng-click="selectAllCheckBoxs()"/>
                                        <label for="chkParent"></label>
                                    </div>
                                </th>
                            </tr>

                        </thead>
                        <tbody>
                           <tr dir-paginate="rtr in RTRData|itemsPerPage:250000" ng-class="{'redcolor':rtr.IsLBHAvailable == 0}">
                                
                                <td style="border-bottom:1px solid #e0ddd8 !important;">{{("000"+rtr.LineNumber).slice(-3)}}</td>
                                <%--<td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.MCode}}&nbsp;<span style="font-size:9px">[{{rtr.OEMPartNo}}]</span> <br /><span style="color:#2196F3;font-size:8Pt">{{rtr.MDescription}}</span><br />&emsp;<img style="position: relative;left: -21px;" width="270" src="Code39HandlerWithSID.ashx?code={{rtr.MCode}}&SIDETID={{rtr.SupplierInvoiceDetailsID}}" /></td>--%>
                               <td style="border-bottom:1px solid #e0ddd8 !important;width:110px !important;font-weight:500;">{{rtr.MCode}}&nbsp;<span style="font-size:9px">[{{rtr.OEMPartNo}}]</span> <br /><span class="Mdesc" style="color:#2196F3;font-size:8Pt;" >{{rtr.MDescription}}</span><br />&emsp;<img style="position: relative;left: 0px;" width="70" src="Code39HandlerWithSID.ashx?code={{rtr.MCode}}&SIDETID={{rtr.SupplierInvoiceDetailsID}}" /></td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;" hidden>{{rtr.KitCode}}</td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;width:100px !important;">
                                    <div class="wordbreak">
                                    {{rtr.PONumber}}/<br />{{rtr.InvoiceNumber}}
                                        </div>
                                </td> 
                               <td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.GRNNumber}}</td>
                                <%--<td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.BatchNo}}<span ng-if="rtr.SerialNo!=null">/{{rtr.SerialNo}}</span></td>--%>
                                <td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.BatchNo}}</td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.SerialNo}}</td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;text-align:center !important;">{{rtr.MfgDate}}</td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;text-align:center !important;">{{rtr.ExpDate}}</td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.ProjectRefNo}}</td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.MRP}}</td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;"><div class="dimensions">{{rtr.HUSize}}</div></td>

                                <td number style="border-bottom:1px solid #e0ddd8 !important;"><div class="dimensions">{{rtr.MLength | number : 2}}</div></td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;"><div class="dimensions">{{rtr.MWidth | number : 2}}</div></td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;"><div class="dimensions">{{rtr.MHeight | number : 2}}</div></td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;"><div class="dimensions">{{rtr.Volume}}</div></td>

                         
                                <td style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.BUoM}}/{{rtr.BUoMQty | number : 2}}</td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;">{{rtr.InvUoM}}/{{rtr.InvUoMQty | number : 2}}</td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;text-align:right !important;">[{{rtr.ReceivedQty | number : 2}}/{{rtr.InvoiceQuantity | number : 2}}]</td>
                                <td style="border-bottom:1px solid #e0ddd8 !important;" class="NoPrint">

                                    <%--ng-if="rtr.IsLBHAvailable ==1"--%>
                                    <span>
                                         <a style="color:blue;font-weight:bold;" target="_blank"  href="../mInventory/GoodsIn.aspx?ibdno={{rtr.InboundID}}&mmid={{rtr.MaterialMasterID}}&lno={{rtr.LineNumber}}&PODH={{rtr.POHeaderID}}&SIID={{rtr.SupplierInvoiceID}}&SIDETID={{rtr.SupplierInvoiceDetailsID}}">Receive</a>
                                    </span>
                                   

                                </td>
                                <td number style="border-bottom:1px solid #e0ddd8 !important;" class="NoPrint"> <input id="txtInvoiceValue" ng-model="invQty" ng-init="invQty=rtr.InvoiceQuantity" style="width:55px;text-align:right;" type="text" value="{{rtr.InvoiceQuantity}}" /> </td>  
                                <td style="text-align:center;border-bottom:1px solid #e0ddd8 !important;" class="NoPrint">
                                    <div class="checkbox">
                                        <input   type="checkbox" id="{{$index}}" class="checkedone checkselectall" data-obj='{"MfgDate":"{{rtr.MfgDate}}","ExpDate":"{{rtr.ExpDate}}","SerialNo":"{{rtr.SerialNo}}","BatchNo":"{{rtr.BatchNo}}","ProjectRefNo":"{{rtr.ProjectRefNo}}","MCode":"{{rtr.MCode}}","Description":"{{rtr.MDescription}}","MDescription":"{{rtr.MDescription}}","MDescriptionLong":"{{rtr.MDescriptionLong}}","LineNo":"{{("000"+rtr.LineNumber).slice(-3)}}","MRP":"{{rtr.MRP}}","KitCode":"{{rtr.KitCode}}","PrintQty":"{{invQty}}","HUSize":"{{rtr.HUSize}}"}'/>
                                        <label for="chk{{$index}}"></label>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <div class="NoPrint">
                     <div flex end><dir-pagination-controls direction-links="true" boundary-links="true"></dir-pagination-controls></div>
                    </div>
                </div>
            </div>
        </div>    
                    </div>
        <div class="row NoPrint">
            <div class="col m12" flex end>
                <button type="button" class="btn btn-primary" ng-click="getPrintObjects()"><i class="material-icons">print</i></button>
            </div>
        </div>
                 <div class="row hideFooter" style="font-weight:bold;">
                    <div class="col m12">
                        <table cellspacing="5" class="tableSize">
                            <tr><td>Warehouse Man Name : </td></tr>
                            <tr><td>Signature : </td></tr>
                            <tr><td>Date : </td></tr>
                        </table>                        
                    </div>
                </div>
                <div style="display:table-footer-group;position: fixed;bottom: -5px;clear:both;right:0;left:0px; page-break-inside:avoid;  page-break-before:always;">
                    <table width="100%" class="tableSize">
                            <tr>
                                <td colspan="12">
                                    <div style="display: flex; justify-content: space-between; align-items:center; padding:0px 10px">  
                                        <span><b>Store Ref.#:&nbsp;</b><asp:Literal ID="ltbarStoreRefNo1" runat="server"></asp:Literal></span>
                                        <span><small>Printed On : {{currentDate | date:'dd-MMM-yyyy hh:mm a'}}</small> </span>
                                        <span>www.merlinwms.com</span>
                                   </div> 
                                </td>
                              
                            </tr>
                        </table>
                </div>
                </asp:Panel></div>
    </div>
     <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mInbound/RTRClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mInbound/RTRClientPrintDemo.ashx", "receivingTallyReport")%>
    <%-- <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/TransCrate_SL_2020/mInbound/RTRClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +  "/TransCrate_SL_2020/mInbound/RTRClientPrintDemo.ashx", "receivingTallyReport")%>--%>
     
</asp:Content>
