<%@ Page Title="Location Management" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="LocationTree.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.LocationTree" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <style type="text/css">

    
      .locInfo {
        width:850px;
        height:35px;
        color:#333df1;
        font-weight:800;
       }
         .Inhold {
                color:#0c10b0;
            }

         .Outhold {
                color:#DE1A00;
            }

        .ModuleHeader {
            width:250px
        }


       
       .WhiteLabel
       {
            background-color:#E6E3DF;
	        font-size:1pt;  color: #FFFFFF; text-decoration: none;
       }

        .YellowLabel
       {
            background-color:#ADFF2F;
	        font-size:1pt;  color: #FFFFFF; text-decoration: none;
       }

        .GreyLabel
       {
            background-color:#848484;
	        font-size:1pt;  color: #FFFFFF; text-decoration: none;
       }

        .BlueLabel
        {
	        background-color:#25538E;
	        font-size:1pt;  color: #FFFFFF; text-decoration: none;
        }

        .PinkLabel
       {
            background-color:#ff4dff;
	        font-size:1pt;  color: #FFFFFF; text-decoration: none;
       }

        #locationcreatedialog td[colspan="2"] {
                padding-top: 15px;
        }

        #locationcreatedialog input[type="checkbox"] {
            height:fit-content !important;
        }

        .cssdailog .textcss{
            margin-bottom:10px;
        }
    </style>

    <script type="text/javascript" src="jQuery2/jquery.blockUI.js"></script>

 
    <script type="text/javascript" language="javascript">
            
            function UnBlockDialog() {
                $.unblockUI();
            }

            function BlockDialog()
            {
              
               $.blockUI({ message: '<h3> Just a moment...</h3>' });
            }
    </script>

    <script>
        $(document).ready(function () {
            $('#txtLevelAdd').attr('disabled', 'disabled').val('');
            $('#txtLevelMand').hide(0);

            $('#ddlColumnOrLevel').change(function () {
                if ($(this).val() == "1") {
                    $('#txtColumnAdd').removeAttr('disabled').val('');
                    $('#txtColumnMand').show(0);

                    $('#txtLevelAdd').attr('disabled', 'disabled').val('');
                    $('#txtLevelMand').hide(0);
                }
                else {
                    $('#txtLevelAdd').removeAttr('disabled').val('');
                    $('#txtLevelMand').show(0);

                    $('#txtColumnAdd').attr('disabled', 'disabled').val('');
                    $('#txtColumnMand').hide(0);
                }
            });
           
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
                width: 700,
                height: 450,
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
            $("#Addinglocationcreatedialog").dialog({
                autoOpen: false,
                width: 700,
                height: 450,
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
            $("#printlocationdialog").dialog({
                autoOpen: false,
                width: 650,
                height: 420,
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
                width: 600,
                height: 330,
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


                       

            $("#canwidth").bind("mouseup", function (event) {
               // getLocations();
                GetLocations();
            });
            $("#canheight").bind("mouseup", function (event) {
                //getLocations();
                GetLocations();
            });
            $("#myCanvas").bind("mousemove", function (event) {
                
                myCanvas.style.cursor = "auto";
                var elem = document.getElementById("myCanvas");

                // var x = xposition - elemLeft - 80, y = yposition - elemTop - 35;


                elemLeft = elem.offsetLeft,
                    elemTop = elem.offsetTop;
                var xposition = event.pageX;
                var yposition = event.pageY;
                locdetailsarray.forEach(function (location) {

                    var x = xposition - elemLeft - 76, y = yposition - elemTop - 38;
                    if (y > location.yaxis && y < location.yaxis + location.height && x > location.xaxis && x < location.xaxis + location.width) {
                        myCanvas.style.cursor = "pointer";
                        //Checking Condition for Clicked Location

                        if (location.name.length != 5) {


                            var reservedLoc = location.name.substr(7, 2);
                            var loccode = location.name.substr(0, 9);
                            var Locations = location.name.split('@')[0];
                            var AllocatedLocData = Locations.split('|')[0];
                            var ActiveLocation = AllocatedLocData.split('!')[0];
                            var AllocatedTenant = AllocatedLocData.split('!')[2];
                            var AllocatedMaterial = AllocatedLocData.split('!')[3];
                            var UpdatedData = Locations.split('|')[1];

                            

                            //var reservedLoc = location.name.substr(7, 2);
                            //var loccode = location.name.substr(0, 9);                           
                            //var Locations = location.name.split('zzMakkyzz')[3];
                            //var AllocatedLocData = Locations.split('|')[0];
                            //var ActiveLocation = AllocatedLocData.split('!')[0];
                            //var AllocatedTenant =  location.name.split('zzMakkyzz')[2];
                            //var AllocatedMaterial = location.name.split('zzMakkyzz')[1];//  AllocatedLocData.split('!')[3];
                            //var UpdatedData = Locations;// Locations.split('|')[1];

                            var Materials = "";
                            if (AllocatedMaterial != "")
                                Materials = "- [ " + AllocatedMaterial + " ]";

                            var MaterialData = UpdatedData.split('Œ');

                            var splitdetails = "";
                            for (MData in MaterialData) {

                                var MdataWithQty = MaterialData[MData].toString().split('á');
                                var splitdetailsWithQty = "";
                                var Count = 0;

                                for (QtyDetails in MdataWithQty) {
                                    var str = "";
                                    if (Count == 0) {
                                        if ((QtyDetails == MdataWithQty.length - 2 && MdataWithQty[QtyDetails].toString() == 1) || (QtyDetails == MdataWithQty.length - 1 && MdataWithQty[QtyDetails].toString() == 1)) {
                                            str = '<img src="../images/blue_menu_icons/check_cancel.png" />';
                                            Count = Count + 1;
                                        }
                                        else
                                            str = MdataWithQty[QtyDetails].toString();
                                    }

                                    splitdetailsWithQty += str;
                                }
                                splitdetails += splitdetailsWithQty + "<br>";
                            }

                            if (splitdetails == "<br>") {

                                tool.style.color = "blue";
                                tool.runtimeStyle = true;
                                tool.innerHTML = "<b>" + loccode + ' - [ ' + AllocatedTenant + " ] " + Materials + " </b><br><font color='black'>Location Is Empty";
                                if (reservedLoc == "0") {
                                    tool.innerHTML = "<b>" + loccode + ' - [ ' + AllocatedTenant + " ] " + Materials + " </b><br><font color='black'>Location Is Blocked";
                                }

                            } else {

                                tool.style.color = "blue";
                                tool.innerHTML = "<b>" + loccode + ' - [ ' + AllocatedTenant + " ] " + Materials + " </b><br><font color='black' face='verdana'><div>" + splitdetails+"</div>";
                                if (reservedLoc == "0") {
                                    tool.innerHTML = "<b>" + loccode + ' - [ ' + AllocatedTenant + " ] " + Materials + " </b><br><font color='black'>Location Is Blocked";
                                }
                            }

                            //var locdetails = location.name.substr(9).split('Œ')

                            //var splitdetails = "";
                            //for (locdata in locdetails) {

                            //    var str = locdetails[locdata].toString();
                            //    var str1 = str.substring(0, str.indexOf('Ø'));
                            //    splitdetails += str1 + "<br>";
                            //}

                            //if (splitdetails == "<br>") {

                            //    tool.style.color = "blue";
                            //    tool.runtimeStyle = true;
                            //    tool.innerHTML = "<b>" + loccode + "</b><br><font color='black'>Location Is Empty";
                            //    if (reservedLoc == "0") {
                            //        tool.innerHTML = "<b>" + loccode + "</b><br><font color='black'>Location Is Blocked";
                            //    }

                            //} else {

                            //    tool.style.color = "blue";

                            //    tool.innerHTML = "<b>" + loccode + "</b><br><font color='black' face='verdana'>" + splitdetails;
                            //    if (reservedLoc == "0") {
                            //        tool.innerHTML = "<b>" + loccode + "</b><br><font color='black'>Location Is Blocked";
                            //    }
                            //}

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
                
                //alert('mouse Down');
                var elem = document.getElementById("myCanvas");
                canvasContext = elem.getContext("2d");
                var xposition = event.pageX;
                var yposition = event.pageY;
                locdetailsarray.forEach(function (element)
                {                   
                    //var x = xposition - elemLeft, y = yposition - elemTop;
                    var x = xposition - elemLeft - 76, y = yposition - elemTop - 38;
                    if (y > element.yaxis && y < element.yaxis + element.height && x > element.xaxis && x < element.xaxis + element.width)
                    {
                        //Checking Condition for Clicked Location
                        if ('<%=ViewState["TenantID"]%>' == "0")
                        {
                            if (element.name.length == 5)
                            {

                                $("#beamdialog").dialog('option', 'title', "Bay location : " + element.name);
                                $("#beamdialog").dialog('open');
                                $('#txtBaySupplier').empty();
                                //$("#beamdialog").css("background-color", "lightgreen");
                                beamcode = element.name;
                                document.getElementById("trBaySupplier").style.display = "none";
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
                                    canvasContext.fillText(element.name.substr(7, 2), element.xaxis + 6, element.yaxis + 8);



                                    selecteddataarray.push({
                                        name: element.name,
                                        xaxis: element.xaxis,
                                        yaxis: element.yaxis,
                                        width: element.width,
                                        height: element.height,
                                        color: element.color
                                    });
                                }
                                else
                                {
                                   
                                    canvasContext.fillStyle = element.color;
                                    canvasContext.fillRect(element.xaxis, element.yaxis, element.width, element.height);
                                    canvasContext.fillStyle = "black";
                                    canvasContext.fillText(element.name.substr(7, 2), element.xaxis + 6, element.yaxis + 8);

                                }
                            }
                        }
                    }

                });
            });
        });



    </script>
    
    <style>
    .mcodepicker {
       font-family:Calibri;
       position:relative;
       color: #000000; 
       font-size:13pt;
       padding-right:20px;
       width:180px;
       border: 1px solid #848484;
       border-radius: 3px;
       height:22px;
    }
    .mcodepicker:focus {
            outline: none;
    color: #000000; 
    box-shadow: 0px 0px 5px #E67E22;
    border:1px solid #E67E22;
    border-radius: 5px;
    }
    .ButWhite
{
   background-repeat:no-repeat;
    background-position:center;
    background-color:#fff;
    font-family:Calibri,Verdana,Geneva,sans-serif;
    text-decoration:none;
	font-size: 15px;
    color:#000;
   
    font-weight:bold;
     padding-left:20px;padding-right:20px;padding-top:9px;padding-bottom:9px;
}
 .ButSearch
{

    width:100px;
    background-color:#ffb033;
    font-family:Calibri;
    text-decoration:none;
	font-size: 15px;
    color:#000;
    font-weight:bold;
     padding-left:20px;padding-right:20px;padding-top:9px;padding-bottom:9px;
}
.ButSave
{  

  
	padding: 9px 10px;
	background-repeat:no-repeat;
    background-position:right;
    background-color:#ffb033;
    font-family:Calibri;
    text-decoration:none;
	font-size: 15px;
    color:#000;
    font-weight:bold;
        width: 130px;
        height: 40px;
}
        select {    min-height: 33px;
        }
.ButCancel
{
        /*sss*/

    width:100px;
   
    background-color:#ffb033;
    font-family:Calibri;
    text-decoration:none;
	font-size: 15px;
    color:#000;
    font-weight:bold;
      padding-left:0px;padding-right:20px;padding-top:9px;padding-bottom:9px;

}

 .ButReceive
{
	
    
	
    background-color:#ffb033;
    font-family:Calibri;
    text-decoration:none;
	font-size: 15px;
    color:#000;
    font-weight:bold;
    z-index:-1;
    border-bottom-style:solid;
        height: 40px;
         width:155px;
}

 .ButAddNew
{
    
   width:120px;
    background-color:#ffb033;
    font-family:Calibri,Verdana,Geneva,sans-serif;
    text-decoration:none;
	font-size: 15px;
    color:#000;
    
    font-weight:bold;
    padding-left:0px;padding-right:20px;padding-top:9px;padding-bottom:9px;
        height: 40px;
    width:73px;
    }
  .BtnPrint
    {
    
   
        padding: 9px 20px;
       background-image:url('../Images/Print 20X20..png');
        background-repeat:no-repeat;
        background-position:center;
        background-color:#ffb033;
        font-family:Calibri,Verdana,Geneva,sans-serif;
        text-decoration:none;
	    font-size: 15px;
        color:#000;
        font-weight:bold;
        height: 40px;
}




    .can 
    {
          background-color:ActiveBorder;
     }
    .slide 
    {
            width:400px;
            z-index:0;    
     }
    .textcss {
       font-family:Calibri;
       position:relative;
       color: #000000; 
       font-size:13pt;
       padding-right:20px;
       border: 1px solid #848484;
       border-radius: 3px;
        width:100px;
       height:22px
    }
    .textcss:focus {
        outline: none;
    color: #000000; 
    box-shadow: 0px 0px 5px #E67E22;
    border:1px solid #E67E22;
    border-radius: 5px;
    }
    .tooltip
        {
            position:absolute;
            display:none;
            z-index:1000;
             background-color:#BDA670;
            color:white;
            border: 1px solid black;
            width:400px;
            height:150px;
            padding:10px;
            border-bottom-left-radius:10px;
            border-bottom-right-radius:10px;
            border-top-left-radius:10px;
            border-top-right-radius:10px;
            overflow:auto;
           
        }
    .cssdailog {

    }

        .txt_Blue_Small {
            min-height:33px !important;
        }


    .ui-autocomplete 
    { 
      position: absolute;   
      height:200px;
      overflow-y:scroll;
    }
    .ui-state-hover
     {
         cursor:pointer;
    }
    .ui-slider {
          position: absolute;   
    }
    
    .auto-style1 {
        width: 1055px;
    }
    
    </style>
    
    <script type="text/javascript">
           $(document).ready(function () {
             
               $("#updatebindialog").dialog({
                   autoOpen: false,
                   hide: "close",
                   draggable: true,
                   width: 650,
                   height: 450,
                   modal: true,
                   resizable: false
               });
               $("#createlocalert").dialog({
                   autoOpen: false,
                   hide: "close",
                   draggable: true,
                   width: 500,
                   height: 150,
                   modal: true,
                   resizable: false,
                   position: {
                       my: "center",
                       at: "center",
                       of: window
                   }
               });
               $("#Addinglocalert").dialog({
                   autoOpen: false,
                   hide: "close",
                   draggable: true,
                   width: 500,
                   height: 150,
                   modal: true,
                   resizable: false,
                   position: {
                       my: "center",
                       at: "center",
                       of: window
                   }
               });
               $("#deletealert").dialog({
                   autoOpen: false,
                   hide: "close",
                   draggable: true,
                   width: 500,
                   height:200,
                   modal: true,
                   resizable: false,
                   position: {
                       my: "center",
                       at: "center",
                       of: window
                   }
               });
               $("#createbeamlocalert").dialog({
                   autoOpen: false,
                   hide: "close",
                   draggable: true,
                   width: 500,
                   height: 200,
                   modal: true,
                   resizable: false,
                   position: {
                       my: "center",
                       at: "center",
                       of: window
                   }
               });
               $("#warningalert").dialog({
                   autoOpen: false,
                   hide: "close",
                   draggable: true,
                   width: 500,
                   height: 150,
                   modal: true,
                   resizable: false,
                   position: {
                       my: "center",
                       at: "center",
                       of: window
                   }
               });
               
               var textfieldname = $("#txtSearch");
               DropdownFunction(textfieldname);
               
               textfieldname = $("#txtFixedmaterialcode");
               DropdownFunction(textfieldname);

               var textfieldname = $("#txtSelectTenant");
               DropdownFunction(textfieldname);

               var textfieldname = $("#txtSelectTenantAdd");
               DropdownFunction(textfieldname);

               var textfieldname = $("#txtBayTenant");
               DropdownFunction(textfieldname);

               var textfieldname = $("#txtModifyTenant");
               DropdownFunction(textfieldname);

               $('#txtSearch').bind('keydown', function (e) {
                   if (e.keyCode == 13)
                       document.getElementById("btnSearch").focus();
                       
                       //e.preventDefault();
               });
           });
           var beamcode = "";
           var bayarray;
           var aisle = 0;
           var aislefrom = 65, aisleto = 65, aisle2from = 65, aisle2to=65,bayfrom = 1, bayto = 1, beamfrom = 65, beamto = 65, locfrom = 1, locto = 1, beamlocfrom = 1, beamlocto = 1;
           var canvaswidth = 1200, canvasheight = 2000;
           var aisleposx = 10, aisleposy = 10, aisleposh = 0, ileposw = 20, bayheight = 10, binwidth = 15, binheight = 10, beamheight = 9, constwidth = 20;
           var phase1;
           var phase2;
           var selectedlocation;
           var locationsdataarray = new Array();
           var Zonecode = 0;
           var locdetailsarray = new Array();
           var selecteddataarray = new Array();
           var mtlocationarray = new Array();
           var TenantArray = new Array();
           var beam;
           var deldata;
           //Delete Selected Locations
           function deleteLocations()
           {
               debugger;   
               if (selecteddataarray.length == 0) {                  
                   showStickyToast(false, "Please select a bin to Delete", false);
               }

               var dellocations = "";
               var fulllocations = "";
               selecteddataarray.forEach(function (selectedlocation)
               {
                   var Data = selectedlocation.name.split('|');
                   var TenantList = Data.splice(0, 1).toString();

                   var LocationSplit = TenantList.split('!');

                   var DeleteSelLocation = LocationSplit.splice(0, 1).toString();

                   if (DeleteSelLocation.length >= 9) {
                       dellocations = dellocations + DeleteSelLocation.substr(0, 9) + ",";
                   } else {
                       fulllocations = fulllocations + DeleteSelLocation.substr(0, 9) + ",";

                   }
                  
                   //if (selectedlocation.name.length ==9)
                   //{
                   //    dellocations = dellocations + selectedlocation.name.substr(0, 7) + ",";
                   //} else
                   //{
                   //    fulllocations = fulllocations + selectedlocation.name.substr(0, 7) + ",";

                   //}
               });
               if (fulllocations != "")
               {
                   fulllocations = fulllocations.substr(0, fulllocations.length - 1);                  
                   showStickyToast(false, "Cannot delete these Locations " + fulllocations, false);
               }
               if (dellocations != "")
               {

                   dellocations = dellocations.substr(0, dellocations.length - 1);
                   deldata = dellocations;
                   document.getElementById("deletealertspan").innerHTML = "<b>The following location will be deleted permanently<br>" + dellocations;
                   $("#deletealert").dialog("option", "title", "Delete Location(s)");
                   $("#deletealert").dialog('open');
               }
           }

           function PrintLocations()
           {
               

               var printLocations = "";
               selecteddataarray.forEach(function (selectedlocation) {

                   printLocations = printLocations + selectedlocation.name.substr(0, 9) + ",";
                   
               });
               if (printLocations == "") {                  
                   showStickyToast(false, "Please select location you want to print", false);
                   return;
               }
             
               var printerIP = $('#<%=ddlPrinter.ClientID %>').val();
              
               if (printerIP == 0 || printerIP==null) {                  
                   showStickyToast(false, "Please select Printer", false);
                   return;
               }
               $.ajax({
                   url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/Print_Location") %>',
                   data: "{  'location': '" + printLocations + "','printerIP':'"+printerIP+"'}",
                  dataType: "json",
                  type: "POST",
                  contentType: "application/json; charset=utf-8",
                  success: function (data)
                  {
                      
                      
                            },
                  error: function (response) {                     
                      showStickyToast(false, response.text, false);
                      
                  },
                  failure: function (response) {
                      showStickyToast(false, response.text, false);
                      
                  }
              });
           }
           function doDelete() {
             
               $.ajax({
                   type: "POST",
                   url: "LocationHandlers/DeleteLocationHandler.ashx?dellocations=" + deldata,
                   contentType: "text/html",
                   success: function (response) {
                       if (response == "success") {
                           GetLocations();
                           //getLocations();
                           CloseDialog(deletealert);
                       } else {
                           showStickyToast(false, response, false);
                       }
                   },
                   error: function (response) {
                       showStickyToast(false, "Error", false);
                   }
               });
           }

           //Update Selected Locations
          
           function GetUpdateDialog() {
               debugger;
               document.getElementById("UpdateError").innerHTML = "";
               if (selecteddataarray.length == 0) {                   
                   showStickyToast(false, "Please select a bin to update", false);
                   return false;
               }
               else if (selecteddataarray.length > 1) {                  
                   showStickyToast(false, "Please select only one bin to update", false);
                   return false;

               }

               else if (selecteddataarray.length == 1) {
                   selectedlocation = selecteddataarray[0].name.substr(0, 9);
                   $.ajax({
                       type: "GET",
                       url: "LocationHandlers/GetLocationDetailsHandler.ashx?location=" + selectedlocation,
                       contentType: "application/json; charset=utf-8",
                       success: function (jsonList) {
                           prepareUpdateDialog(jsonList);
                       },
                       error: function (e) {
                           showStickyToast(false, "Error please contact 'Inventrax Admin' ", false);                          
                       }
                   });

               } else {
                   //alert("Multiple locations cannot be updated");
                   $('#txtModifySupplier').empty();
                   txtwidth.value = "";
                   txtHeight.value = "";
                   txtLength.value = "";
                   txtMaxweight.value = "";
                   selectIsMMA.value = 1;
                   txtFixedmaterialcode.value = "";
                   selectIsActive.value = 1;
                   selectIsQuarantine.value = 0;
                   txtModifyTenant.value = "";
                   //txtModifySupplier.value = "";
                   $("#<%=hifTenant.ClientID%>").val("");
                   $("#<%=hifSupplierID.ClientID%>").val("");
                   divCbm.innerText = "0";
                   selectIsFMA.value = 0;
                   document.getElementById("updatebindialog").style.visibility = "visible";
                   $("#updatebindialog").dialog('option', 'title', 'Modify Location(s)');
                   $("#updatebindialog").dialog('option', 'position', [300, 40]);
                   $("#updatebindialog").dialog('open');

                   if (document.getElementById("selectIsMMA").value == "0") {
                       document.getElementById("trFixedMaterial").style.display = "table-row";
                   }
                   else {
                       document.getElementById("trFixedMaterial").style.display = "none";
                   }
                   document.getElementById("trModifySupplier").style.display = "none";
               }
           }
        function prepareUpdateDialog(jsonList) {
            $('#txtModifySupplier').empty();
               txtwidth.value = jsonList[0].width;
               txtHeight.value = jsonList[1].height;
               txtLength.value = jsonList[2].length;
               txtMaxweight.value = jsonList[3].maxweight;
              
               var IsMMok = jsonList[4].isMMok;
               var FixedMCode = jsonList[5].fixedMcode;
               var IsActive = jsonList[6].isactive;
               var IsQuarantine = jsonList[7].isQuarantine;
               var Tenant = jsonList[8].Tenant;
               var Supplier = jsonList[9].Supplier;
               var TenantID = jsonList[10].TenantID;
               var SupplierID = jsonList[11].SupplierID;
               var CBM = jsonList[13].cbm;
               var IsFastMoving = jsonList[14].isFastMoving;

               selectIsMMA.value = IsMMok;
               selectIsActive.value = IsActive;
               selectIsQuarantine.value = IsQuarantine;
               txtModifyTenant.value = Tenant;
               $("#<%=hifTenant.ClientID%>").val(TenantID);
               divCbm.innerText = CBM;
               txtFixedmaterialcode.value = FixedMCode;
               selectIsFMA.value = IsFastMoving;
               
                $.ajax({
                    url: "LocationHandlers/GetMMCodehandler.ashx?prefix=0&choice=SupplierData" + "&Tenant=" + jsonList[8].Tenant,
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $.map(data, function (item) {
                            if (jsonList[8].Tenant != "") {
                                $('#txtModifySupplier').append(
                                        $('<option></option>').html(item.split(',')[0]) // Append all Suppliers
                                    );
                            }
                        });

                        var TotalSuppliers = document.getElementById('txtModifySupplier');
                        var SelectedSupplier = Supplier.split(',');
                        var Count = 0;
                        for (var i = 0; i < SelectedSupplier.length; i++) {
                            for (var j = 0; j < TotalSuppliers.options.length; j++) {
                                if (SelectedSupplier[i].trim() == TotalSuppliers.options[j].value.trim()) {
                                    TotalSuppliers.options[j].selected = true;
                                }
                            }
                        }
                        
                    },
                    error: function (request, error) {                       
                        showStickyToast(false, "Request: " + JSON.stringify(request), false);    
                    }
                });

               document.getElementById("updatebindialog").style.visibility = "visible";
               $("#updatebindialog").dialog('option', 'title', selectedlocation);
               $("#updatebindialog").dialog('option', 'position', [300, 40]);
               $("#updatebindialog").dialog('open');

               if (document.getElementById("selectIsMMA").value == "0") {
                   document.getElementById("trFixedMaterial").style.display = "table-row";
               }
               else {
                   document.getElementById("trFixedMaterial").style.display = "none";
               }
           }
           function UpdateLocationDetails()
           {
               debugger;
               var mmcode = "";
               if (selectIsMMA.value == "0" && document.getElementById("txtFixedmaterialcode").value.trim() == "")
               {
                  
                   document.getElementById("UpdateError").innerHTML = "<b>'Material Code' cannot be blank when the bin is configured to a fixed  material<br>";
                   //document.getElementById("warningalertspan").innerHTML = "<b>Material Code' cannot be blank when the bin is configured to a fixed  material<br>";
                   //$("#warningalert").dialog("option", "title", "Warning");
                   //document.getElementById("warningalert").style.background = "red";
                   //$("#warningalert").dialog('open');
                }
               if (selectIsMMA.value == "0" && document.getElementById("txtFixedmaterialcode").value.trim() != "")
               {
                   $.ajax({
                       type: "POST",
                       url: "LocationHandlers/CheckMMcodehandler.ashx?mmcode=" + document.getElementById("txtFixedmaterialcode").value,
                       contentType: "text/html",
                       success: function (response) {
                           mmcode = response;
                           if (mmcode == "0") {
                               document.getElementById("warningalertspan").innerHTML = "<b>Please provide valid material code";
                               $("#warningalert").dialog("option", "title", "Warning");
                               document.getElementById("warningalert").style.background = "red";
                               $("#warningalert").dialog('open');
                           } else if (mmcode != "0") {
                               UpdateLocationData(document.getElementById("txtFixedmaterialcode").value);
                               CloseDialog(updatebindialog);
                           }
                       },
                       error: function (response) {                          
                           showStickyToast(false, "Error", false);   
                           return false;
                       }
                   });
               }
               if (document.getElementById("selectIsMMA").value == "1") {
                   mmcode = "null";
                   if (UpdateLocationData(mmcode) == false) {
                   }
                   else {
                       CloseDialog(updatebindialog);
                   }
                   
               }
           }
           function UpdateLocationData(mmcode)
           {
               debugger;
               if (document.getElementById("txtwidth").value == "" || document.getElementById("txtHeight").value == "" || document.getElementById("txtLength").value == "" ) {
                   
                   showStickyToast(false, "Please enter the details", false);   
                   return false;
               }
               var SelectedSuppliers = [];
               $('#txtModifySupplier :selected').each(function (i, selected) {
                   SelectedSuppliers[i] = $(selected).text();
               });

               if (document.getElementById('txtModifyTenant').value == "" && SelectedSuppliers != "") {                 
                   showStickyToast(false, "Please select the Tenant", false);   
                   return false;
               }

               selectedlocation = "";

               for (var index = 0; index < selecteddataarray.length; index++)
               {
                   selectedlocation += selecteddataarray[index].name.substr(0, 9) + ",";
               }

               selectedlocation = selectedlocation.substring(0, selectedlocation.length - 1);

               if (SelectedSuppliers != "" && document.getElementById('txtModifyTenant').value == "") {
                   showStickyToast(false, "Please select the Tenant", false);   
                   return false;
               }
               if (document.getElementById("txtModifyTenant").value == "") {
                   SelectedSuppliers = "";
               }

               $.ajax({
                   type: "GET",
                   url: "LocationHandlers/UpdateLocDataHandler.ashx?width=" + document.getElementById("txtwidth").value + "&height=" + document.getElementById("txtHeight").value + "&length=" + document.getElementById("txtLength").value + "&maxweight=" + (document.getElementById("txtMaxweight").value == "" ? null : document.getElementById("txtMaxweight").value) + "&ismmok=" + document.getElementById("selectIsMMA").value + "&isFM=" + document.getElementById("selectIsFMA").value + "&isactive=" + document.getElementById("selectIsActive").value + "&MCode=" + mmcode + "&locid=" + selectedlocation + "&IsQuarantine=" + document.getElementById("selectIsQuarantine").value + "&Tenant=" + document.getElementById("txtModifyTenant").value + "&Supplier=" + SelectedSuppliers,
                   //+ "&TenantID=" + document.getElementById('<%=hifTenant.ClientID%>').value + "&SupplierID=" + document.getElementById('<%=hifSupplierID.ClientID%>').value
                   contentType: "text/html",
                   success: function (response) {
                       GetLocations();
                       //getLocations();
                   },
                   error: function (response) {                      
                       showStickyToast(false, "Sorry Some error occured", false);   
                   }
               });
           }

           //functionn for inserting locations into database
          
        function createLocations() {
              

            var _iAisle = $('#txtAisle').val();
            var _iRack = $('#txtRack').val();
            var _iCol = $('#txtColumn').val();
            var _iLev = $('#txtLevel').val();
            var _iBin = $('#txtBin').val();

            var _iWidth = $('#txtWidthB').val();
            var _iLength = $('#txtLengthB').val();
            var _iHeight = $('#txtHeightB').val();
            var _iMaxWeight = $('#txtMaxWeightB').val();

            var SelectedSuppliers = [];
            $('#txtSupplier :selected').each(function (i, selected) {
                SelectedSuppliers[i] = $(selected).text();
            });

            if (document.getElementById('txtSelectTenant').value == "" && SelectedSuppliers != "") {
                showStickyToast(false, "Please select the Tenant", false);  
                return false;
            }
            if (document.getElementById("txtSelectTenant").value == "") {
                SelectedSuppliers = "";
            }

            var boolIsFastMove;
            if ($('#cbxfastmove').prop('checked')) {
                boolIsFastMove = 1;
            }
            else {
                boolIsFastMove = 0;
            }

               $.ajax({
                   type: "POST",
                   //url: "LocationHandlers/InsertLocationHandler.ashx?bayfrom=" + bayfrom + "&bayto=" + bayto + "&beamfrom=" + beamfrom + "&beamto=" + beamto + "&locfrom=" + locfrom + "&locto=" + locto + "&zone=" + Zonecode + "&Tenant=" + document.getElementById("txtSelectTenant").value + "&Supplier=" + SelectedSuppliers + "&IsFastMoving=" + boolIsFastMove + "&aisle=" + aisle,
                   url: "LocationHandlers/InsertLocationHandler.ashx?aisle=" + _iAisle + "&rack=" + _iRack + "&column=" + _iCol + "&level=" + _iLev + "&bin=" + _iBin + "&zone=" + Zonecode + "&Tenant=" + document.getElementById("txtSelectTenant").value + "&Supplier=" + SelectedSuppliers + "&IsFastMoving=" + boolIsFastMove + "&length=" + _iLength + "&width=" + _iWidth + "&height=" + _iHeight + "&maxWeight=" + _iMaxWeight,
                   contentType: "text/html",
                   success: function (response) {
                       if (response != "1") {                          
                           showStickyToast(false, "Location(s) already exists", false);  
                           CloseDialog(alertdialog);
                           return false;
                       } else {
                           showStickyToast(true, "Location(s) Created Successfully", false);  
                           CloseDialog(printlocationdialog);
                           CloseDialog(createlocalert);
                          // getLocations();
                           GetLocations();
                       }

                   },
                   error: function (response) {
                       showStickyToast(false, response, false);  
                   }
               });
           }
        function InsertLocations() {
            
            var _iAisle = $('#txtAisle').val();
            var _iRack = $('#txtRack').val();
            var _iCol = $('#txtColumn').val();
            var _iLev = $('#txtLevel').val();
            var _iBin = $('#txtBin').val();

            var _iWidth = $('#txtWidthB').val();
            var _iLength = $('#txtLengthB').val();
            var _iHeight = $('#txtHeightB').val();
            var _iMaxWeight = $('#txtMaxWeightB').val();

            if (_iAisle == "" || _iRack == "" || _iCol == "" || _iLev == "" || _iBin == "")
            {               
                showStickyToast(false, "Please enter Mandatory fields", false);  
                return false;
            }

               var SelectedSuppliers = [];
               $('#txtSupplier :selected').each(function (i, selected) {
                SelectedSuppliers[i] = $(selected).text();
               });

               if (document.getElementById('txtSelectTenant').value == "" && SelectedSuppliers != "") {
                   showStickyToast(false, "Please select the Tenant", false);  
               return false;
               }

              
               var numberofbays = bayto - bayfrom + 1;
              
               var numberofbeams = beamto - beamfrom + 1;
              
               var numberoflocations = locto - locfrom + 1;
               var totallocations;
               // totallocations = numberofbays * numberofbeams * numberoflocations;
               totallocations = _iAisle * _iRack * _iLev * _iCol * _iBin;
                    
                   document.getElementById("createlocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";
                   $("#createlocalert").dialog("option", "title", "Confirmation");
                   $("#createlocalert").dialog("open");
        }

        //Printing Alert
        function InitatePrint() {


           
            totallocations = "Rack "+$('#ddlRackPrint option:selected').val();
           
            document.getElementById("printlocalertspan").innerHTML = " Do you want to Print <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";
            $("#printlocaalert").dialog("option", "title", "Confirmation");
            $("#printlocaalert").dialog("open");
            $("#printlocaalert").dialog({
                buttons: [
                    {
                        text: "OK",
                        "click": function () {
                            PrintBulkLocations();
                        }
                    },
                    {
                        text: "Cancel",
                        "click": function () {
                            ClosePrintDialog("printlocaalert");
                        }
                    }
                ],
                focus: function (i, j) {
                    $('#printlocaalert').siblings('.ui-dialog-buttonpane').find('button.ui-widget').css('position', "initial");
                },
                create: function (i, j) {
                    $('#printlocaalert').siblings('.ui-dialog-buttonpane').find('button.ui-widget').css('position', "initial");
                },
                close: function (i, j) {
                    $("#printlocaalert").dialog("destroy");
                }
            });
           
        }

        ///Adding Locations


        function AddingLocations() {
            


            var _iRack = $('.ddlRack option:selected').val();
            var _iCol = $('#txtColumnAdd').val();
            var _iLev = $('#txtLevelAdd').val();
            var _iBin = $('#txtBinAdd').val();

            var _iWidth = $('#txtWidthAdd').val();
            var _iLength = $('#txtLengthAdd').val();
            var _iHeight = $('#txtHeightAdd').val();
            var _iMaxWeight = $('#txtMaxWeightAdd').val();

            var SelectedSuppliers = [];
            $('#txtSupplierAdd :selected').each(function (i, selected) {
                SelectedSuppliers[i] = $(selected).text();
            });

            if (document.getElementById('txtSelectTenantAdd').value == "" && SelectedSuppliers != "") {
                showStickyToast(false, "Please select the Tenant", false);  
                return false;
            }
            if (document.getElementById("txtSelectTenantAdd").value == "") {
                SelectedSuppliers = "";
            }

            var boolIsFastMove;
            if ($('#cbxfastmoveAdd').prop('checked')) {
                boolIsFastMove = 1;
            }
            else {
                boolIsFastMove = 0;
            }

            $.ajax({
                type: "POST",

                //url: "LocationHandlers/AddingLocationHandler.ashx?rack=" + _iRack + "&column=" + _iCol + "&level=" + _iLev + "&bin=" + _iBin + "&zone=" + Zonecode + "&Tenant=" + document.getElementById("txtSelectTenantAdd").value + "&Supplier=" + SelectedSuppliers + "&IsFastMoving=" + boolIsFastMove + "&length=" + _iLength + "&width=" + _iWidth + "&height=" + _iHeight + "&maxWeight=" + _iMaxWeight,
                url: "LocationTree.aspx/AddingLocations",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{'rack': '" + _iRack + "' , 'column':  '" + _iCol + "', 'level': '" + _iLev + "', 'bin':  '" + _iBin + "', 'zone': '" + Zonecode + "', 'Tenant':  '" + document.getElementById("txtSelectTenantAdd").value + "', 'Supplier': '" + SelectedSuppliers + "' ,'IsFastMoving':  " + boolIsFastMove + ", 'length': " + _iLength + " ,'width':  " + _iWidth + ", 'height': " + _iHeight + " ,'maxWeight':  " + _iMaxWeight + "}",
                success: function (response) {
                   
                    if (response.d != "1") {                       
                        showStickyToast(false, "Location(s) already exists", false);  
                        CloseDialog(alertdialog);
                        return false;
                    } else {
                        CloseDialog(printlocationdialog);
                        CloseDialog(Addinglocalert);
                        // getLocations();
                        GetLocations();
                        showStickyToast(true, "Locations Added Successfully", false); 
                    }

                },
                error: function (response) {
                    showStickyToast(false, "Error", false);  
                }
            });
        }

        function AddingInsertLocations() {
            
            debugger;
            var _iRack = $('.ddlRack option:selected').val();
            var _iCol = $('#txtColumnAdd').val();
            var _iLev = $('#txtLevelAdd').val();
            var _iBin = $('#txtBinAdd').val();

            var _iWidth = $('#txtWidthAdd').val();
            var _iLength = $('#txtLengthAdd').val();
            var _iHeight = $('#txtHeightAdd').val();
            var _iMaxWeight = $('#txtMaxWeightAdd').val();

            var _iColorLev = $('#ddlColumnOrLevel').val();



            if (_iRack == "" || _iRack == 0) {
                showStickyToast(false, "Please select Rack", false);
                return false;
            }

            if (_iColorLev == 1)
            {
                if (_iCol == "" || _iCol == 0) {
                    showStickyToast(false, "Please enter No.of Columns Required", false);
                    return false;
                }
                if (_iLev == "") {
                    _iLev = 0;
                }
            }
            if (_iColorLev == 2) {
                if (_iLev == "" || _iLev == 0) {
                    showStickyToast(false, "Please enter No.of Levels Required", false);
                    return false;
                }
                if (_iCol=='') {
                    _iCol = 0;
                }
            }


            if (_iBin == "" || _iBin == 0) {               
                showStickyToast(false, "Please enter No.of Locations Required in each Bin", false); 
                return false;
            }

            var SelectedSuppliers = [];
            $('#txtSupplierAdd :selected').each(function (i, selected) {
                SelectedSuppliers[i] = $(selected).text();
            });

            if (document.getElementById('txtSelectTenantAdd').value == "" && SelectedSuppliers != "") {
                showStickyToast(false, "Please select Tenant", false); 
                return false;
            }


            
            var totallocations = _iLev * _iCol * _iBin;
            if (_iCol == 0)
                totallocations = _iLev + " Levels";
            else
                totallocations = _iCol + " Columns";

            document.getElementById("Addinglocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";
            $("#Addinglocalert").dialog("option", "title", "Confirmation");
            $("#Addinglocalert").dialog("open");
        }


        ///END Adding Locations






        //For Printing Bulk locations
        function PrintBulkLocations() {

            BlockDialog();
            var Rack = $('#ddlRackPrint').val();
            var Column = $('#ddlColumnPrint').val();
            var Level = $('#ddlLevelPrint').val();
            var Bin = $('#ddlBinPrint').val();
            var printerIP = $('#<%=ddlPrinter.ClientID %>').val();

            $.ajax({
                type: "POST",
                url: "LocationHandlers/PrintLocationHandler.ashx?Zone=" + Zonecode + "&Rack=" + Rack + "&Column=" + Column + "&Level=" + Level + "&Bin=" + Bin + "&PrinterIP=" + printerIP + "&aisle=" + aisle,
                contentType: "text/html",
                success: function (response) {
                    if (response == "1") {
                        showStickyToast(true, "Successfully Printed");
                        //CloseDialog(alertdialog);
                        ClosePrintDialog("printlocaalert");
                        UnBlockDialog();
                        return false;
                    }
                },
                error: function (response) {
                    showStickyToast(false, "Error", false); 
                    UnBlockDialog();
                }
            });
        }















        function InsertBeamLocations() {

            var SelectedSuppliers = [];
            $('#txtBaySupplier :selected').each(function (i, selected) {
                SelectedSuppliers[i] = $(selected).text();
            });

            if (document.getElementById('txtBayTenant').value == "" && SelectedSuppliers != "") {
                alert('Please select the Tenant');
                return false;
            }
            beam = beamcode;
            var totallocations = _iAisle * _iRack * _iLev * _iRack;// beamlocto - beamlocfrom + 1;
            document.getElementById("createbeamlocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s) in Beam " + beam + "?<br> ";
            $("#createbeamlocalert").dialog("option", "title", "Confirmation");
            /*$("#alertdialog").dialog({
                buttons: [
                    {
                    text: "Cancel", click: function () {
                        CloseDialog(alertdialog);

                    }
                }, {
                    text: "Ok", click: function () {
                        $.ajax({
                            type: "POST",
                            url: "LocationHandlers/InsertLocationHandler.ashx?beamcode=" + beam + "&beamlocfrom=" + beamlocfrom + "&beamlocto=" + beamlocto,
                            contentType: "text/html",
                            success: function (response) {
                                if (response != "1") {
                                    alert("Locations Already Existed");
                                    CloseDialog(alertdialog);
                                    return false;
                                } else {
                                    CloseDialog(beamdialog);
                                    getLocations();
                                    CloseDialog(alertdialog);
                                }

                            },
                            error: function (response) {
                                alert('error');
                            }
                        });
                    }
                }]
            });*/
            $("#createbeamlocalert").dialog("open");
        }
        function CreateBeamLocations() {
            var SelectedSuppliers = [];
            $('#txtBaySupplier :selected').each(function (i, selected) {
                SelectedSuppliers[i] = $(selected).text();
            });

            if (document.getElementById('txtBayTenant').value == "" && SelectedSuppliers != "") {
                alert('Please select the Tenant');
                return false;
            }

            if (document.getElementById("txtBayTenant").value == "") {
                SelectedSuppliers = "";
            }

            $.ajax({
                type: "POST",
                url: "LocationHandlers/InsertLocationHandler.ashx?beamcode=" + beam + "&beamlocfrom=" + beamlocfrom + "&beamlocto=" + beamlocto + "&Tenant=" + document.getElementById("txtBayTenant").value + "&Supplier=" + SelectedSuppliers + "&aisle=" + aisle,
                contentType: "text/html",
                success: function (response) {
                    if (response == "0") {
                        //alert("Locations already exists");
                        alert("Error while Inserting the Locations");
                        CloseDialog(alertdialog);
                        return false;
                    } else {
                        CloseDialog(beamdialog);
                        //getLocations();
                        GetLocations();
                        CloseDialog(createbeamlocalert);
                    }

                },
                error: function (response) {
                    alert('error');
                }
            });
        }
        function getSliderDialog() {

            $('#txtAisle, #txtRack, #txtColumn, #txtLevel, #txtBin, #txtSelectTenant, #txtSupplier').val('');
            $('#txtWidthB, #txtLengthB, #txtHeightB, #txtMaxWeightB').val(0);

            document.getElementById("txtSelectTenant").value = "";
            $('#cbxfastmove').prop('checked', false);
            //document.getElementById("txtSupplier").value = "";
            $('#txtSupplier').empty();

            Zonecode = $("#<%=ddlLocationCode.ClientID%>").attr("value");
            if (Zonecode == 0) {
                alert("Please select 'Zone' ");
                return false;
            }
            if (selecteddataarray.length == 0) {
                // $("#locationcreatedialog").dialog('option', 'position', [300, 40]);
                document.getElementById("locationcreatedialog").style.visibility = "visible";
                $("#locationcreatedialog").dialog('open');
                //$("#locationcreatedialog").css("background-color", "lightblue");
                $("#locationcreatedialog").dialog('option', 'title', "Create New Locations");
               // document.getElementById("trSupplier").style.display = "none";
            } else {
                alert("Deselect the location(s) first");
            }

        }

           function GetAddingDialog()
           {
               
               document.getElementById("txtSelectTenantAdd").value = "";
               $('#cbxfastmoveAdd').prop('checked', false);
               //document.getElementById("txtSupplier").value = "";
               $('#txtSupplierAdd').empty();

               Zonecode = $("#<%=ddlLocationCode.ClientID%>").attr("value");
               if (Zonecode == 0) {                 
                   showStickyToast(false, "Please select 'Zone'", false); 
                   return false;
               }

               $('#txtColumnAdd').val('');
               $('#txtLevelAdd').val('');
               $('#txtBinAdd').val('');
               $('#txtWidthAdd').val('0');
               $('#txtLengthAdd').val('0');
               $('#txtHeightAdd').val('0');
               $('#txtSelectTenantAdd, #txtSupplierAdd').val('');
              


               if (selecteddataarray.length == 0) {
                   LoadRacks(Zonecode);
                   // $("#locationcreatedialog").dialog('option', 'position', [300, 40]);
                   document.getElementById("Addinglocationcreatedialog").style.visibility = "visible";
                   $("#Addinglocationcreatedialog").dialog('open');
                   //$("#Addinglocationcreatedialog").css("background-color", "lightblue");
                   $("#Addinglocationcreatedialog").dialog('option', 'title', "Adding New Locations(Levels or Columns)");
                  // document.getElementById("trSupplierAdd").style.display = "none";
               } else {                 
                   showStickyToast(false, "Deselect the location(s) first", false); 
               }
           }
           function LoadRacks(ZoneCode)
           {
               $.ajax({
                   url: "LocationTree.aspx/GetRacks",
                   dataType: "json",
                   type: "POST",
                   data: "{'zoneCode':'" + ZoneCode + "'}",
                   async: true,
                   contentType: "application/json; charset=utf-8",
                   success: function (data) {
                       $('.ddlRack').empty();
                       var obj = JSON.parse(data.d);
                       for (var i = 0; i < obj.length; i++)
                       {
                           $('.ddlRack').append("<option value=" + obj[i].Rack + ">" + obj[i].Rack +"</option>");                          
                       }

                   },
                   error: function (response) {                      
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);                       
                   },
                   failure: function (response) {
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                   }

               });
           }
        //For BulkPrint Dialog

           function getSliderDialogForBulkPrint() {
               
            document.getElementById("txtSelectTenant").value = "";
            //document.getElementById("txtSupplier").value = "";
            $('#txtSupplier').empty();



            Zonecode = $("#<%=ddlLocationCode.ClientID%>").attr("value");
               if (Zonecode == 0) {                  
                   showStickyToast(false, "Please select 'Zone' ", false);
                   return false;
               }

               var printerIPforbulkprint = $('#<%=ddlPrinter.ClientID %>').val();

               if (printerIPforbulkprint == 0 || printerIPforbulkprint == null) {
                   showStickyToast(false, "Please select 'Printer' ", false);
                   return;
               }
               if (selecteddataarray.length == 0) {
                    LoadRacksForPrint(Zonecode);
                   // $("#locationcreatedialog").dialog('option', 'position', [300, 40]);
                   document.getElementById("printlocationdialog").style.visibility = "visible";
                   $("#printlocationdialog").dialog('open');
                   //$("#locationcreatedialog").css("background-color", "lightblue");
                   $("#printlocationdialog").dialog('option', 'title', "Print New Labels");
                  // document.getElementById("trSupplier").style.display = "none";
               } else {                  
                   showStickyToast(false, "Deselect the location(s) first", false);
               }

           }

           function ddlRackPrint_Change() {
               $('#ddlLevelPrint,#ddlColumnPrint,#ddlBinPrint').empty();
               $('#ddlLevelPrint,#ddlColumnPrint,#ddlBinPrint').append("<option value='0'>All</option>");

               
               var ZoneCode = $('#MainContent_MMContent_ddlLocationCode').val();
               var RackCode = $('#ddlRackPrint').val();





               $.ajax({
                   url: "LocationTree.aspx/GetColumnAndLevel",
                   dataType: "json",
                   type: "POST",
                   data: "{'zoneCode':'" + ZoneCode + "','rackCode':'" + RackCode + "'}",
                   async: true,
                   contentType: "application/json; charset=utf-8",
                   success: function (data) {
                       var obj = JSON.parse(data.d);                    
                       for (var i = 0; i < obj.Table.length; i++) {
                           $('#ddlColumnPrint').append("<option value=" + obj.Table[i].Column + ">" + obj.Table[i].Column + "</option>");
                       }


                       for (var i = 0; i < obj.Table1.length; i++) {
                           $('#ddlLevelPrint').append("<option value=" + obj.Table1[i].Level + ">" + obj.Table1[i].Level + "</option>");
                       }

                   },
                   error: function (response) {                      
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText , false);
                   },
                   failure: function (response) {
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                   }



               });
           }

           function ddlColLevPrint_Change() {
               
               var ZoneCode = $('#MainContent_MMContent_ddlLocationCode').val();
               var RackCode = $('#ddlRackPrint').val();
               var ColCode = $('#ddlColumnPrint').val();
               var LevCode = $('#ddlLevelPrint').val();





               $.ajax({
                   url: "LocationTree.aspx/GetBins",
                   dataType: "json",
                   type: "POST",
                   data: "{'zoneCode':'" + ZoneCode + "','rackCode':'" + RackCode + "','ColCode':'" + ColCode + "','LevCode':'" + LevCode + "'}",
                   async: false,
                   contentType: "application/json; charset=utf-8",
                   success: function (data) {
                       var obj = JSON.parse(data.d);
                       $('#ddlBinPrint').empty();
                       $('#ddlBinPrint').append("<option value='0'>All</option>");
                       for (var i = 0; i < obj.length; i++) {
                           $('#ddlBinPrint').append("<option value=" + obj[i].Bin + ">" + obj[i].Bin + "</option>");
                       }
                   },
                   error: function (response) {
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                   },
                   failure: function (response) {
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                   }
               });
           }
           function LoadRacksForPrint(ZoneCode) {
               $.ajax({
                   url: "LocationTree.aspx/GetRacks",
                   dataType: "json",
                   type: "POST",
                   data: "{'zoneCode':'" + ZoneCode + "'}",
                   async: true,
                   contentType: "application/json; charset=utf-8",
                   success: function (data) {
                       $('.ddlRackPrint').empty();
                       var obj = JSON.parse(data.d);
                       for (var i = 0; i < obj.length; i++) {
                           $('#ddlRackPrint').append("<option value=" + obj[i].Rack + ">" + obj[i].Rack + "</option>");
                       }

                   },
                   error: function (response) {
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                   },
                   failure: function (response) {
                       showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                   }

               });
           }
                  
                         
            

        //autocompleting material data in searchbox
        function getMaterialAutoComplete(vTenantID) {
              
               try
               {

            $('.mcodepicker').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=mcode" + "&Tenant=" + vTenantID,
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                           label: item.split('~')[0].split('`')[0],
                                           description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                           val: item.split('~')[1]
                                }
                            }))

                        },
                        error: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        },
                        failure: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        }

                    });
                },
                select: function (e, i) {
                    document.getElementById("hidTenantID").value = i.item.val;
                },
                minLength: 0,
 
                   }).data("autocomplete")._renderItem = function (ul, item) {
                       // Inside of _renderItem you can use any property that exists on each item that we built
                       // with $.map above */
                       return $("<li></li>")
                           .data("item.autocomplete", item)
                           .append("<a>" + item.label + "" +(item.description==null?'': item.description )  + "</a>")
                           .appendTo(ul)
                   };
               } catch (ex) { }

           }
           //auto completing deptdata in searchbox
        function getTenantAutoComplete(vTenantID) {

            $('.mcodepicker').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=" + vTenantID,
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))

                        },
                        error: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        },
                        failure: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        }

                    });
                },
                select: function (e, i) {
                    document.getElementById("hidTenantID").value = i.item.val;

                },
                minLength: 0
            });
        }

        function getTenantForMultipleSupplier(value) {
            debugger;
            $('.TenantListboxPicker').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=0",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]

                                }
                            }))

                        },
                        error: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        },
                        failure: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        }

                    });
                },
                select: function (e, i) {
                    $("#<%=hifTenant.ClientID%>").val(i.item.val);
                    $("#<%=hifTenantAdd.ClientID%>").val(i.item.val);

                    if (value == 'Create') {
                        $("#txtSelectTenant").val(i.item.label);
                        document.getElementById("trSupplier").style.display = "table-row";
                    }
                    else if (value == '+') {
                        $("#txtSelectTenantAdd").val(i.item.label);
                        document.getElementById("trSupplierAdd").style.display = "table-row";
                    }

                    else if (value == 'Modify') {
                        $("#txtModifyTenant").val(i.item.label);
                        //document.getElementById("trModifySupplier").style.display = "table-row";
                    }
                    else if (value == 'Bay') {
                        $("#txtBayTenant").val(i.item.label);
                        document.getElementById("trBaySupplier").style.display = "table-row";
                    }
                    $('#txtModifySupplier').empty();
                    $('#txtSupplier').empty();
                    $('#txtBaySupplier').empty();

                    getListboxList(value);
                },
                minLength: 0
            });
        }

        function getListboxList(value) {

            $('#txtModifySupplier').empty();
            $('#txtSupplier').empty();
            $('#txtBaySupplier').empty();

            if (value == 'Modify') {
                Tenant = document.getElementById('txtModifyTenant').value;
            }

            else if (value == 'Create') {
                Tenant = document.getElementById('txtSelectTenant').value;
            }

            else if (value == '+') {
                Tenant = document.getElementById('txtSelectTenantAdd').value;
            }

            else if (value == 'Bay') {
                Tenant = document.getElementById('txtBayTenant').value;
            }

            $.ajax({
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=0&choice=SupplierData" + "&Tenant=" + Tenant,
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $.map(data, function (item) {
                        if (value == 'Modify') {
                            $('#txtModifySupplier').append(
                                $('<option></option>').html(item.split(',')[0])
                            );
                        }
                        else if (value == 'Create') {
                            $('#txtSupplier').append(
                                $('<option></option>').html(item.split(',')[0])
                            );
                        }
                        else if (value == '+') {
                            $('#txtSupplierAdd').append(
                                $('<option></option>').html(item.split(',')[0])
                            );
                        }
                        else if (value == 'Bay') {
                            $('#txtBaySupplier').append(
                                $('<option></option>').html(item.split(',')[0])
                            );
                        }

                    });
                },
                error: function (request, error) {                   
                    showStickyToast(false, "Request: " + JSON.stringify(request), false);
                }
            });
        }
        function getTenantList() {

            $("#<%=hifTenant.ClientID%>").val("");

            $('.MMCodepicker').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=0",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))

                        },
                        error: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        },
                        failure: function (response) {
                            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                        }

                    });
                },
                select: function (e, i) {
                    $("#<%=hifTenant.ClientID%>").val(i.item.val);
                    $("#txtSupplier").val("");
                    $("#txtModifySupplier").val("");
                    $("#txtBaySupplier").val("");

                },
                minLength: 0
            });
        }


        function getSupplierList(Val) {

            $("#<%=hifSupplierID.ClientID%>").val("");

            $('.SupplierPicker').autocomplete({
                source: function (request, response) {
                    
                    var Tenant = "";
                    if (Val == 1) {
                        if (document.getElementById('txtSelectTenant').value == "" ) {
                            showStickyToast(false, "Please select Tenant ", false);
                            return false;
                        }
                        Tenant = document.getElementById('txtSelectTenant').value;
                    }

                    else if (Val == 2) {
                        if (document.getElementById('txtModifyTenant').value == "") {
                            showStickyToast(false, "Please select Tenant ", false);
                            return false;
                        }
                        Tenant = document.getElementById('txtModifyTenant').value;
                    }

                    else if (Val == 3) {
                        if (document.getElementById('txtBayTenant').value == "") {
                            showStickyToast(false, "Please select Tenant ", false);
                            return false;
                        }
                        Tenant = document.getElementById('txtBayTenant').value;
                    }

                    $.ajax({
                        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=SupplierData" + "&Tenant=" + Tenant.toString(),
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                           
                                response($.map(data, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            

                        },
                        error: function (response) {
                            alert("Error, Please contact 'Inventrax Admin'");
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert("Error, Please contact 'Inventrax Admin'");
                            alert(response.responseText);
                        }

                    });
                },
                select: function (e, i) {
                    $("#<%=hifSupplierID.ClientID%>").val(i.item.val);

                   },
                   minLength: 0
               });
               }

        function getMMCodeList() {

            try {
                $("#txtFixedmaterialcode").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=mcode" + "&Tenant=0",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data, function (item) {
                                    return {
                                        label: item.split('~')[0].split('`')[0],
                                        description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                        val: item.split('~')[1]
                                    }
                                }))

                            },
                            error: function (response) {
                                alert("Error, Please contact 'Inventrax Admin' ");
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert("Error, Please contact 'Inventrax Admin' ");
                                alert(response.responseText);
                            }

                        });
                    },
                    minLength: 0,

                }).data("autocomplete")._renderItem = function (ul, item) {
                    // Inside of _renderItem you can use any property that exists on each item that we built
                    // with $.map above */
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "" + (item.description == null ? '' : item.description) + "</a>")
                        .appendTo(ul)
                };
            } catch (ex) { }
        }

        function getselectMixedMaterial() {

            if (document.getElementById("selectIsMMA").value == "1") {
                document.getElementById("trFixedMaterial").style.display = "none";
                $('#txtFixedmaterialcode').empty();
            }
            else {
                document.getElementById("trFixedMaterial").style.display = "table-row";
                getMcodeListboxList();
            }
        }

        function getMcodeListboxList() {

            $('#txtFixedmaterialcode').empty();
            $.ajax({
                //url: "LocationHandlers/GetMMCodehandler.ashx?prefix=MAN&choice=SupplierData" + "&TenantID=" + document.getElementById('txtModifyTenant').value,
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=MAN&choice=mcode" + "&Tenant=0",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $.map(data, function (item) {
                        $('#txtFixedmaterialcode').append(
                                $('<option></option>').html(item.split(',')[0])
                            );
                    });
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                }
            });
        }

        function getLocations()
        {
            var vTenantID = '<%=ViewState["TenantID"]%>';

            if (vTenantID == 0) {
                document.getElementById("selSearchCategory").value = 0;
                document.getElementById("txtSearch").value = "Search Part /Tenant";
            }
            else if (vTenantID != 0 && document.getElementById("selSearchCategory").value == 2) {
                document.getElementById("selSearchCategory").value = 2;
                document.getElementById("txtSearch").value = "Search Part /Tenant";
            }

            locinformation.innerHTML = "";

            $("#LocationDataForStock").text('');

            locdetailsarray.splice(0);
            selecteddataarray.splice(0);
            mtlocationarray.splice(0);
            TenantArray.splice(0);
            document.getElementById("myCanvas").width = canvaswidth;
            document.getElementById("myCanvas").height = canvasheight;

            Zonecode = $("#<%=ddlLocationCode.ClientID%>").attr("value");

            if (Zonecode == 0) {
                alert("Please select 'Zone' ");
                UnBlockDialog();
                return false;

            }
            BlockDialog();
            var elem = document.getElementById('myCanvas');
            var canvasContext = elem.getContext("2d");
            var elemLeft = elem.offsetLeft;
            var elemTop = elem.offsetTop;
            //var vTenantID = '<%=ViewState["TenantID"]%>';

            canvasContext.clearRect(0, 0, myCanvas.width, myCanvas.height);
            //An asynchronous call for Getting Total Zonewise Locations

            $.ajax({
                type: "POST",
                url: "LocationHandlers/GetZonewiseLocationsHandler.ashx?zone=" + Zonecode + "&TenantID=" + vTenantID,
                contentType: "text/html",
                success: function (response)
                {
                    if (response == undefined || response == "") {
                        alert("Sorry no data found");
                        UnBlockDialog();
                        return false;
                    }
                    else {
                        SplitData(response);
                    }
                },
                error: function (response) {
                  
                    alert("Sorry no data found");
                    UnBlockDialog();
                    return false;
                },
            });
            //This Method is for Preparing Complete Chart
            function preparechart()
            {
                var LocationCount = locationsdataarray.toString().split('@');
                var TotalLocations = locationsdataarray.length;
                var FilledLocations = LocationCount[LocationCount.length - 1];
                var EmptyLocations = TotalLocations - FilledLocations;
                $("#LocationDataForStock").text('');
                $("#LocationDataForStock").append('Locations Total : ' + locationsdataarray.length);
                //$("#LocationDataForStock").append('&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp; Filled &nbsp;  :  &nbsp; ' + FilledLocations + '&nbsp;&nbsp;  [' + Math.round((FilledLocations * 100) / TotalLocations) + ' %]');
                //$("#LocationDataForStock").append('&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp; Empty &nbsp; : &nbsp; ' + EmptyLocations + '&nbsp;&nbsp;  [' + Math.round((EmptyLocations * 100) / TotalLocations) + ' %]');

                $("#LocationDataForStock").append('&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp; Filled &nbsp;  :  &nbsp; ' + FilledLocations + '&nbsp;&nbsp;  [' + ((FilledLocations * 100) / TotalLocations).toFixed(1) + ' %]');
                $("#LocationDataForStock").append('&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp; Empty &nbsp; : &nbsp; ' + EmptyLocations + '&nbsp;&nbsp;  [' + ((EmptyLocations * 100) / TotalLocations).toFixed(1) + ' %]');

                var vRacksArray = new Array();
                var aisle = 0;
                
                for (var locationsdata = 0; locationsdata < locationsdataarray.length; locationsdata++)
                {
                    
                    if (vRacksArray.indexOf(locationsdataarray[locationsdata].substr(2, 2)) == -1)
                    {
                       
                        vRacksArray[aisle] = locationsdataarray[locationsdata].substr(2, 2);
                        aisle++;
                    }
                }
                var linenumber = 1;
                var MaximumRackBins = 0;
                for (var vRack = 0; vRack < vRacksArray.length; vRack++)
                {
                    bayarray = new Array();
                    bayarray = GetBayData(vRacksArray[vRack]);


                    var bays = getBayWidth(vRacksArray[vRack]);
                    var bins = getBeamLength(vRacksArray[vRack])
                    if (bins > MaximumRackBins)
                    {
                        MaximumRackBins = bins;
                    }

                }
                aisleposx = 10;
                
                for (var vRack = 0; vRack < vRacksArray.length; vRack++)
                {
                    bayarray = new Array();
                    bayarray = GetBayData(vRacksArray[vRack]);
                    
                    
                    var bays = getBayWidth(vRacksArray[vRack]);
                    var bins = getBeamLength(vRacksArray[vRack])
                    var rackwidth = (MaximumRackBins * 25);
                    if (rackwidth < 100) {
                        rackwidth = 100;
                    }
                 
                   
                  
                    var rackHeight = 0;
                    if (vRack == 0 ||vRack%4==0)
                    {
                        var bayCount = GetMaxHeightForRacks(vRack, vRacksArray);
                        if (bayCount == 1) {
                            rackheight = 45;
                        }
                        else if (bayCount > 1)
                            rackheight = bayCount * 33;
                    }

                    
                    drawRacks(aisleposx, aisleposy, rackwidth, rackheight, vRacksArray[vRack]);
                  
                    var bayx = aisleposx;
                    var bayy = aisleposy + rackheight;
                    
                    for (var bay = 0; bay < bayarray.length; bay++)
                    {
                        var bins = GetBeamData(vRacksArray[vRack], bayarray[bay]);
                        //var baywidth = (bins.length) * 25;
                        var baywidth = rackwidth;
                      
                        var bayloc = Zonecode + vRacksArray[vRack] + bayarray[bay];
                       
                        drawBaydata(bayx, bayy - 10, baywidth, 12, bayarray[bay], bayloc);

                        var binx=bayx;
                        var biny=bayy-20;
                        for (var bin = 0; bin < bins.length; bin++)
                        {
                            var binarray = new Array();
                            binarray = GetBinData(vRacksArray[vRack], bayarray[bay], bins[bin]);
                           
                         
                            drawBinData(binx, biny, 20, 10, binarray.toString());
                            binx = binx + 25;
                        }

                        bayy = bayy -30;
                       
                    }

                    if ((vRack + 1) % 4 == 0) {
                        linenumber++;

                        aisleposy += rackheight + 10;

                    }
                    if (linenumber % 2 == 0) {
                        if ((vRack + 1) % 4 == 0) {
                        } else {
                            aisleposx -= rackwidth + 20;
                        }


                    } else {
                        if ((vRack + 1) % 4 == 0) {
                        } else {
                            aisleposx += rackwidth + 20;
                        }

                    }


                    if ((vRack + 1) % 8 == 0) {

                        aisleposy += 30;
                    }
                    /*if ((vRack + 1) % 4 == 0) {
                        linenumber++;
                        aisleposy += rackheight + 10;
                        aisleposx = 10;

                    } else {
                        //aisleposx += rackwidth + 20;
                    }
                   if (linenumber % 2 == 0) 
                    {
                        if ((vRack + 1) % 4 == 0)
                        {
                        } else
                        {
                            aisleposx -= rackwidth + 20;
                        }
                           
                          
                    } else 
                    {
                        if ((vRack + 1) % 4 == 0)
                        {
                        } else {
                            aisleposx += rackwidth + 20;
                        }
                           
                    }
                    if ((vRack + 1) % 8 == 0)
                    {
                       
                        aisleposy += 30;
                    }*/
                   
                
                }
            }

            //This method is used for Darwing Aisle Data
            function drawAisle(x, y, w, h, name) {
                canvasContext.fillStyle = "#003366";
                canvasContext.fillRect(x, y, w, h);
                canvasContext.strokeStyle = "black";
                canvasContext.lineWidth = 2;
                canvasContext.strokeRect(x, y, w, h);
                canvasContext.fillStyle = "#FFFFFF";
                canvasContext.fillText(name, x + 6, y + h / 2);
                canvasContext.canvas.draggable = true;
                aisleposy = y + h + 10;
            }
            //This method is used for Drawing BayData
            function drawBaydata(x, y, w, h, name,bayloc) {
                /*
                var my_gradient = canvasContext.createLinearGradient(x, y, w, h);
                my_gradient.addColorStop(0, "#BFBAAE");
                my_gradient.addColorStop(0.5, "#E6E3DF");
                my_gradient.addColorStop(1, "#CFCBC2");
                canvasContext.fillStyle = my_gradient;
                */
                canvasContext.fillStyle = "#BFBAAE";
                canvasContext.fillRect(x, y, w, h);
                canvasContext.fillStyle = "#000000";
                canvasContext.font = "bold 10px Arial";
                canvasContext.fillText("   Level- " + name, x, y +8);
                locdetailsarray.push(
               {
                   width: w,
                   height: h,
                   xaxis: x,
                   yaxis: y,
                   name: bayloc,
                   color: "black"
               });


            }
            //This method is used for Drawing BeamData
            function drawBeamData(x, y, w, h, beamtext, beamloc) {

                canvasContext.fillStyle = "#E8730E";
                canvasContext.fillRect(x, y, w, h);
                canvasContext.fillStyle = "black";
                canvasContext.fillText("Column-" + beamtext, x, y + 8);
             
                locdetailsarray.push(
                {
                    width: w,
                    height: h,
                    xaxis: x,
                    yaxis: y,
                    name: beamloc,
                    color: "black"
                });
            }
            function drawRacks(x, y, w, h, data)
            {
                canvasContext.fillStyle = "#E8730E";
                canvasContext.fillRect(x, y, w, h);
                canvasContext.fillStyle = "black";
                
                canvasContext.fillStyle = "#333333"
                canvasContext.fillRect(x, y, w, 20);
                canvasContext.fillStyle = "#ffffff"
                canvasContext.font = "bold 15px Arial";
                canvasContext.fillText("Rack-" + data, x+(w/2)-30, y+15);
            }

            //This method is used for Drawing Bindata and Store Binlocaton in elements Array
            function drawBinData(x, y, w, h, loc)
            {
                var LocationSplit = loc.split('@');
                var LocationSplice = LocationSplit.splice(0, 1).toString();

                var Data = LocationSplice.split('|');
                var TenantList = Data.splice(1, 1).toString();

                //if (loc.substr(7).length != 2)
                if (TenantList.length != 0) {
                    loc = loc;//+ "Œ";
                    canvasContext.fillStyle = "#ADFF2F";
                    var col = "#ADFF2F";
                    if (loc.substr(7, 1) == "0") {
                        canvasContext.fillStyle = "#848484";
                        col = "#848484";
                    }
                    //loc = loc.substr(0, 9);
                }
                    // if empty loc
                else {

                    canvasContext.fillStyle = "#E6E3DF";

                    var col = "#E6E3DF";
                    if (loc.substr(7, 1) == "0") {
                        canvasContext.fillStyle = "#848484";
                        col = "#848484";
                    }
                }
                canvasContext.fillRect(x, y, w, h);
                canvasContext.fillStyle = "black";
                canvasContext.font = "Bold";
                canvasContext.font = "bold 10px Arial";
                canvasContext.fillText(loc.substr(7, 2), x + 6, y + 8);
                
                locdetailsarray.push(
                {
                    width: w,
                    height: h,
                    xaxis: x,
                    yaxis: y,
                    //name: loc,
                    name: htmlUnescape(loc),
                    color: col,
                    label: loc.substr(7, 2)
                });

            }

            function htmlUnescape(value) {
                return String(value)
                    .replace(/&quot;/g, '"')
                    .replace(/&#39;/g, "'")
                    .replace(/&lt;/g, '<')
                    .replace(/&gt;/g, '>')
                    .replace(/&amp;/g, '&');
            }

            //This method is used for Getting Beamwise BinCount
            function getBemwiseBinCount(aile, bay, beam) {

                var count = 0;
                var binarray = GetBinData(aile, bay, beam);
                count = binarray.length;
                return count;

            }
            function getRackWidth(rack)
            {
                var max = 0;
                var bayData = GetBayData(rack);
            }
            //this method is used for Deciding BayWidth
            function getBayWidth(aile, bay) {
                var max = 0;
                var beamarray = GetBeamData(aile, bay);
                for (var beam = 0; beam < beamarray.length; beam++) {
                    var binarray = GetBinData(aile, bay, beamarray[beam]);
                    if (max < binarray.length) {
                        max = binarray.length;
                    }
                }
                return max;
            }
            //Getting Rack wise BeamArray Length
            function getBeamLength(aile)
            {
                var max = 0;
              
                for (var bay = 0; bay < bayarray.length; bay++)
                {
                    
                    var beamarray = GetBeamData(aile, bayarray[bay]);
                    
                    if (max < beamarray.length)
                    {
                        max = beamarray.length;
                    }

                }
                return max;
            }
            //Getting Bin Data
            function GetBinData(aile, bay,bin)
            {
                var bindata = new Array();
                var bincount = 0;
                for (var locationsdata = 0; locationsdata < locationsdataarray.length; locationsdata++)
                {
                    
                    if (locationsdataarray[locationsdata].substr(2, 2) == aile && locationsdataarray[locationsdata].substr(4, 1) == bay && locationsdataarray[locationsdata].substr(7, 2) == bin)
                    {
                        
                        bindata[bincount] = locationsdataarray[locationsdata];
                        break;
                    }

                }
                return bindata;

            }
            function GetMaxHeightForRacks(index,rackData)
            {
               
                var maxcount = 0;
                for (var data = index; data < index + 4; data++)
                {
                    var rack = rackData[data];
                   var baydata = GetBayData(rack);
                   if (maxcount < baydata.length) {
                       maxcount = baydata.length;

                   }

                }
              
                return maxcount;
            }
            //Getting Beam data
            function GetBeamData(aile, bay)
            {
                

                var binData = new Array();
                var bin = 0;
                for (var locationsdata = 0; locationsdata < locationsdataarray.length; locationsdata++)
                {
                    if (locationsdataarray[locationsdata].substr(2, 2) == aile && locationsdataarray[locationsdata].substr(4, 1) == bay)
                    {

                        if (binData.indexOf(locationsdataarray[locationsdata].substr(7, 2)) == -1) {
                            binData[bin] = locationsdataarray[locationsdata].substr(7, 2);
                            bin++;
                        }
                    }

                }
                
                return binData;

            }
            //Getting Rack wise Bay Data
            function GetBayData(aile)
            {
                var b = new Array();
                var bay = 0;
                for (var locationsdata = 0; locationsdata < locationsdataarray.length; locationsdata++)
                {

                    if (locationsdataarray[locationsdata].substr(2, 2) == aile)
                    {
                        if (b.indexOf(locationsdataarray[locationsdata].substr(4, 1)) == -1) {
                            b[bay] = locationsdataarray[locationsdata].substr(4, 1);
                            bay++;
                        }
                    }
                }
               
                return b;
            }
            //this method is used for splitting the responce into an array
            function SplitData(response)
            {
               
                locationsdataarray = response.toString().split('Ñ');
                Zonecode = locationsdataarray[0].substr(0, 2);
                //reinitilise the variables with default values
                aisleposx = 10, aisleposy = 10, aisleposh = 0, ileposw = 20, bayheight = 10, binwidth = 15, binheight = 10, beamheight = 9, constwidth = 20;
                preparechart();
                UnBlockDialog();
            }

            //function for deleting locations
            return false;

           
        }


        /* AtHome Display Logic */


        function GetLocations() {

            // alert("okkkkkkkkkkkkk");
            BlockDialog();
            $.ajax({
                type: "POST",
                async: true,
                url: 'LocationManager.aspx/GetLocations',
                contentType: "application/json; charset=utf-8",
                data: "{ 'phaseName':'" + $("#MainContent_MMContent_ddlLocationCode option:selected").text() + "' }",
                dataType: "json",
                success: function (data) {                    
                    DrarCanvasChart(data);
                },
                error: function (result) {
                    alert("Error");
                    UnBlockDialog();
                }
            });
        }


        function DrarCanvasChart(data) {           
            var elem = document.getElementById('myCanvas');
            var canvasContext = elem.getContext("2d");
            // canvasContext.clearRect(0, 0, myCanvas.width, myCanvas.height);
            canvasContext.clearRect(0, 0, canvaswidth, canvasheight);


            document.getElementById("myCanvas").width = canvaswidth;
            document.getElementById("myCanvas").height = canvasheight;
           

            var raxkX = 10, rackY = 50, rackWidth = 25, rackHeight = 400;

            if (data.d.length == 0)
            {
                showStickyToast(false, "No location found", false);
            }

            //Loop through Rack
            for (var index = 0; index < data.d.length; index++) {
                // alert(data.d[index].RackName);

                //Drawing Rack
                //var newcolor;
                rackHeight = GetRackHeight(data.d[index]);
                DrawShape(raxkX, rackY, rackWidth, rackHeight, canvasContext, data.d[index].RackName, 'Rack', '', '#E67F22', '#FFFFFF');


                var cloumnX = raxkX + 30 - 2;
                var columnY = rackY + rackHeight - 20;
                var columnWidth = 150;
                var columnHeight = 20;
                //Loop throuth columns in each rack
                for (var colIndex = 0; colIndex < data.d[index].ColumnList.length; colIndex++) {

                    //Draw(cloumnX, columnY, columnWidth, columnHeight, canvasContext, data.d[index].ColumnList[colIndex].ColumnName);
                    var levelX = cloumnX;
                    var levelY = columnY;
                    var levelWidth = columnWidth;
                    var levelHeight = columnHeight;

                    //loop through levels on each rack column
                    for (var levelIndex = 0; levelIndex < data.d[index].ColumnList[colIndex].LevelList.length; levelIndex++) {
                        //drawing levels
                        DrawShape(levelX, levelY, levelWidth, levelHeight, canvasContext, data.d[index].ColumnList[colIndex].ColumnName + '-' + data.d[index].ColumnList[colIndex].LevelList[levelIndex].LevelName, 'Level', '', '#CCCCCC', '#000000');


                        var binX = levelX;
                        var binY = levelY - 25;
                        var binWidth = 20;
                        var binHeight = 20;
                        for (var binIndex = 0;
                            binIndex < data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList.length;
                            binIndex++) {

                            var Label = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].BinName;
                            var binData = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].bindata;
                            var Account = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].Account;
                            var Tenant = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].Tenant;
                            var Location = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].FullLocation;
                            var IsActive = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].IsActive;
                            var TenantID = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].TenantID;
                            var MCode = data.d[index].ColumnList[colIndex].LevelList[levelIndex].binList[binIndex].MCode;

                            var sData = Location + IsActive + '!' + TenantID + '!' + Tenant + '!' + MCode + '|' + binData;



                            var bgcolor = binData.length == 0 ? "#e0e0e0" : "#ADFF2F";

                            DrawShape(binX, binY, binWidth, binHeight, canvasContext, Label, 'Bin', sData, bgcolor, '#000000');

                            locdetailsarray.push(
                                {
                                    width: binWidth,
                                    height: binHeight,
                                    xaxis: binX,
                                    yaxis: binY,
                                    name: sData,
                                    color: "#E6E3DF",
                                    label: Label
                                });

                            binX = binX + 30;
                        }
                        levelY = levelY - 50;
                    }
                    cloumnX = cloumnX + columnWidth + 30.
                }
                rackY = rackY + rackHeight + 10;
            }

           
            UnBlockDialog();
        }

        var binInfoArray = new Array();
        function DrawShape(x, y, w, h, canvasContext, text, type, ToolTipInfo, bgcolor, fontcolor) {

            //alert(text);
            canvasContext.fillStyle = bgcolor;
            canvasContext.fillRect(x, y, w, h);
            canvasContext.strokeStyle = "black";
            canvasContext.lineWidth = 2;
            //canvasContext.strokeRect(x, y, w, h);
            //canvasContext.fillStyle = '#FFFFFF';
            canvasContext.fillStyle = fontcolor;
            canvasContext.font = "9pt Arial";
            canvasContext.fillText(text, x + 6, y + h / 2);
            canvasContext.canvas.draggable = true;

            if (type == 'Bin') {
                binInfoArray.push(
                    {
                        width: w,
                        height: h,
                        xaxis: x,
                        yaxis: y,
                        name: ToolTipInfo//.split('zzMakkyzz')[1],
                    });
            }

        }


        function GetRackHeight(oRack)
        {
            var rackHeight = 0;
            for (var i = 0; i < oRack.ColumnList.length; i++)
            {
                if (oRack.ColumnList[i].LevelList.length > rackHeight)
                    rackHeight = oRack.ColumnList[i].LevelList.length;
            }
            return rackHeight * 50;
        }
        /* END AtHome Display Logic */







    </script>
      <!-- Script for loading Warehousewide Zones-->
      <script type="text/javascript">
          
          $(document).ready(function () {
              $("#canwidth .ui-slider").css('background', '#003366');
              $("#<%=ddlWarehouse.ClientID%>").change(function () {
                  $("#<%=ddlLocationCode.ClientID%>").html("");
                  var DivisionID = $("#<%=ddlWarehouse.ClientID%>").attr("value");
                  if (DivisionID != 0) {
                    
                      $.ajax({
                          type: "POST",
                          url: "LocationHandlers/Warehouse_LocationCode.ashx?whid=" + DivisionID,
                          contentType: "application/json",
                          success: function (locationcodes) {
                     
                              $("#<%=ddlLocationCode.ClientID%>").append($("<option></option>").val('0').html('Select Zone'));
                              $.each(locationcodes, function () {
                           
                                  if (this['ID'] != undefined) {
                                      $("#<%=ddlLocationCode.ClientID%>").append($("<option></option>").val(this['ID']).html(this['LocationCode']));
                                  }
                              });
                          },
                          error: function (response) {                             
                              showStickyToast(false, "No zones available", false);
                              return false;
                          }
                      });
                  }
              });


          });
          //close update dialog
          function CloseDialog(id) {

              $("#txtBayTenant").val("");
              $("#txtBaySupplier").val("");
              $("#txtModifyTenant").val("");
              $("#txtModifySupplier").val("");
              $("#txtSelectTenant").val("");
              $("#txtSupplier").val("");

              $(id).dialog("close");
          }
          function ClosePrintDialog(id) {

          

              $('#'+id+'').dialog("close");
          }
          function SearchCategory() {
              debugger;
               var searchid = document.getElementById("selSearchCategory").value;
               var vTenantID = '<%=ViewState["TenantID"]%>';
                           
              if (searchid == 1)
              {
              
                  getTenantAutoComplete(vTenantID);
                  
                 // $('.mcodepicker').autocomplete('search', '');
              } else if (searchid == 2) {
                  getMaterialAutoComplete(vTenantID);
                  //$('.mcodepicker').autocomplete('search', '');
              } else  {
                  document.getElementById("selSearchCategory").focus();
                  document.getElementById("txtSearch").value = "Search Part /Tenant";
                  GetSearchCategory();
              }
          }

          function GetSearchCategory() {

              $('.mcodepicker').autocomplete({
                  source: function (request, response) {
                      return false;
                  },
              });
          }

          //Searching for Materilawiselocationns or department wise locations
          function doSearch()
          {
              var SearchElement = document.getElementById("selSearchCategory").value;

              if (SearchElement == 1 || SearchElement == 2) {

                  SerachTenantData(SearchElement);
              }
              else {                 
                  showStickyToast(false, "Select search category", false);
              }
          }

          function SerachTenantData(SearchElement) {
              
              BlockDialog();
              if (SearchElement == 2) {
                  //here iam going to search for material wise locations

                  var searchelement = document.getElementById("txtSearch").value.trim();

                  searchelement = searchelement.toUpperCase();
                  if (searchelement.length >= 3) {
                      drawPrevoiusData();
                      mtlocationarray.splice(0);
                      TenantArray.splice(0);

                      locdetailsarray.forEach(function (loc) {
                          
                          if (loc.name.indexOf(searchelement) != -1) {
                              mtlocationarray.push({
                                  name: loc.name,
                                  label: loc.label,
                                  xaxis: loc.xaxis,
                                  yaxis: loc.yaxis,
                                  width: loc.width,
                                  height: loc.height,
                                  color: loc.color
                              });

                          }
                      });
                      drawSearchData();
                      UnBlockDialog();
                  } else {
                     
                      showStickyToast(false, "Part number must be more than 3 characters", false);
                      UnBlockDialog();
                  }
              }
              else if (SearchElement == 1) {
                  //here iam going to search for department wise locations
                  var searchelement = document.getElementById('hidTenantID').value;

                  drawPrevoiusData();
                  mtlocationarray.splice(0);
                  TenantArray.splice(0);

                  locdetailsarray.forEach(function (loc) {                     
                      //var LocationSplit = loc.name.split('@');
                      var LocationSplice = loc.name.split('@')[0].toString();//.splice(0, 1).toString();

                      var TenantMaterialList = LocationSplice.split('|');
                      var TenantList = TenantMaterialList.splice(0, 1).toString();
                      var LocationList = TenantList;
                      var MaterialList = TenantMaterialList.toString();

                      var TenantData = TenantList.split('!');
                      var TenantID = TenantData.splice(1, 1).toString();
                      //var TenantID = TenantData;

                      //if (loc.name.indexOf(searchelement) != -1)
                      if (MaterialList.length != 0 && TenantID == searchelement) {
                          mtlocationarray.push({
                              name: loc.name,
                              label: loc.label,
                              xaxis: loc.xaxis,
                              yaxis: loc.yaxis,
                              width: loc.width,
                              height: loc.height,
                              color: loc.color
                          });
                      }

                      if (LocationList.length != 0 && TenantID == searchelement) {
                          TenantArray.push({
                              name: loc.name,
                              label: loc.label,
                              xaxis: loc.xaxis,
                              yaxis: loc.yaxis,
                              width: loc.width,
                              height: loc.height,
                              color: loc.color,
                          });
                      }


                  });

                  drawSearchData();
                  //document.getElementById("hidTenantID").value = 0;

              }
          }
         
          function drawPrevoiusData()
          {
              var searchelement = document.getElementById("txtSearch").value.trim();
              
              //searchelement = "$" + searchelement;
              mtlocationarray.forEach(function (mdata) {
                  var elem = document.getElementById('myCanvas');
                  var canvasContext = elem.getContext("2d");
                  canvasContext.fillStyle = mdata.color;
                  canvasContext.fillRect(mdata.xaxis, mdata.yaxis, mdata.width, mdata.height);
                  canvasContext.fillStyle = "black";
                  canvasContext.fillText(mdata.label, mdata.xaxis + 6, mdata.yaxis + 8);
              });

              TenantArray.forEach(function (mdata) {
                  var elem = document.getElementById('myCanvas');
                  var canvasContext = elem.getContext("2d");
                  canvasContext.fillStyle = mdata.color;
                  canvasContext.fillRect(mdata.xaxis, mdata.yaxis, mdata.width, mdata.height);
                  canvasContext.fillStyle = "black";
                  canvasContext.fillText(mdata.label, mdata.xaxis + 6, mdata.yaxis + 8);
              });
          }
          //drawing data on canvas after searching
          function drawSearchData()
          {
              locinformation.innerHTML = '';
              if (TenantArray.length != 0) {
                  TenantArray.forEach(function (Tenantloc) {
                      var elem = document.getElementById('myCanvas');
                      var canvasContext = elem.getContext("2d");
                      var grd = canvasContext.createRadialGradient(Tenantloc.xaxis - 2, Tenantloc.yaxis - 2, 5, Tenantloc.xaxis + 6, Tenantloc.yaxis + 6, 20);
                      grd.addColorStop(0, "#ff4dff");

                      canvasContext.fillStyle = grd;
                      canvasContext.fillRect(Tenantloc.xaxis, Tenantloc.yaxis, Tenantloc.width, Tenantloc.height);
                      canvasContext.fillStyle = "black";
                      canvasContext.fillText(Tenantloc.label, Tenantloc.xaxis + 6, Tenantloc.yaxis + 8);

                  });
              }

              if (mtlocationarray.length != 0) {

                  mtlocationarray.forEach(function (mtloc) {
                      var elem = document.getElementById('myCanvas');
                      var canvasContext = elem.getContext("2d");
                      //alert(mtloc.name.substr(0,7));
                      // canvasContext.fillStyle = "#CCFF00";
                      //canvasContext.fillStyle = fillStyle.
                      var grd = canvasContext.createRadialGradient(mtloc.xaxis - 2, mtloc.yaxis - 2, 5, mtloc.xaxis + 6, mtloc.yaxis + 8, 20);
                      grd.addColorStop(0, "#58ACFA");
                      grd.addColorStop(1, "#0066FF");

                      canvasContext.fillStyle = grd;
                      canvasContext.fillRect(mtloc.xaxis, mtloc.yaxis, mtloc.width, mtloc.height);
                      canvasContext.fillStyle = "black";

                      canvasContext.fillText(mtloc.label, mtloc.xaxis + 6, mtloc.yaxis + 8);
                      locinformation.innerHTML += mtloc.name.substring(0, 9) + "&nbsp&nbsp&nbsp";
                  });
              }

              else {                 
                  showStickyToast(false, "No materials are available", false);
              }
              UnBlockDialog();
          }
          //prasad
          function checkNum(evt)
          {
              
              evt = (evt) ? evt : window.event;
              var charCode = (evt.which) ? evt.which : evt.keyCode;             
             
              if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                  status = "This field accepts numbers only."
                  return false;
              }
              status = "";
              return true;
          }

          function checkNumInclZero(evt) {

              evt = (evt) ? evt : window.event;
              var charCode = (evt.which) ? evt.which : evt.keyCode;

              if (charCode == 48) {
                  return false;
              }
              if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                  status = "This field accepts numbers only."
                  return false;
              }
              status = "";
              return true;
          }
        
          function ClearText1(TextBox) {
         
              if (TextBox.value="Search RT Part # ...")
              {
                  TextBox.value = "";
                  TextBox.style.color = "#000000";
              }
              function FillText(TextBox)
              {
                  TextBox.value = "Search RT Part # ...";
              }
               
          }

          function ClearText(TextBox) {

              if (document.getElementById('txtSearch').value = "Search Part /Tenant") {
                  document.getElementById('txtSearch').value = "";
                  document.getElementById('txtSearch').style.color = "#000000";

              }
          }

          function FillDefaultText() {

              if (document.getElementById('txtSearch').value.trim() == "") {

                  document.getElementById('txtSearch').value = "Search Part /Tenant";
              }
          }

          function GetSelectedData() {
              if (document.getElementById("selSearchCategory").value == 0) {
                  document.getElementById("txtSearch").value = "Search Part /Tenant";
              }
              else if (document.getElementById("selSearchCategory").value == 1) {
                  document.getElementById("txtSearch").value = "Search Tenant";
              }
              else if (document.getElementById("selSearchCategory").value == 2) {
                  document.getElementById("txtSearch").value = "Search Part Number";
              }
          }
        
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <table align="center" style="margin: 5px;">
        <tr>
            <td align="left" colspan="2" class="auto-style1">
                <span style="font-weight: bold;"></span>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="auto-style1">
                <!--Creating Zone Selection DropDown-->

                <table cellspacing="3" cellpadding="3" class="cardss nopad">
                    <tr>
                        <td>
                            <div class="flex">
                            <asp:DropDownList ID="ddlWarehouse" runat="server" required="" />
                            <label>Warehouse</label>
                            </div>
                        </td>
                        <td>
                            <div class="flex">
                                <asp:DropDownList ID="ddlLocationCode" runat="server" required="" />
                                <label>Zone</label>
                             </div>
                        </td>
                        <td>  
                           <div class="flex"><input type="button" id="btnGetlocation" onclick="GetLocations()" class="ui-btn ui-button-large" value="Get Location Map" />  
                               
                           </div>
                        </td>                       

                        <td>
                            <table id="tbltenant" runat="server">
                                <tr>
                                    <td>
                                        <input type="button" id="btnCreate" onclick="getSliderDialog()" class="ui-btn ui-button-large" value="Bulk Create" />
                                    </td>
                                    <td>
                                        <input type="button" id="btnLocationdelete" onclick="deleteLocations()" class="ui-btn ui-button-large" value="Delete" />
                                    </td>
                                    <td>
                                        <input type="button" id="btnUpdatelocation" onclick="GetUpdateDialog()" class="ui-btn ui-button-large" value="Modify" />
                                    </td>
                                    <td>
                                        <input type="button" id="btnAdding" onclick="GetAddingDialog()" class="ui-btn ui-button-large" value="Add" />
                                    </td>
                                </tr>
                            </table>
                        </td>

                        

                    </tr>
                     <tr>
                        <td>
                           Category :
                            <asp:DropDownList ID="selSearchCategory" runat="server" ClientIDMode="Static" Style="" class="txt_Blue_Small" onchange="GetSelectedData()">
                                <asp:ListItem Value="0" Selected="True">Category</asp:ListItem>
                                <asp:ListItem Value="1">Tenant</asp:ListItem>
                                <asp:ListItem Value="2">Part Number</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input type="text" id="txtSearch" class="mcodepicker" onclick="SearchCategory();" onkeydown="SearchCategory()" value="Search Part /Tenant" style="" onfocus="ClearText(this)" onblur="FillDefaultText();" />
                        </td>
                        <td>
                            <input type="button" id="btnSearch" onclick="doSearch()" class="ui-btn ui-button-large" value="Get Details" />
                        </td>
                        <td>
                            <%--<input type="button" id="btnPrint" onclick="PrintLocations()" class="ui-btn ui-button-large " value="Print" />--%>
                            <asp:LinkButton runat="server" ClientIDMode="Static" OnClientClick="PrintLocations();return false" CssClass="ui-btn ui-button-large " ID="btnPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></asp:LinkButton>


                        </td>
                    </tr>
                </table>

            </td>

        </tr>
        <tr>
            <td align="left">

                <table border="0" align="center" cellpadding="0" cellspacing="0" class="internalData cardss" width="100%" style="padding-top: 0">
                    <tr>
                        <td width="70%">
                            <table>
                                <tr>
                                    <td width="445">Increase Width<br />
                                        <br />
                                        <div id="canwidth" class="slide" style="margin-right: 25px;"></div>
                                    </td>

                                    <td></td>

                                    <td width="445">Increase Height<br />
                                        <br />
                                        <div id="canheight" class="slide"></div>
                                    </td>

                                    <td>&nbsp;<br />
                                        <br />
                                        <asp:DropDownList ID="ddlPrinter" runat="server" CssClass="NoPrint" />
                                        <button type="button" id="btnbulkprint" onclick="getSliderDialogForBulkPrint()" class="ui-btn ui-button-large bkprint">Bulk Print</button>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>

            <td>
                <div class="internalData">
                    <br />
                    <div class="fix-flex-evenly">
                        <div class="flex-nows">
                            <div>Empty  </div>
                            <div class="WhiteLabel cc"></div>
                        </div>
                        <div class="flex-nows">
                            <div>Filled  </div>
                            <div class="YellowLabel cc"></div>
                        </div>
                        <div class="flex-nows">
                            <div>In Active  </div>
                            <div class="GreyLabel cc"></div>
                        </div>
                        <div class="flex-nows">
                            <div>Search Filled  </div>
                            <div class="BlueLabel cc"></div>
                        </div>
                        <div class="flex-nows">
                            <div>Search Allocated</div>
                            <div class="PinkLabel cc"></div>
                        </div>
                    </div>
                </div>

            </td>
        </tr>
       
    </table>

    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <div id="locinformation" style="max-width: 973px; height: auto; line-height: 2; background-color: #ddddc7; word-wrap: break-word;"></div>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <div id="LocationDataForStock" class="locInfo"></div>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="auto-style1">
                <!--Creating Canvas for Drawing Locations-->
               <%-- <iframe style="width:100%;">   --%> 
                        <canvas id="myCanvas" class="can" width="1200" height="2000" style="position: relative; overflow: auto;">this is visible When Ur Browser Doesn't Suppourt Canvas
                        </canvas>
                    
                <%--</iframe>--%>

            </td>
        </tr>
    </table>
        
      <!--Creating Dialog for LocationDeatils-->
    <div id="alertdialog" Title="Warning">
    </div>
      <!--Creating Dialog for CreateLocations-->
    <div id="locationcreatedialog" class="cssdailog">
        <div style="padding: 5px">
            <table width="100%" style="padding: 10px;">
                <tr>
                    <td>
                        <span class="mandatory_field">*</span>Aisle :<br />
                        <input type="number" id="txtAisle" class="txtAisle textcss" min="1" onkeypress="return checkNumInclZero(event);" />
                    </td>

                    <td>
                        <span class="mandatory_field">*</span>Racks / Aisle :
                        <br />
                        <input type="number" id="txtRack" class="txtRack textcss" min="1" onkeypress="return checkNumInclZero(event);" />
                    </td>
                    <td>
                        <span class="mandatory_field">*</span>Columns / Rack :<br />
                        <input id="txtColumn" type="number" class="txtColumn textcss" min="1" onkeypress="return checkNumInclZero(event);" />
                    </td>
                    <td>
                        <span class="mandatory_field">*</span> Levels / Rack :<br />
                        <input type="number" id="txtLevel" class="txtLevel textcss" min="1" onkeypress="return checkNumInclZero(event);" />
                    </td>
                    <td>
                        <span class="mandatory_field">*</span>Bins  :<br />
                        <input type="number" id="txtBin" class="txtBin textcss" min="1" onkeypress="return checkNumInclZero(event);" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr style="border-color: white; border-width: 5px;" />
                    </td>
                </tr>
                <tr>

                    <td colspan="3">Tenant:
                        <br />
                        <input type="text" id="txtSelectTenant" class="TenantListboxPicker textcss"  style="width:65%;" onclick="getTenantForMultipleSupplier('Create');" onkeydown="getTenantForMultipleSupplier('Create');" />
                        <asp:HiddenField ID="hifTenant" runat="server" />
                        <br /><p></p>

                        <div id="trSupplier">
                            Supplier:
                            <br />
                            <select id="txtSupplier" name="txtSupplier" multiple="multiple" style="border:1px solid lightgrey;width:90% !important;" ></select>
                            <asp:HiddenField ID="hifSupplierID" runat="server" />
                        </div>
                    </td>

                    <td colspan="2">
                        <table>
                            <tr>

                                <td valign="top">
                                    Width (cm)<br>
                                    <input type="text" id="txtWidthB" value="0" class="textcss"  onkeypress="return checkNum(event);"><br />
                                    Height (cm)<br>
                                    <input type="text" id="txtHeightB" value="0" class="textcss" onkeypress="return checkNum(event);"></td>
                                <td valign="top">
                                    Length (cm)<br>
                                    <input type="text" id="txtLengthB" value="0" class="textcss" onkeypress="return checkNum(event);"><br />
                                    Max Weight (kg)<br>
                                    <input type="text" id="txtMaxWeightB" value="0" class="textcss"  onKeyPress="return checkNum(event);"></td>                                
                            </tr>
                        </table>
                    </td>

                </tr>

                <tr>
                    <td>
                        <input type="checkbox" id="cbxfastmove" class="form-control" />Is Fast Moving</td>
                    <td style="display:none;">CBM<br>
                        <div id="divCbmB"></div><br />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="InsertLocations()" class="ui-button-large ui-btn">Create<%=MRLWMSC21Common.CommonLogic.btnfaSave %> </a>
                <a onclick="CloseDialog(locationcreatedialog)" class="ui-button-large ui-btn">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %> </a>
                <%--<input type="button"  value="Create" onclick="InsertLocations()" class="ButSave"/>
                <input type="button" id="btncancel" value="Cancel" onclick="CloseDialog(locationcreatedialog)" class="ButSave"/>--%>
            </div>
        </div>
    </div>

    <!--Create Dialog to Print Locations-->
    <div id="printlocationdialog" class="cssdailog">
        <div style="padding: 5px">
            <table width="100%" style="padding-left: 53px;">
                <tr>
                    <td>&nbsp&nbsp</td>
                </tr>

                <tr>
                    
                    <td>Rack :<br />
                        <select id="ddlRackPrint" class="ddlRackPrint" onchange="ddlRackPrint_Change();" ></select>
                    </td>
                   
                    <td>Columns(Bay) :<br />
                        <select id="ddlColumnPrint" class="ddlColumnPrint" onchange="ddlColLevPrint_Change();" ></select>
                    </td>
                    <td>Level(Beam) :<br />
                        <select id="ddlLevelPrint"  class="ddlLevelPrint" onchange="ddlColLevPrint_Change();" ></select>
                    </td>
                    <td>Bin :<br />
                        <select id="ddlBinPrint" class="ddlBinPrint" ></select>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="InitatePrint()" class="ui-button-large ui-btn">Print<%=MRLWMSC21Common.CommonLogic.btnfaSave %> </a>
                <a onclick="CloseDialog(printlocationdialog)" class="ui-button-large ui-btn">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %> </a>
                <%--<input type="button"  value="Create" onclick="InsertLocations()" class="ButSave"/>
                <input type="button" id="btncancel" value="Cancel" onclick="CloseDialog(locationcreatedialog)" class="ButSave"/>--%>
            </div>
        </div>
    </div>
        <!--Creating Dialog for CreateBeamLocations-->
    <div id="beamdialog">
        <div style="padding: 5px;">
            <table style="padding: 27px;">
                <tr>
                    <td>Location/Bin</td>
                </tr>
                <tr>
                    <td>
                        <div id="beamlocval">1</div>
                        <div id="beamlocslider" class="slide"></div>
                    </td>
                    <td>
                        <div id="beamlocval1"></div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp&nbsp</td>
                </tr>
                <tr>
                    <td>&nbsp&nbsp</td>
                </tr>

                <tr>

                    <td>Tenant:
                        <br />
                        <input type="text" id="txtBayTenant" class="TenantListboxPicker" onclick="getTenantForMultipleSupplier('Bay');" onkeydown="getTenantForMultipleSupplier('Bay');" />

                    </td>
                    <td>
                        <table>
                            <tr id="trBaySupplier">
                                <td>Supplier:
                                    <br />
                                    <select id="txtBaySupplier" name="txtBaySupplier" multiple="multiple"></select>
                                </td>
                            </tr>
                        </table>

                    </td>

                </tr>

            </table>
        </div>

        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">

                <a onclick="InsertBeamLocations()" class="ui-button-large ui-btn">Create<%=MRLWMSC21Common.CommonLogic.btnfaSave %> </a>
                <a onclick="CloseDialog(beamdialog)" class="ui-btn ui-button-large">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a>
                <%--<asp:LinkButton ID="btnbeamCreate" runat="server" OnClientClick="InsertBeamLocations()" CssClass="ui-btn ui-button-large">Create<%=MRLWMSC21Common.CommonLogic.btnfaSave %></asp:LinkButton>--%>
                <%--<asp:Button ID="btnbeamCreate" runat="server" Text="Create" OnClientClick="InsertBeamLocations()" CssClass="ButSave"/>
                    <input type="button" id="Button1" value="Cancel" onclick="CloseDialog(beamdialog)" class="ButSave"/>--%>
            </div>
        </div>

    </div>

   <div id="Addinglocationcreatedialog" class="cssdailog">
        <div style="padding: 5px">
            <table width="100%" style="padding: 10px;">
                <tr>                    
                    <td>
                        <span class="mandatory_field">*</span>Rack :
                        <br />
                        <select id="ddlRack" class="ddlRack" ></select>                          
                    </td>
                    <td>
                        Column Or Level :<br />
                         <select id="ddlColumnOrLevel" class="ddlColumnOrLevel" >
                             <option value="1">Column</option>
                              <option value="2">Level</option>
                         </select>  
                    </td>
                    <td>
                        <span id="txtColumnMand" class="mandatory_field">*</span>Column(Bay) :<br />
                        <input id="txtColumnAdd" type="text" class="txtColumnAdd textcss" onkeypress="return checkNum(event);" />
                    </td>
                    <td>
                        <span id="txtLevelMand" class="mandatory_field">*</span> Level(Beam) :<br />
                        <input type="text" id="txtLevelAdd" class="txtLevelAdd textcss" onkeypress="return checkNum(event);" />
                    </td>
                    <td>
                        <span class="mandatory_field">*</span>Bin :<br />
                        <input type="text" id="txtBinAdd" class="txtBinAdd textcss" onkeypress="return checkNum(event);" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr style="border-color: white; border-width: 5px;" />
                    </td>
                </tr>
                <tr>

                    <td colspan="3">Tenant:
                        <br />
                        <input type="text" id="txtSelectTenantAdd" class="TenantListboxPicker txtcss" style="width:65%;" onclick="getTenantForMultipleSupplier('+');" onkeydown="getTenantForMultipleSupplier('Create');" />
                        <asp:HiddenField ID="hifTenantAdd" runat="server" /><br />
                         <div id="trSupplierAdd">
                            Supplier: <br />
                            <select id="txtSupplierAdd" name="txtSupplierAdd" multiple="multiple" style="border:1px solid lightgrey;width:90%;"></select>
                            <asp:HiddenField ID="hifSupplierAdd" runat="server" />
                        </div>
                    </td>

                    <td>
                       Width (cm)<br>
                        <input type="text" id="txtWidthAdd" value="0" class="textcss" onkeypress="return checkNum(event);"><br />
                        Height (cm)<br>
                        <input type="text" id="txtHeightAdd" value="0" class="textcss" onkeypress="return checkNum(event);"></td>
                    <td>Length (cm)<br>
                        <input type="text" id="txtLengthAdd" value="0" class="textcss" onkeypress="return checkNum(event);"><br />
                        Max Weight (kg)<br>
                        <input type="text" id="txtMaxWeightAdd" value="0" class="textcss" onkeypress="return checkNum(event);">
                        <div style="display:none;">
                            CBM: <span id="divCbmAdd"></span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="cbxfastmoveAdd" class="form-control" />Is Fast Moving</td>
                </tr>
            </table>
        </div>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="AddingInsertLocations()" class="ui-button-large ui-btn">Create<%=MRLWMSC21Common.CommonLogic.btnfaSave %> </a>
                <a onclick="CloseDialog(Addinglocationcreatedialog)" class="ui-button-large ui-btn">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %> </a>
                <%--<input type="button"  value="Create" onclick="InsertLocations()" class="ButSave"/>
                <input type="button" id="btncancel" value="Cancel" onclick="CloseDialog(locationcreatedialog)" class="ButSave"/>--%>
            </div>
        </div>
    </div>



  


    <div id="displayBox">
    </div>
    <!--alert for creating locations-->
    <div id="createlocalert">
        <div style="padding: 10px;">
            <span id="createlocalertspan"></span>
        </div>      
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="createLocations()" class="ui-btn ui-button-large">OK<span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(createlocalert)" class="ui-btn ui-button-large">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a>
                <%--<input type="button" value="OK" onclick="createLocations()" class="ButSave" />
                <input type="button" value="Cancel" onclick="CloseDialog(createlocalert)" class="ButSave" />--%>
            </div>
        </div>
    </div>


      <div id="updatebindialog" class="cssdailog" >
        <div style="padding:5px;">
            <table>
                <tr>
                    <td colspan="3">
                        <div id="UpdateError" style="color: red"></div>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">Tenant:
                        <br />
                        <input type="text" id="txtModifyTenant" class="TenantListboxPicker txtcss" style="width:65%;" onclick="getTenantForMultipleSupplier('Modify');" onkeydown="getTenantForMultipleSupplier('Modify');" value="Modify" />
                        <input type="hidden" id="hidTenantID" /><br />
                        Supplier:
                        <br />
                        <select id="txtModifySupplier" name="txtModifySupplier" multiple="multiple" style="border:1px solid lightgrey; width:90%;"></select>
                    </td>
                    <td>Width (cm)<br>
                        <input type="text" id="txtwidth" class="textcss" onkeypress="return checkNum(event);"><br />
                        Height (cm)<br>
                        <input type="text" id="txtHeight" class="textcss" onkeypress="return checkNum(event);">
                    </td>
                    <td>Length (cm)<br>
                        <input type="text" id="txtLength" class="textcss" onkeypress="return checkNum(event);"><br />
                        Max.Weight (kgs)<br>
                        <input type="text" id="txtMaxweight" class="textcss" onkeypress="return checkNum(event);" /><br />
                        CBM : <span id="divCbm"></span>
                        <br />
                    </td>


                </tr>


                <tr>
                    <td>Is Active<br />
                        <select id="selectIsActive">
                            <option value="1">Yes</option>
                            <option value="0">No</option>
                        </select>
                    </td>
                    <td>Is Quarantine<br />
                        <select id="selectIsQuarantine">
                            <option value="1">Yes</option>
                            <option value="0">No</option>
                        </select>
                    </td>
                    <td>Is Mixed Material Allowed<br>
                        <select id="selectIsMMA" onchange="getselectMixedMaterial()">
                            <option value="1">Yes</option>
                            <option value="0">No</option>
                        </select>
                    </td>
                    <td>Is FastMoving<br>
                        <select id="selectIsFMA">
                            <option value="1">Yes</option>
                            <option value="0">No</option>
                        </select>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <table>
                            <tr id="trFixedMaterial">
                                <td>Fixed MaterialCode<br>
                                    <input type="text" id="txtFixedmaterialcode" class="mcodepicker" onclick="getMMCodeList();" onkeydown="getMMCodeList();" />
                                   
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>



            </table>
            </div>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                     
                     <a onclick="UpdateLocationDetails()" class="ui-btn ui-button-large" >Update<%=MRLWMSC21Common.CommonLogic.btnfaUpdate %></a>
                     <a id="btnupCancel" onclick="CloseDialog(updatebindialog)" class="ui-btn ui-button-large" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a>
                     <%--<input type="button" id="btnUpdate" value="Update" onclick="UpdateLocationDetails()" class="ButSave"/>
                     <input type="button" id="btnupCancel" value="Cancel" onclick="CloseDialog(updatebindialog)" class="ButSave"/>--%>
                </div>
            </div>
        </div>


   <!-- Adding Locations Alert-->
    <div id="Addinglocalert">
        <div style="padding: 10px;">
            <span id="Addinglocalertspan"></span>
        </div>
        <%--<table>
            <tr>
                <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                <td>
                </td>
            </tr>
        </table>--%>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="AddingLocations()" class="ui-btn ui-button-large">OK<span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(Addinglocalert)" class="ui-btn ui-button-large">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a>               
            </div>
        </div>
    </div>









    <!--Alert for Printing location-->
      <div id="printlocaalert" class="printlocaalert">
        <div>
            <span id="printlocalertspan"></span>
        </div>        
    </div>

    <!--alert for Delete locations-->
    <div id="deletealert">
         <div style="padding:10px;">
             <span id="deletealertspan"></span>
             </div>
        
       <%-- <table>
            <tr>
                 <td>&nbsp</td> <td>&nbsp</td>
                <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
            </tr>
            <tr>
                <td>&nbsp</td> <td>&nbsp</td>
                <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                <td>
                    
                </td>
            </tr>
        </table>--%>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="doDelete()" class="ui-btn ui-button-large" >OK<span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(deletealert)" class="ui-btn ui-button-large" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a>     
                <%--<input type="button" value="OK" onclick="doDelete()" class="ButSave" />
                <input type="button" value="Cancel" onclick="CloseDialog(deletealert)" class="ButSave" />--%>
                
                     
            </div>
        </div>
        
    </div>
     <!--alert for create beam locations-->
        <div id="createbeamlocalert">
            <div style="padding:10px;">
                <span id="createbeamlocalertspan"></span>
            </div>
        
        <%--<table>
            <tr>
                 <td>&nbsp</td> <td>&nbsp</td>
                <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
            </tr>
            <tr>
                <td>&nbsp</td> <td>&nbsp</td>
                <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                  <td>&nbsp</td> <td>&nbsp</td>
                <td>
                </td>
            </tr>
        </table>--%>

        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="CreateBeamLocations()" class="ui-btn ui-button-large" >OK<span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(createbeamlocalert)" class="ui-btn ui-button-large" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></a>
                <%--<input type="button" value="OK" onclick="CreateBeamLocations()" class="ButSave" />
                <input type="button" value="Cancel" onclick="CloseDialog(createbeamlocalert)" class="ButSave" />--%>
            
                
        
            </div>
        </div>
    </div>
   
  <!--Warning alert-->
    <div id="warningalert">
        <div style="padding:5px;">
        <span id="warningalertspan"></span>
        <table align="right">
            <tr>
                 <td>
                  
            </td>
            </tr>
           
        </table>
       </div>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="CloseDialog(warningalert)" class="ui-button-large ui-btn">OK<span class="space fa fa-check-circle-o"></span> </a>
                <%--<input type="button" value="OK" onclick="CloseDialog(warningalert)" class="ButWhite" />--%>
            </div>
        </div>
    </div>
    <div class="tooltip" id="tool">Tooltip!</div>
  </div>
</asp:Content>



