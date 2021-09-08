<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.DashBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .mycanvas {
            background-color:red;
        }

         .tooltip
         {
            position:absolute;
            display:none;
            z-index:1000;
             background-color:#8181F7;
            color:white;
            border: 1px solid black;
            width:300px;
            /*height:300px;*/
            padding:10px;
            border-bottom-left-radius:3px;
            border-bottom-right-radius:3px;
            border-top-left-radius:3px;
            border-top-right-radius:3px;
            overflow:auto;
           
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var WorkCenterdetailsSroreArray = new Array();
            var flagfory = 0;
            var x = 50; y = 50;
            var flag = 50;
            var elem = document.getElementById('mycanvas');
            var canvasContext = elem.getContext("2d");
            var prorefid = document.getElementById('<%=lblProRefNumbers.ClientID%>');
            prorefid.style.display = 'none';
            var ProductionReferenceNumbers = prorefid.innerText;
            var ProductionRefnumberArray = ProductionReferenceNumbers.split(',');

            var id = document.getElementById('<%=lbWorkCrenterdata.ClientID%>');
            id.style.display = 'none';
            var ProductionOrderArray = new Array();
            var WorkCenterGroupArray = new Array();
            var WorkceneterGroupDetatilsArray = new Array();
            var WorkCenterListArray = new Array();
            var WorkCenterDetailsArray = new Array();
            var SomeArray = new Array();
            var WorkCenterRamMaterilalArray = new Array();
            var ProductionOrderData = id.innerText;
            var prorefnum = "";
            // alert(ProductionOrderData);
            ProductionOrderArray = ProductionOrderData.split('^');
            for (i = 0; i < ProductionOrderArray.length; i++) 
            {
                //alert("DATA IS  "+ProductionOrderArray[i]);
                var maxWorkGroupCount = getMaxWorkGroups();

                x = 50;
                if (ProductionOrderArray[i].length != "")
                {
                   
                    WorkCenterGroupArray = ProductionOrderArray[i].split(',');
                    var maxwidth = maxWorkGroupCount * 200;
                    var maxCount = getMaxWorkStations();
                    var maxHeight = maxCount *75;
                   
                    var altcolor = (i + 2)%2;
                    if (altcolor == 0)
                    {
                        drawRectangleWithColor(0, y - 25, maxHeight+100, maxwidth, "#CED8F6");
                    } else
                    {
                        drawRectangleWithColor(0, y - 25, maxHeight, maxwidth, "#F8E0E6 ");
                    }
                  
                    var ProductionDetailsArray = ProductionRefnumberArray[i].split('^');
                    for (var vProdetails = 0;vProdetails<ProductionDetailsArray.length; vProdetails++)
                    {
                        DrawingColourDescription(x,y);
                        drawRectangleWithColor(x-50, y-25, 30, maxwidth, "#333333");
                       // alert("yes");
                        drawHeading("Production Order Number :  " + ProductionDetailsArray[0],0, y-5);
                        drawHeading("Quantity :  " + ProductionDetailsArray[1], 400, y-5);
                        drawHeading("Start Date :  " + ProductionDetailsArray[2], 600, y-5);
                        drawHeading("Due Date :  " + ProductionDetailsArray[3], 800, y-5);
                    }
                    
                    flag = flag + 50;
                    y = y + 50;
                    for (var j = 0; j < WorkCenterGroupArray.length; j++)
                    {

                        WorkceneterGroupDetatilsArray = WorkCenterGroupArray[j].split('|');

                        // alert(WorkceneterGroupDetatilsArray.length);
                        draCircle(x, y);
                        //For Drawing Sequence Numbers Inside Circle
                        if (j + 1 <= 9) {
                            drawTextWithColour(j + 1, x - 8, y + 8, "white", "25px caliber Bold");
                        } else {
                            drawTextWithColour(j + 1, x -15, y + 8, "white", "25px caliber Bold");
                        }
                        

                        //For Drawing WorkCenterGroupNames
                        drawTextWithColour(WorkceneterGroupDetatilsArray[0], x-30, y + 40, "#003366", "12px caliber Bold");
                        //Get Max WorStations
                       
                        for (var k = 0; k < WorkceneterGroupDetatilsArray.length; k++)
                        {

                            if (k == 1)
                            {
                                WorkCenterListArray = WorkceneterGroupDetatilsArray[k].split('#');
                                var workcenterscount = WorkCenterListArray.length;
                                var calY = maxCount / workcenterscount;
                                
                                for (var wlist = 0; wlist < WorkCenterListArray.length; wlist++)
                                {
                                   
                                    WorkCenterDetailsArray = WorkCenterListArray[wlist].split('$');
                                    var workCenterProducedQtyArray = WorkCenterDetailsArray[3].split('!');
                                    var vWorkCenterProducedQty = workCenterProducedQtyArray[1];
                                    var vScrapQty = workCenterProducedQtyArray[2];
                                   
                                    //y = y + calY;
                                    
                                    y = y + 50;
                                
                                    //For Getting WorkCenterCode
                                    var WorkCenterCode =WorkCenterDetailsArray[0].substring(0,WorkCenterDetailsArray[0].lastIndexOf('+'));
                                    var WorkCenterName = WorkCenterDetailsArray[0].substring(WorkCenterDetailsArray[0].lastIndexOf('+') + 1);
                                    //Drawing WorkCenterCode
                                    var height = maxHeight + 120;
                                    var totalAvg=height / maxCount;
                                    var workCenterwiseAvg = height / workcenterscount;
                                    var diff = workCenterwiseAvg - totalAvg;
                                   

                               if (diff == 0)
                                  {
                               } else if (diff == (height / 2)) {
                                   y = y + (diff / maxCount);
                                   y = y - 30;
                               } else
                               {

                                   var mid = maxCount / workcenterscount;
                                   mid = mid * 20;
                                   y = y + mid;

                               }
                                   

                                    drawTextWithColour(WorkCenterCode, x - 25, y + 30, "black", "10px caliber Bold");
                                    for (var wrawmatlist = 0; wrawmatlist < WorkCenterDetailsArray.length; wrawmatlist++)
                                    {
                                        if (wrawmatlist == 1) 
                                        {
                                            var assignedQty = WorkCenterDetailsArray[wrawmatlist];
                                            //alert(assignedQty);
                                        }
                                        if (wrawmatlist == 2)
                                        {
                                            //Draw Work Center Rectangles with Work Center Details
                                            var WOrkCentreWiseRawMaterialWithWorkCenterName = WorkCenterName + "+" + WorkCenterDetailsArray[wrawmatlist];
                                            
                                            var workcenterRawMAtListForCheckingIsReceiveQtyArray = WOrkCentreWiseRawMaterialWithWorkCenterName.split('@');
                                            for (var val = 0; val < workcenterRawMAtListForCheckingIsReceiveQtyArray.length; val++)
                                            {
                                                
                                                var vrawMatArray = workcenterRawMAtListForCheckingIsReceiveQtyArray[val].split('!');

                                                if (vrawMatArray[1] == 0)
                                                {

                                                    drawRectangle(x - 20, y + 35, 20, 150, WOrkCentreWiseRawMaterialWithWorkCenterName);
                                                    drawRectangleWithColor(x - 20, y + 35, 20, 150, "#848484");
                                                    break;
                                                } else
                                                {

                                                    var ConsumQty = vrawMatArray[2] / vrawMatArray[3];


                                                    var InProgressQty = (assignedQty - vWorkCenterProducedQty) - vScrapQty;
                                                    var vBalQty = (assignedQty - vWorkCenterProducedQty) - vScrapQty;
                                                    drawRectangle(x - 20, y + 35, 20, 150, WOrkCentreWiseRawMaterialWithWorkCenterName);
                                                    //Drawing smaalBoxesWithColours
                                                    drawRectangleWithColor(x - 20, y + 35, 20, 30, "#66CCFF");
                                                    //Drawing Assigned Quantity
                                                    drawTextWithColour(assignedQty, x - 15, y + 50, "#000000", "10px caliber Bold");
                                                  
                                                    drawRectangleWithColor(x + 10, y + 35, 20, 30, "#FF9933");
                                                    //Drawing InProgress Quantity
                                                    drawTextWithColour(InProgressQty, x +15, y + 50, "#000000", "10px caliber Bold");

                                                    drawRectangleWithColor(x + 40, y + 35, 20, 30, "#CCFF66");
                                                    //Drawing Produced  Quantity
                                                    drawTextWithColour(vWorkCenterProducedQty, x + 45, y + 50, "#000000", "10px caliber Bold");

                                                    drawRectangleWithColor(x + 70, y + 35, 20, 30, "#9933CC");
                                                    //Drawing Balanced  Quantity
                                                    drawTextWithColour(vBalQty, x + 75, y + 50, "#000000", "10px caliber Bold");

                                                    drawRectangleWithColor(x + 100, y + 35, 20, 30, "#FF0000");
                                                    //Drawing Balanced  Quantity
                                                    drawTextWithColour(vScrapQty, x + 105, y + 50, "#000000", "10px caliber Bold");
                                                    break;
                                                }
 
                                               
                                            }
                                           

                                        }
                                    }
                                }


                            }
                        }
                        
                        if (y > flagfory)
                        {
                            flagfory = y;
                           
                        }
                        y = flag;
                        x = x + 250;
                       // x = x + WorkceneterGroupDetatilsArray[0].length*15;


                    }
                
                    y = flagfory+150;
                    flag = flagfory+150;
                }
               
            }
          
            function DrawingColourDescription(x,y)
            {
                x = x -50;
                y = y -45;
                //Color for Assigned Qty
                drawRectangleWithColor(x, y, 20, 20, "#66CCFF");
                x = x + 30;
                drawTextWithColour("Assigned Quantity",x, y+15, "#000000", "12px caliber Bold");
                
                //Color For InProgress
                x = x + 100;
                drawRectangleWithColor(x, y, 20, 20, "#FF9933");
                x = x + 30;
                drawTextWithColour("InProgress Quantity", x, y+15, "#000000", "12px caliber Bold");

                x = x + 100;
                drawRectangleWithColor(x, y, 20, 20, "#CCFF66");
                x = x + 30;
                drawTextWithColour("Produced Quantity",x, y+15, "#000000", "12px caliber Bold");


                x = x + 100;
                drawRectangleWithColor(x, y, 20, 20, "#9933CC");
                x = x + 30;
                drawTextWithColour("Balanced Quantity",x, y+15, "#000000", "12px caliber Bold");

                x = x + 100;
                drawRectangleWithColor(x, y, 20, 20, "#FF0000");
                x = x + 30;
                drawTextWithColour("Unconfirmed Quantity", x, y+15, "#000000", "12px caliber Bold");
            }
       
      
            function drawText(str, x, y)
            {
                canvasContext.font = "22px caliber";
                canvasContext.fillStyle = "#003366";
                canvasContext.fillText(str, x, y);
            }
            function drawRectangle(x,y,h,w,details)
            {
                
                canvasContext.fillStyle = "#003366";
                canvasContext.fillRect(x, y, w, h);
                WorkCenterdetailsSroreArray.push(
                    {
                        width: w,
                        height: h,
                        xaxis: x,
                        yaxis: y,
                        name: details
                    });

           
            }
            function drawRectangleWithColor(x, y, h, w, color)
            {
                canvasContext.fillStyle = color;
                canvasContext.fillRect(x, y, w, h);
             
            }
            function drawHeading(str, x, y)
            {
                canvasContext.font = "15px caliber Bold";
                canvasContext.fillStyle = "#FFFFFF";
                canvasContext.fillText(str, x, y);
            }
            function drawTextWithColour(str, x, y, color, font)
            {
               
                canvasContext.font = font;
                canvasContext.fillStyle = color;
                canvasContext.fillText(str, x, y);
               
            }
            function draCircle(x, y) {
                canvasContext.beginPath();
                canvasContext.arc(x, y,20, 0,50,false);
                canvasContext.fillStyle = "#8181F7";
                canvasContext.fill();
            }
            function getMaxWorkStations()
            {
              
                var count = 0;
                for (var vworkgrp = 0; vworkgrp < WorkCenterGroupArray.length; vworkgrp++)
                 {
                     var WorkCenterListArrayforCount = WorkCenterGroupArray[vworkgrp].split('#');
                    
                     if (count < WorkCenterListArrayforCount.length)
                     {
                         count = WorkCenterListArrayforCount.length;

                     }

                }
                return count;
            }
            function getMaxWorkGroups()
            {
                var count = 0;
                for (var vworkGroups = 0; vworkGroups < ProductionOrderArray.length; vworkGroups++)
                {
                    var workgroupListarrayforMaxWorkGroups = ProductionOrderArray[vworkGroups].split(',');
                    if (count < workgroupListarrayforMaxWorkGroups.length) {
                        count = workgroupListarrayforMaxWorkGroups.length;
                    }
                  
                }
                return count;
                
            }

            $("#mycanvas").bind("mousemove", function (event)
            {
                mycanvas.style.cursor = "auto";
                //myCanvas.style.cursor = "auto";
                var elem = document.getElementById("mycanvas");
                elemLeft = elem.offsetLeft;
                 elemTop = elem.offsetTop;
                 WorkCenterdetailsSroreArray.forEach(function (details)
                 {
                     var tooltip = document.getElementById('tool');
                     var x = event.pageX - 13;
                     var y = event.pageY - 160;
                    
                     if (y > details.yaxis && y < details.yaxis + details.height && x > details.xaxis && x < details.xaxis + details.width) {
                         mycanvas.style.cursor = "pointer";
                         var RawMaterialDatawithWorkCenterName = details.name;
                         var WorkCenterName = RawMaterialDatawithWorkCenterName.substring(0, RawMaterialDatawithWorkCenterName.lastIndexOf('+'));
                         var RawMaterialData = RawMaterialDatawithWorkCenterName.substring(RawMaterialDatawithWorkCenterName.lastIndexOf('+') + 1);
                             
                         var RawMaterialDataSplit = RawMaterialData.split('@');
                         
                         tooltip.innerHTML = "";
                         tooltip.innerHTML += "<font color=\"#0000FF\"><b>" + WorkCenterName + "</b></font><br>";
                         var preparetabledata = "";
                         preparetabledata += "<center><table border='1' cellpadding='5' cellspacing='1' style=\"border:1px solid black;border-collapse:collapse;\"><th>MaterialCode</th><th>Received Qty</th><th>ConsumeQty</th>";
                      
                         for (var i = 0; i < RawMaterialDataSplit.length; i++)
                         {
                             
                             var rawmaterialDetailsSplit = RawMaterialDataSplit[i].split('!');
                             preparetabledata += "<tr>"
                             tooltip.innerHTML += "<tr>";
                            
                             for (var k = 0; k < rawmaterialDetailsSplit.length; k++)
                             {
                                 if(k!=3)
                                     preparetabledata += "<td><font color=\"#000000\"><b>" + rawmaterialDetailsSplit[k] + "</b></td>";
                                
                              
                               
                             }
                             preparetabledata += "<tr>"
                         }
                         preparetabledata += "</table></center>"
                         preparetabledata += "</font>";
                      
                         tooltip.innerHTML += preparetabledata;

                         tooltip.innerHTML += "<br><br>";
                        
                         $("#tool").css(
                        {
                            top: event.pageY + 5 + "px",
                            left: event.pageX + 5 + "px"
                        }).show();

                     }
                   
                 });
              

            });
            $("#tool").bind("mouseover", function (event) { $("#tool").show(); });
            $("#tool").bind("mouseout", function (event) { $("#tool").hide(); });
            $("#mycanvas").bind("mouseout", function (event) { $("#tool").hide(); });

        });
          
               
           
                
        

    </script>
    <asp:Label ID="lblProRefNumbers" runat="server"></asp:Label>
    <asp:label ID="lbWorkCrenterdata" runat="server"></asp:label>
    <canvas id="mycanvas" width="3000" height="2000">
        this is visible when ur browser does not suppourts canvas
    </canvas>
       <div class="tooltip" id="tool">Tooltip!</div>
</asp:Content>
