<%@ Page Title="Pick List" Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="DeliveryPickNote.aspx.cs" Inherits="MRLWMSC21.mOutbound.DeliveryPickNote"  MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <asp:ScriptManager runat="server" ID="smngrDPN" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
    
    <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>


    <script type="text/javascript" src="Scripts/jquery-ui-1.8.24.js"></script>
    <script type="text/javascript" src="Scripts/modernizr-2.6.2.js"></script>
        <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <script type="text/javascript">

        function printDiv(divName) {
           debugger
            var Gcount = '<%=this.gvPOLineQty.Rows.Count%>';
            var time = 1000;
            if (Gcount > 100) {
                time = 75
            } else if (Gcount > 500) {
                time = 50
            }
            else if (Gcount > 20) {
                time = 80;
            }
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            //var panel = document.getElementById("PrintPanel");
            var printWindow = window.open('', '', 'location=0, status=0, resizable=1, scrollbars=1, width=800, height=400');
            //printWindow.document.write('<html><head><title>Delivery Pick Note</title>');
            printWindow.document.write('<LINK href="../PrintStyle.css?v=4.0"  type="text/css" rel="stylesheet" media="print">');
            //printWindow.document.write('<style type="text/css"> @page { size: portrait !important; margin-top:80px; margin-bottom:80px;}   @media print{@page {size: portrait}    #tdDDRPrintArea{ position:relative;width:100%;}   div.divFooter {display:block;position: relative;margin-left:50px;} div.divFooterLogo {display:block;position: fixed;bottom: 0px;right:0;}  .tdPickLink { display:none; }  .tblGridInnerTable{font-size:10pt;} .spanFooterNote {display:none;} .NoPrint{display:none;} .page-break{page-break-after:always;}      )</style>'); //This line is commented by kashyap on 17/08/2017 for delivery note print error 
            //printWindow.document.write('<style type="text/css"> @page { size: portrait; margin-top:20px; margin-bottom:20px;}   @media print{@page {size: portrait}.gvSilver{display:table-row-group; position: relative; height: 9.5in;  margin-left: .5in;  margin-right: .5in; margin-top: 0;  margin-bottom: -.25;  z-index: 10;} #tdDDRPrintArea{ display:table-header-group;table-layout:fixed; top:150px;  width: 100%;height:50px;}  div.divFooter {display:table-footer-group;position: relative;margin-left:50px;} div.divFooterLogo {display:block;position: fixed;bottom: 0px;right:0;}  .tdPickLink { display:none; }  .tblGridInnerTable{font-size:10pt;} .spanFooterNote {display:none;} .NoPrint{display:none;} .page-break{page-break-before:always;}      )</style>');//This line is commented by kashyap on 18/08/2017 for delivery note print error 
            printWindow.document.write('<style type="text/css"> @page { size: landscape; margin-top:10px; margin-left:15px;margin-right:15px;}   @media print { .gvSilver{ position: relative; height: 9.5in;margin-top: 0;  margin-bottom: -.25;  z-index: 10;} .gvSilver_DataCellGrid{page-break-inside : avoid;} #tdDDRPrintArea{ display:table-header-group; top:100px;  width: 100%;}  div.divFooter {display:block;position:relative;clear:both; margin-left:25px; } div.divFooterLogo {display:table-footer-group;position: fixed;bottom: 0px;clear:both;right:0;  page-break-inside:avoid;  page-break-before:always; }   .tdPickLink { display:none; } .spanFooterNote {display:none;} } </style>');
            printWindow.document.write('<script src="../Scripts/jquery-1.8.2.min.js"><\/script><script>   $(document).ready(function(){  $(".tblGridInnerTable").attr("cellPadding","5");  });<\/script>');
            //printWindow.document.write('<style type="text/css">   @media print { .page-break  { display: block; page-break-before: always; } }   </style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            setTimeout(function () {
                printWindow.print(); 
                printWindow.close();
 
            }, time*Gcount);
        }      
        


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadMCode();
            }
        }

        
       
        
        function fnLoadMCode() {
            $(document).ready(function () {
                $('#spanClose').click(function (event) {
                    $('#divContainer').hide();
                });

                $('#btnClose').click(function (event) {
                    $('#divContainer').hide();
                });

                $("#<%= this.txtMCode.ClientID %>").focusin(function () {
                    var TextBox = $("#<%=this.txtMCode.ClientID%>");
                    if (TextBox.val() == "Search Part # ...") 
                        $("#<%=this.txtMCode.ClientID%>").val('');      
                });

                try {
                    var textfieldname = $("#<%= this.txtMCode.ClientID %>");
                    DropdownFunction(textfieldname);
                    $("#<%= this.txtMCode.ClientID %>").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDPNoteMCodeOEMData") %>',
                                data: "{ 'prefix': '" + request.term + "', 'OutboundID': '" + '<%= ViewState["OutboundID"] %>' + "' }",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split('`')[0],
                                            description: item.split('`')[1] == undefined ? "" : " <font color='#086A87'>" + item.split('`')[1] + "</font>"
                                        }
                                    }))
                                },
                                error: function (response) {

                                },
                                failure: function (response) {

                                }
                            });
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
                } catch (ex) { }

            });
        }

        fnLoadMCode();
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            CalculateTotals();
        });

        function CalculateTotals() {
            var TotalVol = 0;
            $('.lblVolume').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalVol = eval(TotalVol) + eval(thisQty);
            });
            $('.lblTotalVolume').text(TotalVol.toFixed(2));


            var TotalQty = 0;
            $('.lblQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalQty = eval(TotalQty) + eval(thisQty);
            });
            $('.lblQtyTotal').text(TotalQty.toFixed(2));

            var TotalAQty = 0;
            $('.lblAQty').each(function () {
                var thisQty = $(this).text().trim() == "" ? "0" : $(this).text().trim();
                TotalAQty = eval(TotalAQty) + eval(thisQty);
            });
            $('.lblAQtyTotal').text(TotalAQty.toFixed(2));
        }
        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Part # ...";
            //TextBox.style.color = "#A4A4A4";
        }
        var containerautocomid = 0;
        var MMId = 0;
        var OBDNo = 0;
        var LineNo = 0;
        var SOH = 0;
        var LOc = 0;
        var Mcode = 0;
        var Mfgdate = 0;
        var Expdate= 0;
        var BatchNo = 0;
        var SerailNo = 0;
        var Projrefno = 0;
         var MRP = 0;
        var CartonCode = 0;
        var ToCartonCode = 0;
        var SoDetailsid = 0;
        var PickedQuantity = 0;
        var AssignQty = 0;
        var AssignId = 0;
        var HUNo = 0;
        var HUSize = 0;


      

       

        function getcontainer(loc) {
            var textfieldname = $("#txtcontainer");
            DropdownFunction(textfieldname);
            $("#txtcontainer").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/GetContainersForGoodsOutNOBD',
                        data: "{'Prefix': '" + request.term + "','Outbound':'" + '<%= ViewState["OutboundID"] %>' + "', 'Location':'" + LOc + "' }",
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
                containerautocomid = i.item.val;

                debugger;
            },
            minLength: 0
        });
        }

        function OpenPickDislog(mrp,AssignedQuantity,pickedqty,AssignID,sodetailsid,mid, obdno, lineno, soh, loc, mcode, mfgdate, expdate, bno, sno, projno, cartoncode,HuNo,HuSize)
        {
            debugger;
            //alert(mcode);
            $("#txtPickQty").val('');
            MMId = mid;
            MRP = mrp;
            OBDNo = obdno;
            LineNo = lineno;
            SOH = soh;
            LOc = loc;
            Mcode = mcode;
            Mfgdate = mfgdate;
            Expdate = expdate;
            BatchNo = bno;
            SerailNo = sno;
            Projrefno = projno;
            CartonCode = cartoncode;
            getcontainer("");
            SoDetailsid = sodetailsid;
            AssignId = AssignID;
            PickedQuantity = pickedqty;
            AssignQty = AssignedQuantity;
            HUNo = HuNo;
            HUSize = HuSize;

            $("#divContainer").show();
            GetPickedList(MRP,MMId, OBDNo, LOc, CartonCode, Mfgdate, Expdate, BatchNo, SerailNo, Projrefno);

            //$("#hdnmmid").val(mid);
            //$("#hdnOBDNo").val(obdno);
            //$("#hdnLineNo").val(lineno);
            //$("#hdnsoh").val(soh);
            //$("#hdnloc").val(loc);
            //$("#hdnmcode").val(mcode);
            
        }


        function GetPickedList(MRP,MMId, OBDNo, LOc, CartonCode, Mfgdate, Expdate, BatchNo, SerailNo, Projrefno) {

            debugger;
            $.ajax({
                type: "POST",
                url: 'DeliveryPickNote.aspx/GetPickedItems',
                contentType: "application/json; charset=utf-8",
                data: "{ 'MMId': '" + MMId + "','LOc':'" + LOc + "', 'CartonCode':'" + CartonCode + "','OBDNo':'" + OBDNo + "', 'BatchNo':'" + BatchNo + "', 'SerailNo':'" + SerailNo + "','Projrefno':'" + Projrefno + "', 'Expdate':'" + Expdate + "', 'Mfgdate':'" + Mfgdate+"' }",
                dataType: "text",
                success: function (data) {
                    debugger;
                    var obj = $.parseJSON(data);
                    var Data = obj.d;
                    console.log(Data);
                  
                    pkginfo = Data;
                    $("#tblPalletDetails").empty();
                    var row = 0;
                    // $("#tblReceivedDetails").append("<table class='mytable' style='width:855px !important;'><tr style='height:30px;background: #d2def2 !important;color:#FFFFFF;font-weight:bold;text-align:center;color:#333333;'><td style='width:250px;'>SKU</td><td style='width:250px;'>Desc.</td><td> HU #</td><td> HU Size </td><td>Status</td><td>Location</td><td> Qty.</td></tr>");
                    $("#tblPalletDetails").append("<thead><tr><th> SL No.</th><th>Part No.</th> <th>Location</th> <th>From Container</th> <th>Picked Qty.</th><th>Delete</th></tr></thead><tbody>");

                    for (var i = 0; i < Data.length; i++) {
                        row = row + 1;
                      
                        // $("#tblReceivedDetails").append("<tr style='text-align:center;'><td>" + pkginfo[i].SKU + "</td><td style='width:150px !important;text-align:left;'>" + pkginfo[i].MDescription + "</td><td>" + pkginfo[i].HUNO + "</td><td>" + pkginfo[i].HUSize + "</td><td style='color:red'>Stock / QC pending / Damaged</td><td>" + pkginfo[i].Location + "</td><td>" + pkginfo[i].Quantity + "</td></tr>");
                        //$("#tblReceivedDetails").append("<tr style='text-align:center;'><td  class='aligntext'>" + pkginfo[i].SKU + "</td><td class='aligntext'>" + pkginfo[i].MDescription + "</td><td class='alignnumbers'>" + pkginfo[i].HUNO + "</td><td class='alignnumbers'>" + pkginfo[i].HUSize + "</td><td class='aligntext'>" + pkginfo[i].StockStatus + "</td><td class='aligntext'>" + pkginfo[i].Location + "</td><td class='alignnumbers'>" + pkginfo[i].Quantity + "</td></tr>");
                        $("#tblPalletDetails").append("<tr></td><td>" + row + "</td><td>" + pkginfo[i].MCode + "</td><td>" + pkginfo[i].Location + "</td><td>" + pkginfo[i].CartonCode + "</td><td style='padding-left:10px !important;'>" + pkginfo[i].PickedQty + "</td><td><a style='text-decoration:none;cursor: not-allowed !important;pointer-events: none;' onclick='return Delete1(" + pkginfo[i].AssignID + ", " + pkginfo[i].StorageLocationID+")'><i class='fa fa-trash-o' aria-hidden='true'></i></a></td></tr>");
                    }

                    $("#tblPalletDetails").append("</tbody>");

                    //console.log(FormData);
                },
                error: function (result) {
                    alert("Error");
                }
            });
            console.log(MRP);
        }


        function Delete1(PickedId, statusID) {
            debugger;

           
            if (statusID == "2") {
                var txt;
                if (confirm("Are you sure you want to delete?") == true) {
                
                $.ajax({
                    type: "POST",
                    url: 'GroupDeliveryPickNote.aspx/DeletePickedItems',
                    contentType: "application/json; charset=utf-8",
                    data: "{ 'PickedId': '" + PickedId + "'}",
                    dataType: "text",
                    success: function (data) {
                        var obj = $.parseJSON(data);
                        debugger;
                        if (obj.d == 1) {
                            GetPickedList(MRP,MMId, OBDNo, LOc, CartonCode, Mfgdate, Expdate, BatchNo, SerailNo, Projrefno);
                            showStickyToast(true, " Line item successfully deleted", true);
                            location.reload();
                            return false;
                        }
                        else {
                            showStickyToast(false, " Error While Delete", false);
                        }

                    },
                    error: function (result) {
                        showStickyToast(false, " Error", false);
                    }
                    });
              }
            }
            else {
                showStickyToast(false, "Unable to delete this line item", false);
            }
            
        }

       

        var pickqty = 0;
        function PickItem()
        {            
            console.log(MRP);
            debugger;
            pickqty = $("#txtPickQty").val();
            if (pickqty == "" || pickqty == undefined) {
                showStickyToast(false, 'Please Enter Qty.');
                return false;
            }
            if (pickqty == 0 || pickqty == "0")
            {
                showStickyToast(false, 'Please Enter valid Qty.');
                return false;
                
            }
            var alreadypicked = PickedQuantity;
            var assignedqty = AssignQty;
            if (assignedqty < (alreadypicked + parseInt(pickqty)))
            {
                showStickyToast(false, 'Quantity Exceeded');
                return false;
            }
            //if (PickedQuantity  parseInt)
           
            var ToCarton = $("#txtcontainer").val();
            ToCarton = "";
            if (ToCarton != "") {
                if (containerautocomid == 0 || containerautocomid == null || containerautocomid == "") {
                    showStickyToast(false, 'Intermediate container is incorrect');
                    return false;
                }
            }

 
            

            var obj = {};
            obj.MMId = MMId;
            obj.OBDNo = OBDNo;
            obj.LineNo = LineNo;
            obj.SOH = SOH;
            obj.LOc = LOc;
            obj.Mcode = Mcode;
            obj.Mfgdate = Mfgdate;
            obj.Expdate = Expdate;
            obj.BatchNo = BatchNo;
            obj.SerailNo = SerailNo;
            obj.Projrefno = Projrefno;
            obj.CartonCode = CartonCode;
            obj.Qty = pickqty;
            obj.ToCartonCode = ToCarton;
            obj.sodetailsid = SoDetailsid;
            obj.AssignId = AssignId;
            obj.MRP = MRP;
            obj.HUNo = HUNo;
            obj.HUSize = HUSize;
            $("#divLoading").show();
            $.ajax({
                url: "DeliveryPickNote.aspx/InsertPickItem",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: JSON.stringify(obj),
                success: function (response)
                {
                    debugger;
                     //----------------------- added by durga on 02/03/2018 ----------------------//
                    $("#divLoading").hide();
                    if (response.d == -768) {
                        showStickyToast(false, 'Quantity Exceeded');

                        return false;
                    }
                    if (response.d == 2) {
                        showStickyToast(false, 'Stock not Available');

                        return false;
                    }
                    if (response.d == -333) {
                        showStickyToast(false, 'Please pick all Items with Carton, earlier picked with carton');

                        return false;
                    }
                    if (response.d == -444) {
                        showStickyToast(false, 'Please pick all Items without Carton, since first item is picked without carton');

                        return false;
                    }
                    if (response.d == -999) {
                        showStickyToast(false, 'Quantity Exceeded ');
                        
                        return false;
                    }
                    else if (response.d > 0) {
                        showStickyToast(true, 'Picked Successfully');
                        location.reload();
                        return false;
                    }
                    else {
                        showStickyToast(false, 'Error While Picking ');
                        return false;
                    }
                    //----------------------- commented by durga on 02/03/2018 ----------------------//
                    //if (response.d == "success") {
                    //    showStickyToast(true, 'Picked Successfully');
                    //}
                    //else
                    //{
                    //    showStickyToast(false, 'Error While Picking ');
                    //}
                }


            });
            //alert(MMId);
        }
       /* function myFunction() {
            window.print();
        }*/
    </script>


      <style>
        /* Absolute Center Spinner */
        .loading {
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
            .loading:before {
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
            .loading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after {
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

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>

     <style type="text/css">
                     .divAlign {
                display:flex;justify-content:space-between;
            }
             body    {
            overflow:scroll;

        }

               .BarCodeCell {

        }
               .obd{
           position: relative;
    top: -9px;
    left: 0px;
        }
            .flex input[type="text"], input[type="number"], textarea {
                margin-bottom:0px;
            }

            .scrollable div {
              width: 83vw;
                overflow:auto;
            }

            .frezScroll {
                width: 89vw !important;
                overflow: auto;
            }
                .scrollable div table {
                    white-space:nowrap;
                }
                .SubHeading3
{
	color: #333333;
	font-size:14pt;
	font-weight:bold;
}

            .FormLabels {
                font-size: 10pt;
            }
         .alignRight {
             text-align:right !important;
         }
         .alignCenter {
             text-align:center !important;
         }
            /*#tdDDRPrintArea table {
                width:320px !important;
                
            }*/

            .inscrolling div{
                overflow:auto
            }
              #gvPOLineQty {
                    width:100% !important;
                }

            @media print{
             .barcodeSize img {
                 width:50px !important;
             }
             .alignRight {
             text-align:right !important;
         }
         .alignCenter {
             text-align:center !important;
         }
                          /*.hideTitle {
                 display:none !important;
                 visibility:hidden !important;
             }*/
                menuitem, .Header, .ModuleHeader, .print, back-to-list, footer-bar,.ftoor,.hideContent{
                    display:none !important;
                }
                #Header, #Footer { display: none !important; }

                .container{
                    width:100vw !important;
                    border:0;
                    box-shadow:none;
                    padding:0px;
                    margin:0px;
                }

                .accordian ~ .content{
                    width:100% !important;
                    margin-left:0px !important;
                }

                .inscrolling div{
                    overflow:unset;
                }

                tr{
                    box-shadow:unset !important;
                }

                .styleprint{
                    display:block !important;
                    text-align:center;
                    position:relative;
                   
                }
                table {
                    width:100% !important;
                }

                .Header,.flex__between {
            display:none !important;
            visibility:hidden  !important;
        }
               
            }
            @page { size: landscape;}


   </style>

    <div class="wrapper1">
    <div class="divUP">
    </div>
</div>

    <div class="wrapper2">
    
        
        
            <link href="../PrintStyle.css?v=4.0" rel="stylesheet" media="print" />
        <div class="dashed"></div>
        <div class="container">

           <div class="loading" id="divLoading" style="display: none;"></div>

             <div id="printArea" class="PrintListcontainer">
                  <asp:Panel runat="server" ID="PrintPanel">

            <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center" id="tdDDRPrintArea">
<%--                <tr>
                    <td colspan="5" align="right" valign="bottom" class="NoPrint">
                        <div style="float: right;">

                           
                           
                        </div>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="3">
                        <table width="100%">
                            <tr>
                                <td style="width:43%;">
                                    <div>
                                        <%--<img src="../Images/inventrax.jpg" width="140"/>--%>
                                        <asp:Image runat="server" ID="imgLogo" width="140"/>
                                    </div>
                                </td>
                                <td>
                                    <div class="divAlign">
                                        <div class="SubHeading3 RTRHeader">
                                            PICK LIST
                                        </div>
                                        <div class="NoPrint">
                                            <asp:LinkButton runat="server" ID="lnkbackToList" CssClass="btn btn-primary" PostBackUrl="OutboundTracking.aspx">Back to List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                                            <button onclick="javascript:printDiv('tdDDRPrintArea');" type="button" class="btn btn-primary print">Print <i class="material-icons">print</i></button></flex>&nbsp;
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                   <%-- <td colspan="2" style="text-align: left;" class="SubHeading3">
                        <br />
                        &emsp;&emsp;&emsp;Pick List
                          
                    </td>--%>
                </tr>

                <tr>
                    <td colspan="3">
                        <hr style="height: 0.2px; color: #CCC; border-color: #CCC; background-color: #000;"/>
                    </td>
                </tr>   
                <tr style="box-shadow:var(--z1)" class="DPNoteFSize">
                    <td style="vertical-align:top !important;padding-top:3px !important;width:50%;"><asp:Literal ID="ltDelvDocNo" runat="server" /> <br /></td>
                    <td style="width:25%;"></td>
                    <td style="width:100px !important;vertical-align:top !important;padding-top:6px !important;display:none;">
                        <asp:Literal  ID="ltstore" runat="server" />
                    </td>
                    <td colspan="2" align="right"> <asp:Literal  ID="ltDelvDocDetails" runat="server" />
                      
                    </td>
                </tr>
          



                <tr>  
                    <td colspan="3" align="right">   <br /><asp:Label ID="lblStatusMessage" runat="server" CssClass="ErrorMsg" /></td>
                </tr>
                <tr>
                    <td class="NoPrint" colspan="12">
                        <div class="row">
                            <div class="col m3"><asp:Literal runat="server" ID="ltPNCPRecordCount" /></div>
                            <div class="col m2 offset-m6"><div class="flex"><asp:TextBox ID="txtMCode" Text="Search Part # ..." runat="server" SkinID="txt_Hidden_Req_Auto" onblur="javascript:focuslost(this)"></asp:TextBox></div></div>
                            <div class="col m1 p0"><gap5></gap5><asp:LinkButton ID="lnkMCodeSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkMCodeSearch_Click">Search<i class="material-icons">search</i></asp:LinkButton></div>
                        </div>
                    </td>
                </tr>
   
   
      <tr>
         <td valign="top" align="center" colspan="3"> 
            <style>
                .fixed-scroll  {
                    width: 83.5vw;
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
                
             <asp:GridView  CssClass="gvSilver tableSize" CellPadding="1" CellSpacing="1" border="0" ShowFooter="true" GridLines="Both" BackColor="#ffffff" ID="gvPOLineQty" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" HorizontalAlign="Left" OnRowCreated="gvPOLineQty_RowCreated" OnRowCommand="gvPOLineQty_RowCommand" OnSorting="gvPOLineQty_Sorting" OnPageIndexChanging="gvPOLineQty_PageIndexChanging" OnRowDataBound="gvPOLineQty_RowDataBound">

                 <Columns>
                     <asp:TemplateField HeaderText="Line#" ItemStyle-CssClass="LineNoCell" HeaderStyle-HorizontalAlign="Left">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber","{0:000}") %>' />

                             <%-- <asp:Literal runat="server" ID="hidKitPlannerID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>'/>--%>
                         </ItemTemplate>
                     </asp:TemplateField>

                     <asp:BoundField DataField="KitPlannerID" HeaderText="KitPlannerID" SortExpression="KitPlannerID" Visible="false" />
                     <asp:BoundField DataField="ParentMcode" HeaderText="ParentMcode" SortExpression="ParentMcode" Visible="false" />

                     <asp:TemplateField ItemStyle-Width="140px" HeaderText="Item Code" ItemStyle-CssClass="BarCodeCell">
                         <ItemTemplate>
                             <asp:Label runat="server" CssClass="BarCodetext" ID="lblMCode" Visible="false" Text='<%# String.Format("{0} / {1} ",DataBinder.Eval(Container.DataItem,"TenantName"),DataBinder.Eval(Container.DataItem, "MCode")) %>' />

                             <%--<asp:Literal runat="server" ID="ltOEMPartNo" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' /><br />--%>
                             <asp:Literal runat="server" ID="ltOEMPartNo" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' /><br />
                             <div style="color: #2196F3;font-size: 8Pt;" class="DPNColor">
                             <asp:Literal runat="server" ID="ltItemDesc" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription") %>'/></div> <br />

                             <%--<asp:Literal runat="server" ID="ltBarCodeMCode" Text='<%# String.Format("<img width=\"300px\" src=\"../mInbound/Code39Handler.ashx?code={0} / {1}\"",DataBinder.Eval(Container.DataItem, "MCode")) %>'/><br /><br />--%>
                             <%--<asp:Literal runat="server" ID="ltBarCodeMCode" Text='<%# String.Format("<img width=\"300px\" src=\"../mInbound/Code39Handler.ashx?code={0}/{1}\"",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem, "MCode")) %>'/><br /><br />--%>
                           <div class="barcodeSize">
                                <%--<asp:Literal runat="server" ID="Literal1" Text='<%# String.Format("<img width=\"70px\" src=\"../mInbound/Code39Handler.ashx?code={0}\"",DataBinder.Eval(Container.DataItem, "MCode") + "|" + DataBinder.Eval(Container.DataItem, "BatchNo") + "||" + DataBinder.Eval(Container.DataItem, "MfgDate") + "|" +  DataBinder.Eval(Container.DataItem, "ExpDate") +"||||"  +"DeliveryNote" )  %>' /> --%>
                               <asp:Literal runat="server" ID="Literal1" Text='<%# String.Format("<img width=\"70px\" src=\"../mInbound/Code39Handler.ashx?code={0}\"",DataBinder.Eval(Container.DataItem, "MCode") + "|" + DataBinder.Eval(Container.DataItem, "BatchNo") + "|" + DataBinder.Eval(Container.DataItem, "SerialNo") + "|" + DataBinder.Eval(Container.DataItem, "MfgDate") + "|" +  DataBinder.Eval(Container.DataItem, "ExpDate") +"|"+DataBinder.Eval(Container.DataItem, "ProjectRefNo")+"||"+DataBinder.Eval(Container.DataItem, "MRP")+"|0"  +"DeliveryNote" )  %>' />
                                <%--      <asp:Literal runat="server" ID="Literal1" Text='<%# String.Format("<img width=\"50px\" src=\"../mInbound/Code39Handler.ashx?code={0}\"",DataBinder.Eval(Container.DataItem, "MCode") + "| 99999999 ||20-feb-20020 | 20-feb-2021||||DeliveryNote" )  %>' /> --%>
                               </div>
                            <p></p>
                             <asp:Literal runat="server" ID="ltMMID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                             <asp:Literal runat="server" ID="ltOBDTrackingID" Visible="false" />
                             <asp:Literal runat="server" ID="ltSOHID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID") %>' />


                         </ItemTemplate>


                     </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText=" SO #/Inv. #">--%>
                      <asp:TemplateField HeaderText="SO#" ItemStyle-Width="120px">
                         <ItemTemplate>
                            <%-- <asp:Literal runat="server" ID="ltSoNo_CusterPo" Visible="true" Text='<%# (DataBinder.Eval(Container.DataItem, "InvoiceNo").ToString()==""? String.Format("{0} / {1} ",DataBinder.Eval(Container.DataItem, "SONumber"),DataBinder.Eval(Container.DataItem, "InvoiceNo") ) :String.Format("{0} / {2}",DataBinder.Eval(Container.DataItem, "SONumber"),DataBinder.Eval(Container.DataItem, "CustPONumber"),DataBinder.Eval(Container.DataItem, "InvoiceNo") ) )%>' />--%>
                              <asp:Literal runat="server" ID="ltSoNo_CusterPo" Visible="true" Text='<%# (DataBinder.Eval(Container.DataItem, "InvoiceNo").ToString()==""? String.Format("{0}",DataBinder.Eval(Container.DataItem, "SONumber")) :String.Format("{0} / {2}",DataBinder.Eval(Container.DataItem, "SONumber"),DataBinder.Eval(Container.DataItem, "CustPONumber"),DataBinder.Eval(Container.DataItem, "InvoiceNo") ) )%>' />
                         </ItemTemplate>
                     </asp:TemplateField>



                     <asp:TemplateField HeaderText="UoM" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                         <ItemTemplate>
                             <%--<asp:Literal runat="server" ID="ltBMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}").ToString() )%>' />--%>
                             <asp:Literal runat="server" ID="ltBMMSKU" Text='<%# String.Format("{0}", DataBinder.Eval(Container.DataItem, "BUoM").ToString())%>' />
                             <asp:Literal runat="server" ID="ltBUoMID" Visible="false" />
                             <asp:Literal runat="server" ID="ltBUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BUoMQty","{0:0.00}") %>' />
                         </ItemTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Min Pic." Visible="false" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltMinMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "MUoM").ToString(),DataBinder.Eval(Container.DataItem, "MUoMQty","{0:0.00}").ToString() )%>' />
                         </ItemTemplate>
                     </asp:TemplateField>


                     <asp:TemplateField HeaderText="Sales UoM" Visible="false" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="center">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltMMSKU" Text='<%# String.Format("{0}/{1}", DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty","{0:0.00}").ToString() )%>' />
                             <asp:Literal runat="server" ID="ltSUoMID" Visible="false" />
                             <asp:Literal runat="server" ID="ltSUoMQty" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SUoMQty","{0:0.00}") %>' />
                         </ItemTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Order Qty." HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" HeaderStyle-HorizontalAlign="Right">
                         <ItemTemplate>
                             <div style="text-align: right;">
                             <asp:Literal runat="server" ID="ltQuanity" Text='<%# DataBinder.Eval(Container.DataItem, "SOQuantity","{0:0.00}") %>' />
                                 </div>
                         </ItemTemplate>
                     </asp:TemplateField>



                     <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltSplitLocation" Visible="true" Text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />
                             <asp:Literal runat="server" ID="ltLocation" Visible="true" />
                             <asp:Literal runat="server" ID="ltLocationID" Visible="false" />
                         </ItemTemplate>
                         
                     </asp:TemplateField>

                      <asp:TemplateField HeaderText="Pallet"  ItemStyle-HorizontalAlign="left">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltQuanity" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode") %>' />
                         </ItemTemplate>
                     </asp:TemplateField>



                     <asp:TemplateField HeaderText="Discount"  ItemStyle-HorizontalAlign="left">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltDiscount" Text='<%# DataBinder.Eval(Container.DataItem, "Discount") %>' />
                         </ItemTemplate>
                     </asp:TemplateField>



                      <asp:TemplateField HeaderText="Is Damaged"  ItemStyle-HorizontalAlign="left" Visible="false">
                         <ItemTemplate>
                             <asp:Literal runat="server" ID="ltIsDamaged" Text='<%# DataBinder.Eval(Container.DataItem, "IsDamaged") %>' />
                         </ItemTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Assigned Qty." HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight">
                         <ItemTemplate>
                             <div style="text-align: right;">
                            <%-- <asp:Literal runat="server" ID="ltAssignedQuanity" Text='<%# DataBinder.Eval(Container.DataItem, "AssignedQuantity","{0:0.00}") %>' />--%>
                            <asp:Label ID="ltAssignedQuanity" runat="server" CssClass="lblAQty" Text='<%# DataBinder.Eval(Container.DataItem, "AssignedQuantity","{0:0.00}") %>'></asp:Label>
                                 </div>
                         </ItemTemplate>
                         <FooterTemplate>
                             <div style="text-align: right;">
                             <label class="lblAQtyTotal" style="text-align: left; padding: 0px; width: 100%; color: black; font-weight: 700;"></label>
                                 </div>
                         </FooterTemplate>
                     </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Batch#" ItemStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <asp:Literal ID="ltBatchNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BatchNo") %>'></asp:Literal>
                         </ItemTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Serial#" ItemStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <asp:Literal ID="ltSerialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo") %>'></asp:Literal>
                         </ItemTemplate>
                     </asp:TemplateField>

                      <asp:TemplateField HeaderText="Mfg.&nbsp;Date" HeaderStyle-CssClass="alignCenter" ItemStyle-CssClass="alignCenter" HeaderStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <div style="text-align:center">
                             <asp:Literal ID="ltMFGDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MfgDate") %>'></asp:Literal>
                                 </div>
                         </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Exp.&nbsp;Date" HeaderStyle-CssClass="alignCenter" ItemStyle-CssClass="alignCenter" HeaderStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <div style="text-align:center">
                             <asp:Literal ID="ltExpDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpDate") %>'></asp:Literal>
                                 </div>
                         </ItemTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Project&nbsp;Ref.#" ItemStyle-HorizontalAlign="Center">
                         <ItemTemplate>
                             <asp:Literal ID="ltPojrefno" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectRefNo") %>'></asp:Literal>
                         </ItemTemplate>
                     </asp:TemplateField>
                      <asp:TemplateField HeaderText="MRP" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                         <ItemTemplate>
                             <asp:Literal ID="ltMRP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MRP") %>'></asp:Literal>
                         </ItemTemplate>
                     </asp:TemplateField>
                       <asp:TemplateField HeaderText="HU No." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                         <ItemTemplate>
                             <asp:Literal ID="ltMRP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HUNo") %>'></asp:Literal>
                         </ItemTemplate>
                     </asp:TemplateField>
                      <asp:TemplateField HeaderText="HU Size" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                         <ItemTemplate>
                             <asp:Literal ID="ltMRP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HUSize") %>'></asp:Literal>
                         </ItemTemplate>
                     </asp:TemplateField>
                      <asp:TemplateField HeaderText="Picked Qty." HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-HorizontalAlign="Right">
                         <ItemTemplate>
                             <div style="text-align: right;">
                             <asp:Label ID="ltPickQty" runat="server" CssClass="lblQty" Text='<%# DataBinder.Eval(Container.DataItem, "pickedqty","{0:0.00}") %>'></asp:Label>
                                 </div>
                         </ItemTemplate>
                          <FooterTemplate>
                              <div style="text-align: right;">
                              <label class="lblQtyTotal" style="text-align: left; padding: 0px; width: 100%; color: black; font-weight: 700;"></label>
                                  </div>
                          </FooterTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Total Vol.(m³)" HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" FooterStyle-CssClass="alignRight" HeaderStyle-HorizontalAlign="Right">
                         <ItemTemplate>
                             <div style="text-align: right;">
                              <asp:Label runat="server" Style="padding: 0px !important;text-align: right;" ID="ltVolume" CssClass="lblVolume" Text='<%# DataBinder.Eval(Container.DataItem, "TotalVolume","{0:0.00}") %>' />
                                 </div>
                         </ItemTemplate>
                         <FooterTemplate>
                             <div style="text-align: right;">
                             <asp:Label runat="server" ID="ltVolume" Style="text-align: right; padding: 0px; width: 100%; color: black; font-weight: 700;" CssClass="lblTotalVolume"></asp:Label>
                                 </div>
                         </FooterTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField  HeaderText="Pick" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hideContent NoPrint" HeaderStyle-CssClass="hideContent NoPrint" FooterStyle-CssClass="NoPrint">
                         <ItemTemplate>
                             <%--<asp:LinkButton ID="lnkPick" runat="server" OnClick="lnkPick_Click" >Pick</asp:LinkButton>--%>
                             <a style='text-decoration:none;cursor:pointer;display:<%# DataBinder.Eval(Container.DataItem, "IsVLPDPicking")%>' id="btnPick" onclick="OpenPickDislog('<%# DataBinder.Eval(Container.DataItem, "MRP")%>',<%# DataBinder.Eval(Container.DataItem, "AssignedQuantity")%>,<%# DataBinder.Eval(Container.DataItem, "pickedqty")%>,<%# DataBinder.Eval(Container.DataItem, "AssignID")%>,<%# DataBinder.Eval(Container.DataItem, "SODetailsID")%>,<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID")%>,'<%# DataBinder.Eval(Container.DataItem, "OBDNumber")%>','<%# DataBinder.Eval(Container.DataItem, "LineNumber")%>','<%# DataBinder.Eval(Container.DataItem, "SOHeaderID")%>','<%# DataBinder.Eval(Container.DataItem, "Location")%>','<%# DataBinder.Eval(Container.DataItem, "MCode")%>','<%# DataBinder.Eval(Container.DataItem, "MfgDate")%>','<%# DataBinder.Eval(Container.DataItem, "ExpDate")%>','<%# DataBinder.Eval(Container.DataItem, "BatchNo")%>','<%# DataBinder.Eval(Container.DataItem, "SerialNo")%>','<%# DataBinder.Eval(Container.DataItem, "ProjectRefNo")%>','<%# DataBinder.Eval(Container.DataItem, "CartonCode")%>','<%# DataBinder.Eval(Container.DataItem, "HUNo")%>','<%# DataBinder.Eval(Container.DataItem, "HUSize")%>')"> <%= MRLWMSC21Common.CommonLogic.btnfapick %></a>
                         </ItemTemplate>
                         <%--<FooterTemplate>
                Note: Strike-off row items cannot be picked due to their parameters difference than the required items in Delv.Doc.
                </FooterTemplate>--%>
                     </asp:TemplateField>

                 </Columns>

                 <FooterStyle CssClass="gvSilver_footerGrid"/>
                 <RowStyle CssClass="gvSilver_DataCellGrid" />
                 <EditRowStyle CssClass="gvSilver_DataCellGridEdit" />
                 <PagerStyle CssClass="gvSilver_pagerGrid" />
                 <HeaderStyle CssClass="gvSilver_headerGrid" />
                 <AlternatingRowStyle CssClass="gvSilver_DataCellGrid" />

             </asp:GridView>
            </div>
        </td>

      </tr>
  	         	        	
  	   	

        </table>

                      <div style="display: table-footer-group; position: fixed; bottom: -5px; clear: both; right: 0; left: 0px; page-break-inside: avoid; page-break-before: always;">
                          <table width="100%" class="tableSize">
                              <tr>
                                  <td colspan="12">
                                      <div style="display: flex; justify-content: space-between; align-items: center; padding: 0px 10px">
                                          <span><b>Delivery Doc.#:&nbsp;</b><asp:Literal ID="ltDelvDocNo1" runat="server"></asp:Literal></span>
                                          <span><small>Printed On : <%= DateTime.Now.ToString("dd-MMM-yyyy") %>&nbsp;<%= DateTime.Now.ToString("hh:mm tt") %></small> </span>
                                          <span>www.merlinwms.in</span>
                                      </div>
                                  </td>
                                  <%--<td><small>Printed On : <%= DateTime.Now.ToString("dd-MMM-yyyy") %>&nbsp;<%= DateTime.Now.ToString("hh:mm tt") %></small><br />
                                      <span style="vertical-align: top !important; font-weight: bold;">Delivery Doc. No.</span>
                                      <span><b>:&nbsp;</b><asp:Literal ID="ltDelvDocNo1" runat="server"></asp:Literal></span>
                                  </td>
                                  <td style="text-align: right;">www.merlinwms.in</td>--%>
                              </tr>
                          </table>
                      </div>
  </asp:Panel>
                 </div>
        <//div>
    </div>
        
    <div id="divContainer" class="PopupContainerInbound" style="display:none" style="height:300px">
            <div id="divInner" class="PopupInnerOutbound" style="    width: 47%;">
                <div class="PopupHeadertextOutbound">Picking Item</div>
                    <span id="spanClose" class="fa fa-times PopupSpanCloseOutbound" aria-hidden="true"></span>&emsp;
                        <div class="PopupPaddingOutbound">
                            <div class="PopupSpaceOutbound">
                                <br />
                              <table>
                                 <tr>
                                     <td>
                                    <div class="row">                                       
                                        <div class="col m4 offset-m2">
                                        <div class="flex">

                                            <input type="text" id="txtPickQty" required="" />
                                            <label>
                                                Enter Quantity</label>
                                            <span class="errorMsg"></span>
                                        </div>
                                            </div>
                                       <div class="col m4">
                                      <%--  <div class="flex">

                                            <input type="text" id="txtcontainer" required="" />
                                            <label>
                                                Intermediate Container</label>
                                            
                                        </div>--%>
                                           </div>
                                         <div class="col m4">
                                             <br />
                                            <div>
                                                <a style="text-decoration:none;cursor:pointer;" id="btnPick" class="btn btn-primary obd" onclick="PickItem()" >Pick <%= MRLWMSC21Common.CommonLogic.btnfapick %></a>
                                            </div>
                                             </div>
                                        </div>
                                         </td>
                                     </tr>
                                    </table>
                                 <div id="divReceivedDetails">

                       
                        <br />
                        <div>
                        <table id="tblPalletDetails" class="table-striped">                           
                        </table></div>
                    </div>
                                    <div>   
                                         
                                        <%--<td style="text-align:center;">
                                            
                                           <a style="text-decoration:none;cursor:pointer;" id="btnPick" class="btn btn-primary obd" onclick="PickItem()" >Pick <%= MRLWMSC21Common.CommonLogic.btnfapick %></a>
                                          

                                        </td>--%>
                                            <%--<button type="button" id="btnCreateInbound"  ng-click = "pickItem();" class="addbuttonOutbound" style="width:86px;">Pick <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button> --%>
                                           <%-- &nbsp;<button type="button" id="btnClose" class="addbuttonOutbound" style="width:86px;">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button></td>--%>
                                                              </tr>
                                                                   </table>
                                                                <br /> <br /> <br />
                               <%-- Material : <label id="lblMaterialMasterID"></label>--%>
                                <asp:HiddenField id="hdnmmid" runat="server"/>
                                <asp:HiddenField id="hdnOBDNo" runat="server"/>
                                <asp:HiddenField id="hdnLineNo" runat="server"/>
                                <asp:HiddenField id="hdnsoh" runat="server"/>
                                <asp:HiddenField id="hdnloc" runat="server"/>
                                <asp:HiddenField id="hdnmcode" runat="server"/>
                                
                             
                            </div>
                        </div>
                    </div>                
            </div>


    <br /><br />



    </div>



</asp:Content>
