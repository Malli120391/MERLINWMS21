$(document).ready(function () {
    $("#alertdialog").dialog({
        autoOpen: false,
        hide: "close",
        draggable: false,
        modal: true,
        width: 500,
        position: {
            my: "center",
            at: "center",
            of: window
        }



    });
    $("#locationcreatedialog").dialog({
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
    $("#beamdialog").dialog({
        autoOpen: false,
        width: 500,
        height: 200,
        hide: "close",
        modal: true,
        draggable: true,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#canwidth").slider({
        min: 1200,
        max: 5000,
        values: [1000],
        slide: function (event, ui) {
            canvaswidth = ui.values[0];
        }
    }
        );
    $("#canheight").slider({
        min: 2000,
        max: 7000,
        values: [1000],
        slide: function (event, ui) {
            canvasheight = ui.values[0];
        }
    }
       );
    $("#bayslider").slider({
        range: true,
        width: 10,
        min: 1,
        max: 100,
        values: [1, 100],
        slide: function (event, ui) {
            bayfrom = ui.values[0];
            bayto = ui.values[1];
            if (ui.values[0] > 99) {
                return false;
            }
            bayval.innerHTML = "<b>" + bayfrom + "</b>";
            if (bayto != 100) {
                bayval.innerHTML = bayval.innerHTML + "     to    <b>" + bayto + "</b>";
            } else {
                bayto = bayfrom;

            }

        }

    });
    $("#bayslider .ui-slider-range").css('background', '#B69F9F');
    $("#beamslider").slider({
        range: true,
        enable: false,
        width: 10,
        min: 65,
        max: 91,
        values: [65, 91],
        slide: function (event, ui) {
            beamfrom = ui.values[0];
            beamto = ui.values[1];
            if (ui.values[0] > 90) {
                return false;
            }
            beamval.innerHTML = "<b>" + String.fromCharCode(beamfrom) + "</b>";
            if (beamto != 91) {
                beamval.innerHTML = beamval.innerHTML + "     to   <b> " + String.fromCharCode(beamto) + "</b>";
                //beamval1.innerHTML = String.fromCharCode(beamto);
            } else {
                beamto = beamfrom;
            }

        }

    });
    $("#beamslider .ui-slider-range").css('background', '#E8730E');
    $("#locslider").slider({
        range: true,
        width: 10,
        min: 1,
        max: 100,
        values: [1, 100],
        slide: function (event, ui) {
            locfrom = ui.values[0];
            locto = ui.values[1];
            if (ui.values[0] > 99) {
                return false;
            }
            locval.innerHTML = "<b>" + locfrom + "</b>";
            if (locto != 100) {
                locval.innerHTML = locval.innerHTML + "     to    <b>" + locto + "</b>";
            } else {
                locto = locfrom;
            }

        }

    });
    $("#locslider .ui-slider-range").css('background', '#504545');
    $("#beamlocslider").slider({
        range: true,
        width: 10,
        min: 1,
        max: 100,
        values: [1, 100],
        slide: function (event, ui) {
            beamlocfrom = ui.values[0];
            beamlocto = ui.values[1];
            if (ui.values[0] > 99) {
                return false;
            }
            beamlocval.innerHTML = beamlocfrom;
            if (beamlocto != 100) {
                beamlocval1.innerHTML = beamlocto;
            } else {
                beamlocto = beamlocfrom;
                beamlocval1.innerHTML = "";
            }

        }
    });
    $("#canwidth").bind("mouseup", function (event) {
        getLocations();
    });
    $("#canheight").bind("mouseup", function (event) {
        getLocations();
    });
    $("#myCanvas").bind("mousemove", function (event) {
        myCanvas.style.cursor = "auto";
        var elem = document.getElementById("myCanvas");
        elemLeft = elem.offsetLeft,
        elemTop = elem.offsetTop;
        var xposition = event.pageX;
        var yposition = event.pageY;
        locdetailsarray.forEach(function (location) {
         
            var x = xposition - elemLeft, y = yposition - elemTop;
            if (y > location.yaxis && y < location.yaxis + location.height && x > location.xaxis && x < location.xaxis + location.width)
            {
                myCanvas.style.cursor = "pointer";
                //Checking Condition for Clicked Location
              
                if (location.name.length != 5)
                {
                    var reservedLoc = location.name.substr(7, 1);
                    
                    var loccode = location.name.substr(0, 7);
                  
                    
                    var locdetails = location.name.substr(9).split('#');
                 
                    var splitdetails = "";
                    
                    for (locdata in locdetails)
                    {
                        
                        var str = locdetails[locdata].toString();
                        var str1 = str.substring(0, str.indexOf('$'));
                        splitdetails += str1 + "<br>";
                    }
                   
                    if (splitdetails == "<br>")
                    {
                       
                        tool.style.color = "blue";
                        tool.innerHTML = "<b>" + loccode + "</b><br><font color='black'>Location Is Empty";
                        if (reservedLoc == "0")
                        {
                            tool.innerHTML= "<b>" + loccode + "</b><br><font color='black'>Location Is Blocked";
                        }

                    } else
                    {
                       
                        tool.style.color = "blue";
                        tool.innerHTML = "<b>" + loccode + "</b><br><font color='black' face='verdana'>" + splitdetails;
                        if (reservedLoc == "0")
                        {
                            tool.innerHTML = "<b>" + loccode + "</b><br><font color='black'>Location Is Blocked";
                        }
                    }
                    $("#tool").css(
                     {
                         top: event.pageY + 10 + "px",
                         left: event.pageX - 15 + "px"
                     }).show();

                }
            }
        });
        $("#tool").bind("mouseover", function (event) { $("#tool").show(); });
        $("#tool").bind("mouseout", function (event) { $("#tool").hide(); });
        $("#myCanvas").bind("mouseout", function (event) { $("#tool").hide(); });
    });
    $('#myCanvas').mousedown(function (event) {
        var elem = document.getElementById("myCanvas");
        canvasContext = elem.getContext("2d");
        var xposition = event.pageX;
        var yposition = event.pageY;
        locdetailsarray.forEach(function (element)
        {
            var x = xposition - elemLeft, y = yposition - elemTop;
            if (y > element.yaxis && y < element.yaxis + element.height && x > element.xaxis && x < element.xaxis + element.width) {
                //Checking Condition for Clicked Location
                if (element.name.length == 5)
                {
                  

                    $("#beamdialog").dialog('option', 'title', "Bay location : " + element.name);
                    $("#beamdialog").dialog('open');
                    //$("#beamdialog").css("background-color", "lightgreen");
                    beamcode = element.name;
                } else
                {
                    var count = 0;
                  
                    selecteddataarray.forEach(function (sdata)
                    {
                       
                        if (sdata.name == element.name) {
                            count++;
                            var index = selecteddataarray.indexOf(sdata);
                            selecteddataarray.splice(index, 1);
                        }
                    });
                    if (count == 0) {
                        canvasContext.fillStyle = "red";
                        canvasContext.fillRect(element.xaxis, element.yaxis, element.width, element.height);
                        canvasContext.fillStyle = "black";
                        canvasContext.fillText(element.name.substr(5, 2), element.xaxis+6, element.yaxis + 8);
                        selecteddataarray.push({
                            name: element.name,
                            xaxis: element.xaxis,
                            yaxis: element.yaxis,
                            width: element.width,
                            height: element.height,
                            color: element.color
                        });
                    }
                    else {
                        canvasContext.fillStyle = element.color;
                        canvasContext.fillRect(element.xaxis, element.yaxis, element.width, element.height);
                        canvasContext.fillStyle = "black";
                        canvasContext.fillText(element.name.substr(5, 2), element.xaxis+6, element.yaxis + 8);

                    }
                }
            }

        });
    });
});


