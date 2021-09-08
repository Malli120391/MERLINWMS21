$(document).ready(function () {
    $("#alertdialog").dialog({
        autoOpen: false,
        hide: "close",
        draggable: false,
        modal: true,
        width: 500,
       
    });
    $("#locationcreatedialog").dialog({
        autoOpen: false,
        width: 500,
        height: 400,
        hide: "close",
        modal: true,
        draggable: true,
        resizable:false
    });
    $("#beamdialog").dialog({
        autoOpen: false,
        width: 500,
        height: 200,
        hide: "close",
        modal: true,
        draggable: true,
    });
    $("#aisleslider").slider({
        range: true,
        width: 10,
        min: 65,
        max: 91,
        values: [65, 91],
        slide: function (event, ui) {
            aislefrom = ui.values[0];
            aisleto = ui.values[1];
            aisleval.innerHTML = String.fromCharCode(aislefrom);
            if (aisleto != 91) {
                aisleval1.innerHTML = String.fromCharCode(aisleto);
            } else {
                aisleto = aislefrom;
                aisleval1.innerHTML = "";
            }

        }

    });
    $("#bayslider").slider({
        range: true,
        width: 10,
        min: 1,
        max: 100,
        values: [1, 100],
        slide: function (event, ui) {
            bayfrom = ui.values[0];
            bayto = ui.values[1];
            bayval.innerHTML = bayfrom;
            if (bayto != 100) {
                bayval1.innerHTML = bayto;
            } else {
                bayto = bayfrom;
                bayval1.innerHTML = "";
            }

        }

    });
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
            beamval.innerHTML = String.fromCharCode(beamfrom);
            if (beamto != 91) {
                beamval1.innerHTML = String.fromCharCode(beamto);
            } else {
                beamto = beamfrom;
                beamval1.innerHTML = "";
            }

        }

    });
    $("#locslider").slider({
        range: true,
        width: 10,
        min: 1,
        max: 100,
        values: [1, 100],
        slide: function (event, ui) {
            locfrom = ui.values[0];
            locto = ui.values[1];
            locval.innerHTML = locfrom;
            if (locto != 100) {
                locval1.innerHTML = locto;
            } else {
                locto = locfrom;
                locval1.innerHTML = "";
            }

        }

    });
    $("#beamlocslider").slider({
        range: true,
        width: 10,
        min: 1,
        max: 100,
        values: [1, 100],
        slide: function (event, ui) {
            beamlocfrom = ui.values[0];
            beamlocto = ui.values[1];
            beamlocval.innerHTML = beamlocfrom;
            if (beamlocto != 100) {
                beamlocval1.innerHTML = beamlocto;
            } else {
                beamlocto = beamlocfrom;
                beamlocval1.innerHTML = "";
            }

        }
    });
    $("#myCanvas").bind("mousemove", function (event) {
        myCanvas.style.cursor = "auto";
        var elem = document.getElementById("myCanvas");
        elemLeft = elem.offsetLeft,
        elemTop = elem.offsetTop;
        var xposition = event.pageX;
        var yposition = event.pageY;
        var x = xposition - elemLeft;
        var y = yposition - elemTop;
        locdetailsarray.forEach(function (location) {
            var x = event.pageX - elemLeft, y = event.pageY - elemTop;
            if (y > location.yaxis && y < location.yaxis + location.height && x > location.xaxis && x < location.xaxis + location.width) {
                myCanvas.style.cursor = "pointer";
                //Checking Condition for Clicked Location
                if (location.name.length != 6) {
                    var loccode =location.name.substr(0, 8);
                    var locdetails = location.name.substr(9).split('#');
                    var splitdetails = "";
                    for (locdata in locdetails) {
                        splitdetails += locdetails[locdata] + "<br>";
                    }
                    if (splitdetails == "<br>") {
                        tool.style.color = "blue";
                        tool.innerHTML = "<u>" + loccode + "</u><br><font color='black'>Location Is Empty";
                    } else {
                        tool.style.color = "blue";
                        tool.innerHTML = "<u>" + loccode + "</u><br><font color='black'>" + splitdetails;
                    }
                    $("#tool").css(
                     {
                         top: event.pageY + 5 + "px",
                         left: event.pageX + 5 + "px"
                     }).show();

                }
            }
        });
        $("#tool").bind("mousedown", function (event) { $("#tool").hide(); });
    });
    $('#myCanvas').mousedown(function (event) 
    {
        var elem = document.getElementById("myCanvas");
        canvasContext = elem.getContext("2d");
        var xposition = event.pageX;
        var yposition = event.pageY;
        locdetailsarray.forEach(function (element) 
        {
            var x = xposition - elemLeft, y = yposition - elemTop;
            if (y > element.yaxis && y < element.yaxis + element.height && x > element.xaxis && x < element.xaxis + element.width) 
            {
                //Checking Condition for Clicked Location
                if (element.name.length == 6) 
                {
                    $("#beamdialog").dialog('option', 'position', [xposition, yposition]);
                    $("#beamdialog").dialog('option', 'title', "beam Loc is " + element.name);
                    $("#beamdialog").dialog('open');
                    $("#beamdialog").css("background-color", "lightgreen");
                    beamcode = element.name;
                } else 
                {
                    var count = 0;
                    selecteddataarray.forEach(function (sdata)
                    {
                        if (sdata.name == element.name) 
                        {
                            count++;
                            var index = selecteddataarray.indexOf(sdata);
                            selecteddataarray.splice(index, 1);
                        }
                    });
                    if (count == 0) 
                    {
                        canvasContext.fillStyle = "red";
                        canvasContext.fillRect(element.xaxis, element.yaxis, element.width, element.height);
                        canvasContext.fillStyle = "black";
                        canvasContext.fillText(element.name.substr(6,2), element.xaxis, element.yaxis + 8);
                        selecteddataarray.push({
                            name: element.name,
                            xaxis:element.xaxis,
                            yaxis: element.yaxis,
                            width: element.width,
                            height: element.height,
                            color:element.color
                        });
                    } 
                    else 
                    {
                        canvasContext.fillStyle = element.color;
                        canvasContext.fillRect(element.xaxis, element.yaxis, element.width, element.height);
                        canvasContext.fillStyle = "black";
                        canvasContext.fillText(element.name.substr(6,2), element.xaxis, element.yaxis + 8);

                    }
                }
            }

        });  
    });
   

});


