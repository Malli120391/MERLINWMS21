<%@ Page Title=" Location Manager :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="LocationManager.aspx.cs" EnableEventValidation="false" Inherits="MRLWMSC21.mMaterialManagement.LocationManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
     
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
     <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <script src="LocationManager.js"></script>

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
            $.post('LocationWebClientPrintDemo.ashx',
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


        .darkMode .ui-widget-content {
    background: var(--primarycolor) !important;
}

    .darkMode .ui-widget-content table {
        background: var(--primarycolor) !important;
    }

        .darkMode .ui-widget-content table input, .darkMode .ui-widget-content table select {
            background: var(--primarycolor) !important;
            color:#fff !important;
        }

        .darkMode .ui-dialog .flex{
             background-color: var(--primarycolor) !important;
            color:#fff !important;
        }
          .darkMode .ui-dialog .flex input{
             background-color: var(--primarycolor) !important;
            color:#fff !important;
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

#txtSupplierAdd{
    background:transparent !important;
        min-width: 150px !important;
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


.chk [tooltip]:before {
    content: attr(tooltip);
    background: #585858;
    padding: 5px 7px;
    margin-right: 10px;
    border-radius: 2px;
    color: #FFF;
    font: 500 12px Roboto;
    white-space: nowrap;
    position: absolute;
    bottom: 20%;
    right: 100%;
    visibility: hidden;
    opacity: 0;
    transition: .3s;
}

.chk [tooltip]:hover:before {
    visibility: visible;
    opacity: 1;
}

        /*.chk:hover [tooltip]::after{
            opacity:1;
        }*/

    .can 
    {
          background-color:ActiveBorder;
     }
    .slide 
    {
            width:400px;
            z-index:0;    
     }
    /*.textcss {
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
    }*/
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

        .txt_Blue_Small {
            min-height:33px !important;
                min-width: 180px !important;
        }

     

        .supLabel {
            border:1px solid #E7E7E7;
            padding:5px;
            display:inline-block;
        }


    .ui-autocomplete 
    { 
      position: absolute;   
     
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

.close {
    position: absolute;
    color: red;
    right: 8px;
    z-index: 1;
    background: white !important;
    color: black !important;
    border-radius: 100%;
    padding: 0px 5px !important;
    top: 8px;
}

.c-check {
    position: relative;
    display: inline;
    user-select:none;
}

    .c-check label {
        cursor: pointer;
    }

        .c-check label:before, .c-check label:after {
            content: "";
            position: absolute;
            left: 0;
            top: 0;
        }

        .c-check label:before {
            width: 16px;
            height: 16px;
            background: #fff;
            border: 2px solid rgba(0, 0, 0, 0.54);
            border-radius: 3px;
            cursor: pointer;
            transition: background 0.3s;
            left: 0px;
        }

    .c-check input[type="checkbox"] {
        outline: 0;
        margin-right: 10px;
        position: absolute;
    }

        .c-check input[type="checkbox"]:checked + label:before {
            background: var(--sideNav-bg);
            border-color: var(--sideNav-bg);
        }

        .c-check input[type="checkbox"]:checked + label:after {
            transform: rotate(-45deg) !important;
            top: 4px;
            left: 4px;
            width: 10px;
            height: 5px;
            border: 2px solid #fff;
            border-top-style: none;
            border-right-style: none;
        }

    .c-check input[type="checkbox"] {
        visibility: hidden;
       
    }

    .relative .btn {
        border-radius: 100% !important;
        padding: 10px !important;
    }

      #divTooltip {
            width: 500px;
            min-height: 200px;
            max-height: 200px;
            overflow: auto;
            background-color: #ffffff;
            position: absolute;
            display: none;
            box-shadow: var(--z2);
            border-radius: 1px;
                transform: translate(206px, 10px);
      }

         #divTooltip::-webkit-scrollbar, body::-webkit-scrollbar, #divMapContainer::-webkit-scrollbar {
                width: 5px;
                background-color: var(--sideNav-bg);
         }

         #divTooltip::-webkit-scrollbar-thumb, body::-webkit-scrollbar-thumb, #divMapContainer::-webkit-scrollbar-thumb {
              background-color: #eaf5ff;
            border-radius: 30px;
         }

         #divTooltip::-webkit-scrollbar-track, body::-webkit-scrollbar-track, #divMapContainer::-webkit-scrollbar-track {
            background-color: var(--sideNav-bg);
         }

         ::selection {
            color: white; 
            background: var(--sideNav-bg);
        }





         #divTooltip .flex__ div {
            align-self:unset !important;
         }

     .iconblock {
         padding: 5px;
         box-shadow: var(--z1);
         min-height: 190px;
         max-height: 200px;
     }

      /*#divMap    
      {
          margin-bottom:300px;
      }*/
      
      .divRackContainer {
        /*display:flex;*/
        display:-webkit-box;
        border:0px solid red;
      }
        .divRackName {
            background-color: var(--sideNav-bg);
            color: white;
            margin-bottom: 10px;
            /*display: inline-block;*/
            display: -webkit-box;
            text-align: center;
            font-weight: 600;
            font-size: 17px;
            width: 25px;
            box-shadow: var(--z1-1);
            align-self: center;
            display: flex;
            justify-content: center;
            border-radius:3px;
            /*background-color: var(--sideNav-bg);
            color: white;
            display: inline-block;
            text-align: center;
            font-size: 15px;
            width: 30px;
            align-self: center;
            height: 30px !important;
            line-height: 30px !important;
            border-radius:5px*/
        }
      .divColumnContainer {
        display:inline-block;
        align-self:center;
      }
      .divColumn {
           /*background-color:lightgrey;*/
            color:black;
            margin-right:10px;
            display:inline-block;
      }
        .divLevel {
            /*background-color: #fff3eb;
            margin-bottom: 10px;
            margin-top: 4px;
            padding-left: 5px;
            line-height: 20px;
            margin-left: 5px;
            font-size: 8pt;
            font-weight: 700;
            box-shadow: var(--z1-1);
            text-align: center;*/
            background-color: #fbfbfb;
            margin-bottom: 10px;
            margin-top: 4px;
            padding-left: 5px;
            padding-right: 5px;
            line-height: 20px;
            margin-left: 5px;
            font-size: 8pt;
            font-weight: 700;
            text-align: center;
            border: 1px solid #dbdae6;
            border-top: none;
            border-radius: 3px;
                border-top-right-radius: 0px;
    border-top-left-radius: 0px;
        }

        .spanBin {
            /*border: 0px solid #E7E7E7;
        background-color: #E7E7E7;
        margin-left: 5px;
        color: black;
        font-size: 8pt;
        padding: 4px;
        cursor: pointer;
        box-shadow: 1px 1px 1px -1px red;
        box-shadow: var(--z1);*/
            background-color: #e4e1ef;
            margin-left: 5px;
            color: black;
            font-size: 8pt;
            padding: 6px;
            cursor: pointer;
            /*box-shadow: 1px 1px 1px -1px red;
            box-shadow: var(--z1);*/
            /*border: 1px solid #cccaca;*/
            /*box-shadow: 1px 3px 5px #CCCCCC;*/
            border-radius: 3px;
        }

        .spanBin, .divRackContainer {
            -webkit-user-select: none; /* webkit (safari, chrome) browsers */
            -moz-user-select: none; /* mozilla browsers */
            -khtml-user-select: none; /* webkit (konqueror) browsers */
            -ms-user-select: none; /* IE10+ */
        }

        .EmptyBin {
            /*background-color: #E7E7E7;*/
            background-color: #e4e3f1;
        }
      .binSelect {
        background-color:var(--sideNav-bg) !important;
        color: #fff;
         
      }

    
      .bindataClass {
        /*background-color:chartreuse;*/
        background-color: #109e16;
    color: white;
      }
     .TenantClass {
         background-color: cornflowerblue;
     }
     .mCodeClass {
         background-color: turquoise;
     }

      .table-striped tr th {
        font-size:8pt;
         padding:0 !important;
      }
      .table-striped tr td {
        font-size:8pt !important;
        padding:0 !important;
        vertical-align:middle;
      }

     .iconStyle {
    display: block;
     padding: 40% 12%;
     }

     .iconStyle:last-of-type{
        padding-bottom:0px;
     }

     .spanIndiCators {
     }
     strike {
        color:red;
     }
     .tooltipHeader {
     margin:5px;color:#336699;
     font-size:11pt;
     margin-top:0px;
     }

     .table_block {
             width: calc(100% - 60px);
    padding: 10px;
     }

     .fix-flex-evenly {
         width: 100%;
         display: flex;
         flex-wrap: wrap;
         justify-content: left;
         position: fixed;
         bottom: 1px;
         left: 32%;
         z-index: 99999;
     }

    .flex-nows {
    display: flex;
    justify-content: flex-start;
    margin-left: 5px;
 margin-top: 5px;
}
     .card2 {
         box-shadow: var(--z1);
    padding: 10px 0px;
     }

     .relative .material-icons {
        margin-left:0px !important;
            font-size: 21px !important;
     }

     

     .sugg-tooltis {
         transition: transform 200ms ease-in;
         bottom: unset;
         top: -25px;
         left: -7%;
     }
     .relative:hover .sugg-tooltis {
         transform: scale3d(1, 1, 1);
         transition: transform 200ms ease-in;
         bottom: unset;
         top: -25px;
         left: -7%;
     }

        .btn-float {
            min-width:unset !important;
            line-height:unset !important;
            height:unset !important;
            background-color:var(--sideNav-bg);
        }

        toggleswitch {
                top: 52px !important;
        }
  </style>
    <asp:HiddenField ID="hdnRoles" runat="server" />
    <input type="hidden" id="hdnTenantID" runat="server" value="0" />

    <div class="container">
    <div class="">
         <table align="center" style="position: relative;">
        <tr>
            <td align="left" colspan="2" class="auto-style1">
                <span style="font-weight: bold;"></span>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="auto-style1">
                <!--Creating Zone Selection DropDown-->
                
 <!-- Globalization Tag is added for multilingual  -->
                <div cellspacing="3" cellpadding="3">
                    <div class="row">
                        <div class="col m3">

                            <div class="flex">                                 
                            <asp:DropDownList ID="ddlWarehouse" CssClass="ddlWarehouse" runat="server" required="" />
                               
                            <label><%= GetGlobalResourceObject("Resource", "Warehouse")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <asp:DropDownList ID="ddlLocationCode"  runat="server" required="" Width="200" />
                                <%--<label>Zone</label>--%>
                                <label> <%= GetGlobalResourceObject("Resource", "Zone")%></label>
                                 <span class="errorMsg"></span>
                                <input type="hidden" id="hdnSelectedZone" class="hdnSelectedZone" runat="server" value="0" />
                             </div>
                        </div>
                        <div class="col m6">  
                            <div class="flex__">
                           <div class="relative">
                                <button type="button" onclick="LoadMap();" class="btn btn-float"> <i class="material-icons vl">my_location</i></button> 
                              <%-- <em class="sugg-tooltis" style="    width: 96px;     left: -87%;">Load Location Map</em>--%>
                                <em class="sugg-tooltis" style="    width: 96px;     left: -87%;"> <%= GetGlobalResourceObject("Resource", "LoadLocationMap")%></em>
                               <asp:LinkButton ID="btnLoadMap" CssClass="btn btn-primary" onclick="btnLoadMap_Click" runat="server" style="display:none;">Show Map(Server Logic)</asp:LinkButton>
                           </div>
                        
                            <table id="tbltenant" runat="server" style="width: fit-content;">
                                <tr>
                                    <td>
                                        <div class="flex__">&nbsp;&nbsp;
                                            <div class="relative">
                                                <button type="button" id="btnCreate" onclick="getSliderDialog()" class="btn btn-float"><i class="material-icons vl">playlist_add</i></button>
                                                <%-- <em class="sugg-tooltis" style="width: 57px; left: -45%;">Bulk Create</em>--%>
                                                 <em class="sugg-tooltis" style="width: 57px; left: -45%;"> <%= GetGlobalResourceObject("Resource", "BulkCreate")%> </em>
                                            </div>&nbsp;
                                            <div class="relative">
                                                <button type="button" id="btnLocationdelete" onclick="deleteLocations()" class="btn btn-floating btn-float"> <i class="material-icons vl">delete</i></button>
                                                <%--<em class="sugg-tooltis">Delete</em>--%>
                                                <em class="sugg-tooltis"><%= GetGlobalResourceObject("Resource", "Delete")%> </em>
                                            </div>&nbsp;
                                             <div class="relative">
                                                <button type="button" id="btnUpdatelocation" onclick="GetUpdateDialog()" class="btn btn-float"> <i class="material-icons vl">mode_edit</i></button>
                                                <%--<em class="sugg-tooltis">Modify</em>--%>
                                                 <em class="sugg-tooltis"> <%= GetGlobalResourceObject("Resource", "Modify")%></em>
                                            </div>&nbsp;
                                              <div class="relative">
                                                 <button type="button" id="btnUpdatelocationBulk" onclick="GetUpdateDialogBulk()" class="btn btn-float"><i class="material-icons vl">border_color</i></button>
                                               <%--  <em class="sugg-tooltis" style="width: 60px; left: -42%;">Bulk Modify </em>--%>
                                                    <em class="sugg-tooltis" style="width: 60px; left: -42%;"> <%= GetGlobalResourceObject("Resource", "BulkModify")%> </em>
                                             </div>&nbsp;
                                             <div class="relative">
                                                <button style="visibility:hidden" type="button" id="btnAdding" onclick="GetAddingDialog()" class="btn btn-float"><i class="material-icons vl">add</i></button>
                                             <%--   <em class="sugg-tooltis">Add</em>--%>
                                                    <em class="sugg-tooltis"> <%= GetGlobalResourceObject("Resource", "Add")%></em>
                                            </div>&nbsp;
                                            <div class="relative">
                                                <button type="button" id="toggledbtns"  class="btn btn-float"><i class="material-icons vl">more_vert</i></button>
                                               <%-- <em class="sugg-tooltis" style="    width: 92px;  left: -80%;">Advanced Options</em>--%>
                                                 <em class="sugg-tooltis" style="    width: 92px;  left: -80%;"> <%= GetGlobalResourceObject("Resource", "AdvancedOptions")%> </em>
                                            </div>
                                           <%-- <div class="switch3D">
                                                <input type="checkbox" id="switch" /><label for="switch">Toggle</label>
                                            </div>--%>
                                        </div>
                                     </td>
                                </tr>
                            </table>
                                </div>
                        </div>

                        

                    </div>
                    <div class="toggleBlock" hidden>
                     <div class="row">
                        <div class="col m3">
                            <div class="flex">
                                <asp:DropDownList ID="selSearchCategory" runat="server" ClientIDMode="Static" Style="" required="" class="txt_Blue_Small w200" onchange="GetSelectedData()">
                                 <%--   <asp:ListItem Value="0" Selected="True">Select Search Category</asp:ListItem>--%>
                                       <asp:ListItem Value="0" Selected="True">Select Search Category </asp:ListItem>
                                    <asp:ListItem Value="1">Tenant</asp:ListItem>
                                    <asp:ListItem Value="2">Part Number</asp:ListItem>
                                </asp:DropDownList>
                              <%--  <label>Category </label>--%>
                                  <label><%= GetGlobalResourceObject("Resource", "Category")%> </label>
                                </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtSearch" class="mcodepicker w200" onclick="SearchCategory();" onkeydown="SearchCategory()" required="" />
                             <%--   <label>Search Part /Tenant</label>--%>
                                   <label id="lblSearch"><%= GetGlobalResourceObject("Resource", "SearchPartTenant")%> </label>
                            </div>
                        </div>
                        <div class="col m6">
                            <div class="flex__">
                                <div class="relative"><button type="button" id="btnSearch" onclick="doSearch1()" class="btn btn-float"> <i class="material-icons vl">playlist_add_check</i></button><em class="sugg-tooltis" style="width: 55px; left: -50%;"><%= GetGlobalResourceObject("Resource", "GetDetails")%></em></div>
                                            

                                </div>
                         </div>
                    </div>

                    <div class="row" id="myForm">
                 <div class="col m3">
                     <div class="flex">

                           
                         <input type="hidden" id="sid" name="sid" value="locationManager" />
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
                        <div class="col m3 s3">
                              <flex style="align-items:center">  <div class="relative">
                                    <asp:LinkButton runat="server" ClientIDMode="Static" OnClientClick="PrintLocations();return false" CssClass="btn btn-float" ID="btnPrint"> <i class="material-icons vl">print</i></asp:LinkButton><em class="sugg-tooltis"><%= GetGlobalResourceObject("Resource", "Print")%> </em>
                                </div>
                                <asp:DropDownList ID="ddlPrinter"  Visible="false" runat="server" CssClass="NoPrint" Style="width: 212px !important; padding-left: 1px;" />
                                <div class="relative">
                                    <gap5 style="height:6px;"></gap5>
                                    <button type="button" id="btnbulkprint" onclick="getSliderDialogForBulkPrint()" data-target="#BulkPrintLocations" data-toggle="modal" class="btn bkprint btn-float"><i class="material-icons vl">print</i></button><em class="sugg-tooltis" style="width: 50px; left: -30%;"> <%= GetGlobalResourceObject("Resource", "BulkPrint")%></em>
                                </div>
                                  </flex>
                        </div>
                    </div>
                        </div>
                </div>


                
            </td>

        </tr>
    
        <tr>

            
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
    </table>
        
      <!--Creating Dialog for LocationDeatils-->
    <div id="alertdialog" Title="Warning">
    </div>
      <!--Creating Dialog for CreateLocations-->
    <div id="locationcreatedialog" class="cssdailog" style="max-height:400px !important">
        <div style="padding: 5px">
                <div class="row">
                    <br />
                    <div class="col m4" style="display: none;">
                        <div class="flex">
                            <input type="number" id="txtAisle" class="txtAisle textcss" min="1" onkeypress="return checkNumInclZero(event);" required="" />
                            <%--   <label>Aisle</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "Aisle")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>

                    <div class="col m4" style="display: none;">
                        <div class="flex">
                            <input type="number" id="txtRack" class="txtRack textcss" min="1" onkeypress="return checkNumInclZero(event);" required="" />
                            <%--    <label>Racks / Aisle </label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "RacksAisle")%> </label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                    <div class="col m4" style="display: none;">
                        <div class="flex">
                            <input id="txtColumn" type="number" class="txtColumn textcss" min="1" onkeypress="return checkNumInclZero(event);" required="" />
                            <span class="errorMsg"></span>
                            <%--<label>Columns / Rack</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "ColumnsRack")%></label>
                        </div>
                    </div>
                    <div class="col m4" style="display: none;">
                        <div class="flex">
                            <input type="number" id="txtLevel" class="txtLevel textcss" min="1" onkeypress="return checkNumInclZero(event);" required="" />
                            <span class="errorMsg"></span>
                            <%--  <label> Levels / Rack</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "LevelsRack")%> </label>
                        </div>
                    </div>
                    <div class="col m4" style="display: none;">
                        <div class="flex">
                            <input type="number" id="txtBin" class="txtBin textcss" min="1" onkeypress="return checkNumInclZero(event);" required="" />
                            <%--  <label>Bins</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "Bins")%> </label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>

                    <!-- ========================== Added New Concept build for dropdowns Rack,Level,Column,bins By M.D.Prasad On 08-Nov-2019  ============================== -->

                    <div class="col m3">
                        <div class="flex">
                            <select id="fromRackID">
                            </select>
                            <label>From Rack</label>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <select id="fromLevelID">
                            </select>
                            <label>From Level</label>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <select id="fromColumnID">
                            </select>
                            <label>From Column</label>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <select id="fromBinID">
                            </select>
                            <label>From Bin</label>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <select id="toRackID">
                            </select>
                            <label>To Rack</label>
                        </div>
                    </div>



                    <div class="col m3">
                        <div class="flex">
                            <select id="toLevelID">
                            </select>
                            <label>To Level</label>
                        </div>
                    </div>



                    <div class="col m3">
                        <div class="flex">
                            <select id="toColumnID">
                            </select>
                            <label>To Column</label>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <select id="toBinID">
                            </select>
                            <label>To Bin</label>
                        </div>
                    </div>


                    <!-- ========================== END  ============================== -->










                    <hr />






                </div>
                <div class="row">
                    <div>

                        <div class="">

                            <div class="row">
                                <div class="col m3">
                                    <div class="flex">
                                        <input type="text" id="txtWidthB" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                                        <span class="errorMsg"></span>
                                        <%--  <label>Width (cm)</label>--%>
                                        <label><%= GetGlobalResourceObject("Resource", "Widthcm")%></label>
                                    </div>
                                </div>
                                <div class="col m3">
                                    <div class="flex">
                                        <input type="text" id="txtHeightB" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                                        <span class="errorMsg"></span>
                                        <label><%= GetGlobalResourceObject("Resource", "Heightcm")%> </label>
                                    </div>
                                </div>

                                <div class="col m3">
                                    <div class="flex">
                                        <input type="text" id="txtLengthB" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                                        <span class="errorMsg"></span>
                                        <%-- <label>Length (cm)</label>--%>
                                        <label><%= GetGlobalResourceObject("Resource", "Lengthcm")%> </label>
                                    </div>
                                </div>
                                <div class="col m3">
                                    <div class="flex">
                                        <input type="text" id="txtMaxWeightB" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                                        <span class="errorMsg">*</span>
                                        <%-- <label>Max Weight (kg)</label>--%>
                                        <label><%= GetGlobalResourceObject("Resource", "Maxwt")%> </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">

                    <div class="col m6">
                        <div class="flex">
                            <input type="text" id="txtSelectTenant" class="TenantListboxPicker textcss" required="" onclick="getTenantForMultipleSupplier('Create');" onkeydown="getTenantForMultipleSupplier('Create');" />
                            <%-- <asp:HiddenField ID="hifTenant" runat="server" />--%>
                            <input type="hidden" id="hifTenant" value="0" />
                            <%--     <label>Tenant</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            <%-- <span class="errorMsg"></span>--%>
                        </div>

                    </div>
                    <div class="col m6">

                        <div id="trSupplier">
                            <%= GetGlobalResourceObject("Resource", "Supplier")%>
                            <br />
                            <select id="txtSupplier" name="txtSupplier" multiple="multiple" style="border: 1px solid lightgrey; width: 100% !important;"></select>
                            <%-- <asp:HiddenField ID="hifSupplierID" runat="server" />--%>
                            <input id="hifSupplierID" type="hidden" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col m3">
                       <div class="checkbox"> <input type="checkbox" id="cbxfastmove" class="form-control" />
                         <%--  <label for="cbxfastmove">Is Fast Moving</label>&emsp;--%>
                             <label for="cbxfastmove"> <%= GetGlobalResourceObject("Resource", "IsFastMoving")%></label>&emsp;
                       </div>
                        <div style="display:none;">
                            <%= GetGlobalResourceObject("Resource", "CBM0")%> <span id="divCbmB"></span>
                        </div>
                     </div>
                    <div class="col m3">
                        
                         <div class="flex">
                            <select id="ddlLocTypeBulkCreate"  class="textcss ddlLocTypeBulkCreate" runat="server" style="width:100% !important;" required=""></select>
                   <%--          <label>Location Type </label>--%>
                                       <label><%= GetGlobalResourceObject("Resource", "LocationType")%> </label>
                        </div>
                        

                    </div>
                </div>
        </div>
        <div class="ui-dailog-footer">
            <div style="padding:5px">
               
                <a onclick="CloseDialog(locationcreatedialog)" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Cancel")%> </a>
                 <a onclick="InsertLocations()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Create")%> </a>
            </div>
        </div>
    </div>

    <!--Create Dialog to Print Locations-->
    <div id="printlocationdialog" class="cssdailog">
        <div style="padding: 5px">
            <div>
                <div class="row"></div>
                
            </div>
        </div>
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
               <%-- <a onclick="InitatePrint()" class="btn btn-primary">Print</a>--%>

            </div>
        </div>
    </div>
        <!--Creating Dialog for CreateBeamLocations-->
    <div id="beamdialog">
        <div style="padding: 5px;">
            <table style="padding: 27px;">
                <tr>
                   <%-- <td>Location/Bin</td>--%>
                     <td><%= GetGlobalResourceObject("Resource", "LocationBin")%></td>
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
                                <td><%= GetGlobalResourceObject("Resource", "Supplier")%>
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
            <div style="padding: 5px">

                <a onclick="InsertBeamLocations()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Create")%> </a>
                <a onclick="CloseDialog(beamdialog)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Cancel")%> </a>
            </div>
        </div>

    </div>

  <div id="Addinglocationcreatedialog" class="cssdailog" >
        <div style="padding: 5px">
            <div width="100%" style="padding: 10px;">
                <div class="row">                    
                    <div class="col m4 s4">
                        <div class="flex">
                            <select id="ddlRack" class="ddlRack w100" required=""></select>    
                            <span class="errorMsg">*</span>
                              <label> <%= GetGlobalResourceObject("Resource", "Rack")%></label>
                        </div>
                    </div>
                    <div class="col m4 s4">
                        <div class="flex">
                             <select id="ddlColumnOrLevel" class="ddlColumnOrLevel w100" required="">
                                 <option value="1">Column</option>
                                  <option value="2">Level</option>
                             </select>  
                             <label> <%= GetGlobalResourceObject("Resource", "ColumnOrLevel")%> </label>
                         </div>
                    </div>
                    <div class="col m4 s4">
                        <div class="flex">
                            <input id="txtColumnAdd" type="text" class="txtColumnAdd textcss w100" onkeypress="return checkNum(event);" required="" />
                            <span id="txtColumnMand" class="errorMsg">*</span>
                           
                             <label> <%= GetGlobalResourceObject("Resource", "ColumnBay")%>  </label>
                        </div>
                    </div>
                    <div class="col m4 s4">
                        <div  class="flex">
                            <input type="text" id="txtLevelAdd" class="txtLevelAdd textcss w100" onkeypress="return checkNum(event);" required="" />
                            <span id="txtLevelMand" class="errorMsg">*</span> 
                          
                            <label> <%= GetGlobalResourceObject("Resource", "LevelBeam")%> </label>
                         </div>
                    </div>
                 
                    <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" id="txtBinAdd" class="txtBinAdd textcss w100" onkeypress="return checkNum(event);" required="" />
                            <span class="errorMsg">*</span>
                         
                             <label><%= GetGlobalResourceObject("Resource", "Bin")%> </label>
                        </div>
                    </div>
                  <%-- </div>--%>
               
               <%-- <div class="row">--%>
                    <div class="col m4 s4">
                         <div class="flex">
                            <input type="text" id="txtSelectTenantAdd" required="" class="TenantListboxPicker txtcss" onclick="getTenantForMultipleSupplier('+');" onkeydown="getTenantForMultipleSupplier('+');" />

                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            <input type="hidden" id="hifTenantAdd" value="0" />
                            <asp:HiddenField ID="hifTenantAdd1" runat="server" />
                             <%--<span class="errorMsg"></span>--%>
                        </div>
                    </div>
                    </div>
                 <div class="row">
                    <div class="col m4 s4">
                       

                        <div id="trSupplierAdd" class="flex">
                            <%= GetGlobalResourceObject("Resource", "Supplier")%>
                            <select id="txtSupplierAdd" name="txtSupplierAdd" multiple="multiple" style="border: 1px solid lightgrey;"></select>
                            <input id="hifSupplierAdd" type="hidden" />
                        </div>
                    </div>
                    <%--</div>
                    <div class="row">--%>
                    <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" id="txtWidthAdd" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                            <label><%= GetGlobalResourceObject("Resource", "Widthcm")%></label>
                             <span class="errorMsg"></span>
                       </div>
                        </div>
                       <%-- <br />--%>
                        <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" id="txtHeightAdd" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                            <label><%= GetGlobalResourceObject("Resource", "Heightcm")%> </label>
                             <span class="errorMsg"></span>
                        </div>
                        </div>
                    <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" id="txtLengthAdd" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                            <label> <%= GetGlobalResourceObject("Resource", "Lengthcm")%></label>
                             <span class="errorMsg"></span>
                        </div>
                        </div>
                        <br />
                        <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" id="txtMaxWeightAdd" value="0" class="textcss" onkeypress="return checkNum(event);" required="">
                            <label><%= GetGlobalResourceObject("Resource", "MaxWeightkg")%></label>
                             <span class="errorMsg"></span>
                        </div>
                        <div style="display:none;">
                            <%= GetGlobalResourceObject("Resource", "CBM")%> <span id="divCbmAdd"></span>
                        </div>
                    </div>
                        </div>
                
                <div class="row">
                    <div class="col m3">
                        <div class="checkbox">
                            <input type="checkbox" id="cbxfastmoveAdd" class="form-control" />
                              <label for="cbxfastmoveAdd"><%= GetGlobalResourceObject("Resource", "IsFastMoving")%> </label></div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                        <select  id="ddlLocTypeAdd" runat="server"  class="textcss ddlLocTypeAdd"  required=""></select>
                          
                              <label><%= GetGlobalResourceObject("Resource", "LocationType")%> </label>
                            </div>
                    </div>
                    <br />
                    <br />
                </div>
              
            </div>
        </div>    
        <div class="ui-dailog-footer" style="position: inherit;">
            <div style="padding: 5px;">
                <a onclick="CloseDialog(Addinglocationcreatedialog)" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Cancel")%> </a>              
                <a onclick="AddingInsertLocations()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Create")%> </a>
                
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
            <div style="padding: 5px;">
                <a onclick="createLocations()" id="btnCreateLocations" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "OK")%><span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(createlocalert)" id="btnCancelCreateLocations" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Cancel")%></a>               
            </div>
        </div>
    </div>


      <div id="updatebindialog" class="cssdailog" style="max-height:350px !important">
        <div style="padding:5px;">
            <div>
                <div class="row">
                    <div class="col m12" colspan="3">
                        <div id="UpdateError" style="color: red"></div>
                    </div>
                </div>

                <div class="row">
                    <div class="col m4 s4">
                        <div class="flex">
                            <input required="required" type="text" id="txtModifyTenant" class="TenantListboxPicker txtcss" onclick="getTenantForMultipleSupplier('Modify');" onkeydown="getTenantForMultipleSupplier('Modify');" value="Modify" />
                            <input type="hidden" id="hidTenantID" />
                            <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            <%--<span class="errorMsg"></span>--%>
                        </div>
                         <%= GetGlobalResourceObject("Resource", "Supplier")%>
                        <br />
                        <select id="txtModifySupplier" name="txtModifySupplier" multiple="multiple" style="border:1px solid lightgrey; width:100%;"></select>
                    </div>
                    <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" required="" id="txtwidth" class="textcss" onkeypress="return checkNum(event);">
                            <label> <%= GetGlobalResourceObject("Resource", "Widthcm")%></label>
                        </div>
                        <br />
                        <div class="flex">
                            <input type="text" required="" id="txtHeight" class="textcss" onkeypress="return checkNum(event);">
                            <label><%= GetGlobalResourceObject("Resource", "Heightcm")%> </label>
                        </div>
                    </div>
                    <div class="col m4 s4">
                        <div class="flex">
                            <input type="text" required="" id="txtLength" class="textcss" onkeypress="return checkNum(event);">
                            <label> <%= GetGlobalResourceObject("Resource", "Lengthcm")%></label>
                        </div>
                       <br />
                       <div class="flex">
                            <input type="text" required="" id="txtMaxweight" class="textcss" onkeypress="return checkNum(event);" />
                             <label><%= GetGlobalResourceObject("Resource", "MaxWeightkg")%></label>
                       </div>                       
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col m4" colspan="2">
                      
                     <div class="flex">
                            <select required="" id="ddlLocType" class="textcss ddlLocType" runat="server"></select>
                            <%--<label>LocationType</label>--%>
                         <label> <%= GetGlobalResourceObject("Resource", "LocationType")%></label>
                        </div>
                    </div>
                    <div class="col m4">
                        <gap></gap>
                         <span><%= GetGlobalResourceObject("Resource", "CBM")%> <span id="divCbm"></span></span>
                    </div>
                <div id="trFixedMaterial" class="col m4 s4">
                    <div class="flex">
                        <input type="text" id="txtFixedmaterialcode" class="mcodepicker" onclick="getMMCodeList();" onkeydown="getMMCodeList();" required="" />
                        <label><%= GetGlobalResourceObject("Resource", "FixedMaterialCode")%></label>
                    </div>
              
            </div>
                </div>
            </div>
          


            <div class="row">
                <div class="flex__ alex____" style="justify-content: space-between; width: 50%;">
                    <div class="chk isActivess">
                        <input type="checkbox" id="selectIsActive">
                        <label for="selectIsActive"></label>
                        <span data-tooltip="Active Location"></span>
                    </div>

                    <div class="chk Quarantine">
                        <input type="checkbox" id="selectIsQuarantine">
                        <label for="selectIsQuarantine"></label>
                        <span data-tooltip="Quarantine Location"></span>
                    </div>

                    <div class="chk mixedm">
                        <input type="checkbox" id="selectIsMMA"  onchange="getselectMixedMaterial();" >
                        <label for="selectIsMMA"  ></label>
                        <span data-tooltip="Allow Mixed Material"></span>
                    </div>

                    <div class="chk fastmoving">
                        <input type="checkbox" id="selectIsFMA">
                        <label for="selectIsFMA"></label>
                        <span data-tooltip="Fast-Moving Location"></span>
                    </div>
                </div>
            </div>
   <%--         <div class="row" style="">
                <div id="trFixedMaterial" class="col m4 s4">
                    <div class="flex">
                        <input type="text" id="txtFixedmaterialcode" class="mcodepicker" onclick="getMMCodeList();" onkeydown="getMMCodeList();" required="" />
                        <label><%= GetGlobalResourceObject("Resource", "FixedMaterialCode")%></label>
                    </div>
                </div>
            </div>--%>

        </div>
          <div class="ui-dailog-footer">
              <div style="padding: 5px;">
                  <a id="btnupCancel" onclick="CloseDialog(updatebindialog)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Cancel")%></a>
                  <a onclick="UpdateLocationDetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Update")%></a>
              </div>
          </div>
        </div>

       <!-- Bulk Modify dialog-->
        <div id="updatebindialogBulk" class="cssdailog" style="    max-height: 370px !important;">
            <div style="padding: 5px;">
                <div>
                    <div class="row">
                        <div colspan="3">
                            <div id="UpdateErrorBulk" style="color: red"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col m3">
                            <div class="flex">
                                <select id="ddlRackBulk" class="ddlRackBulk w100" onchange="ddlRackBulk_Change();" required=""></select>
                                <label> <%= GetGlobalResourceObject("Resource", "Rack")%></label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <select id="ddlColumnBulk" class="ddlColumnBulk w100" required=""></select>
                                <label> <%= GetGlobalResourceObject("Resource", "Column")%> </label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <select id="ddlLevelBulk" class="ddlLevelBulk w100" required=""></select>
                                <label><%= GetGlobalResourceObject("Resource", "Level")%> </label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                 <input required="required" type="text" id="txtModifyTenantBulk" class="txtcss w100"  />
                           
                            <input type="hidden" id="hidTenantIDBulk" />                             
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                <%--<span class="errorMsg"></span>--%>
                        </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col m6" colspan="2">
                           <br />
                            <%= GetGlobalResourceObject("Resource", "Supplier")%>
                            <br />
                            <select id="txtModifySupplierBulk" name="txtModifySupplier" multiple="multiple" style="border: 1px solid lightgrey; width: 100%;"></select>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtwidthBulk" class="textcss" onkeypress="return checkNum(event);" required="">
                                <label> <%= GetGlobalResourceObject("Resource", "Widthcm")%></label>
                                <span class="errorMsg"></span>
                            </div>
                            <br />
                            <div class="flex">
                                <input type="text" id="txtHeightBulk" class="textcss" onkeypress="return checkNum(event);" required="">
                                <label><%= GetGlobalResourceObject("Resource", "Heightcm")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtLengthBulk" class="textcss" onkeypress="return checkNum(event);" required="">
                                <label><%= GetGlobalResourceObject("Resource", "Lengthcm")%></label>
                                <span class="errorMsg"></span>
                            </div>
                            <br />
                            <div class="flex">
                                <input type="text" id="txtMaxweightBulk" class="textcss" onkeypress="return checkNum(event);" required="" />
                                <label><%= GetGlobalResourceObject("Resource", "MaxWeightkg")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                           
                            <br />
                        </div>
                    </div>
                    <div class="row">
                    <div class="col m4"> 
                     <div class="flex">
                         
                            <select required="" id="ddlLocTypeBulk" class="textcss ddlLocTypeBulk" runat="server" style="width:100% !important;"></select>
                            <label> <%= GetGlobalResourceObject("Resource", "LocationType")%></label>
                        </div>
                    </div>
                    <div class="col m4">
                        <br />
                          <%= GetGlobalResourceObject("Resource", "CBM")%> : <span id="divCbmBulk"></span>                      
                    </div>
                        <div class="col m4 s4">
                            <div id="trFixedMaterialBulk">
                                <div class="flex">
                                    <input type="text" id="txtFixedmaterialcodeBulk" class="mcodepicker" onclick="getMMCodeListBulk();" onkeydown="getMMCodeListBulk();" required="" />

                                    <label><%= GetGlobalResourceObject("Resource", "FixedMaterialCode")%></label>
                                </div>
                            </div>
                        </div>
                </div>
                </div>
                <br />              
                <div class="">
                    <div class="flex__ alex____" style="justify-content: space-between; width: 50%;">
                        <div class="chk isActivess">
                          
                            <input type="checkbox" id="selectIsActiveBulk">
                            <label for="selectIsActiveBulk"></label>
                            <span data-tooltip="Active Location"></span>
                        </div>

                        <div class="chk Quarantine">
                            <input type="checkbox" id="selectIsQuarantineBulk">
                            <label for="selectIsQuarantineBulk"></label>
                            <span data-tooltip="Quarantine Location"></span>
                        </div>

                        <div class="chk mixedm">
                            <input type="checkbox" id="selectIsMMABulk" onchange="getselectMixedMaterial();">
                            <label for="selectIsMMABulk"></label>
                            <span data-tooltip="Allow Mixed Material"></span>
                        </div>

                        <div class="chk fastmoving">
                            <input type="checkbox" id="selectIsFMABulk">
                            <label for="selectIsFMABulk"></label>
                            <span data-tooltip="Fast-Moving Location"></span>
                        </div>
                    </div>
                </div>

               
                <div class="row" style="padding-top: 70px; text-align: center;">
                  <%--  <div id="trFixedMaterialBulk" class="col m4 s4">
                        <div class="flex">
                            <input type="text" id="txtFixedmaterialcodeBulk" class="mcodepicker" onclick="getMMCodeListBulk();" onkeydown="getMMCodeListBulk();" required="" />
                          
                            <label> <%= GetGlobalResourceObject("Resource", "FixedMaterialCode")%></label>
                        </div>
                    </div>--%>
                </div>
            </div>

<%--            <div class="row" style="padding-top: 70px; text-align: center;">
                <div id="trFixedMaterial" class="col m4 s4">
                    <div class="flex">
                        
                        <input type="text" id="txtFixedmaterialcode" class="mcodepicker" onclick="getMMCodeList();" onkeydown="getMMCodeList();" required="" />
                        <label>Fixed MaterialCode</label>
                    </div>
                </div>
            </div>--%>
            <div class="ui-dailog-footer">
                <div style="padding: 5px;">

                    
                    <a id="btnupCancelBulk" onclick="CloseDialog(updatebindialogBulk)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Cancel")%> </a>
                    <a onclick="UpdateLocationDetailsBulk()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Update")%></a>
                </div>
            </div>
        </div>


              <!-- ========================= Modal Popup For Bulk Print ========================================== -->
    <div class="modal inmodal" id="BulkPrintLocations" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="width: 50% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Print New Labels</h4>
                </div>

                <div class="modal-body">
                    <div class="row">

                        <div class="col m3">
                            <div class="flex">
                                <select id="ddlRackPrint" required="required" class="ddlRackPrint" onchange="ddlRackPrint_Change();"></select>
                                <label><%= GetGlobalResourceObject("Resource", "Rack")%></label>
                            </div>
                        </div>

                        <div class="col m3">
                            <div class="flex">
                                <select required="" id="ddlColumnPrint" class="ddlColumnPrint" onchange="ddlColLevPrint_Change();"></select>
                                <label><%= GetGlobalResourceObject("Resource", "ColumnsBay")%> </label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <select id="ddlLevelPrint" required="required" class="ddlLevelPrint" onchange="ddlColLevPrint_Change();"></select>
                                <label><%= GetGlobalResourceObject("Resource", "LevelBeam")%> </label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <select id="ddlBinPrint" class="ddlBinPrint" required=""></select>
                                <label><%= GetGlobalResourceObject("Resource", "Bin")%></label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a onclick="InitatePrint()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Print")%></a>
                    <%--<a onclick="CloseDialog(printlocationdialog)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Cancel")%> </a>--%>
                    <button type="button" class="btn btn-primary" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Cancel")%></button>
                </div>
            </div>
        </div>
    </div>
<!-- ========================= END Modal Popup For Bulk Print ========================================== -->

        <div id="modalConfirmYesNo" class="modal fade" aria-hidden="true" data-backdrop="static" data-keyboard="false" style="top:100px;">
    <div class="modal-dialog"  style="width: 25% !important;">
        <div class="modal-content">
            <div class="modal-header">
                <a  class="close" data-dismiss="modal" aria-label="Close">
                    &times;
                </a>
                <h4 id="lblTitleConfirmYesNo" class="modal-title"><%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left">
              
                <span style="font-size:larger;"><p id="lblMsgConfirmYesNo"></p></span>

                
            </div>
            <div class="modal-footer">               
                <button id="btnYesConfirmYesNo" 

                type="button" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Yes")%> <i class="fa fa-check" aria-hidden="true"></i></button>
                <button id="btnNoConfirmYesNo" 

                type="button" class="btn btn-primary" style="color:white !important;"><%= GetGlobalResourceObject("Resource", "No")%> <i class="fa fa-remove" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
</div>


   <!-- Adding Locations Alert-->
    <div id="Addinglocalert">
        <div style="padding: 10px;">
            <span id="Addinglocalertspan"></span>
        </div>
       
        <div class="ui-dailog-footer">
            <div style="padding: 5px;">
                <a onclick="AddingLocations()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "OK")%><span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(Addinglocalert)" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Cancel")%></a>               
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
        
      
        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="doDelete()" class="btn btn-primary" > <%= GetGlobalResourceObject("Resource", "OK")%> <span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(deletealert)" class="btn btn-primary" ><%= GetGlobalResourceObject("Resource", "Cancel")%></a>     
               
                
                     
            </div>
        </div>
        
    </div>
     <!--alert for create beam locations-->
        <div id="createbeamlocalert">
            <div style="padding:10px;">
                <span id="createbeamlocalertspan"></span>
            </div>
        
      

        <div class="ui-dailog-footer">
            <div style="padding: 15px 13px 15px 5px;">
                <a onclick="CreateBeamLocations()" class="btn btn-primary" ><%= GetGlobalResourceObject("Resource", "OK")%><span class="space fa fa-check-circle-o"></span></a>
                <a onclick="CloseDialog(createbeamlocalert)" class="btn btn-primary" ><%= GetGlobalResourceObject("Resource", "Cancel")%></a>
              
        
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
                <a onclick="CloseDialog(warningalert)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "OK")%><span class="space fa fa-check-circle-o"></span> </a>
              
            </div>
        </div>
    </div>
    </div>
   
    <div id="divMapContainer" style="width:100%; overflow:auto;">
        <br />
       
        <div id="divpnlChart" runat="server" style="float: left; width: 85%;">
            <br />
           
            <div id="divMap"></div>
            <div id="divMap1" runat="server" style="cursor:move;"></div>
        </div>
    </div>


    <div id="divTooltip">
        
    </div>
    
   </div>
   
                <div class="fix-flex-evenly">
                    <div class="flex-nows">
                        <div> <%= GetGlobalResourceObject("Resource", "Empty")%> </div>
                        <div class="EmptyBin cc"></div>
                    </div>
                    <div class="flex-nows">
                        <div> <%= GetGlobalResourceObject("Resource", "Filled")%>  </div>
                        <div class="bindataClass cc"></div>
                    </div>
                    <div class="flex-nows">
                        <div> <%= GetGlobalResourceObject("Resource", "InActive")%>  </div>
                        <div class="GreyLabel cc" style=""></div>
                    </div>
                    <div class="flex-nows" >
                        <div><%= GetGlobalResourceObject("Resource", "SearchFilled")%></div>
                        <div class="mCodeClass cc"></div>
                    </div>
                    <div class="flex-nows">
                        <div><%= GetGlobalResourceObject("Resource", "SearchAllocated")%> </div>
                        <div class="TenantClass cc"></div>
                    </div>


                    <div class="gmnoprint" controlwidth="28" controlheight="55" style="position: fixed;right: 10px;top: 54px;z-index:999;border:1px solid grey;">
                        <div draggable="false" style="user-select: none; box-shadow: rgba(0, 0, 0, 0.3) 0px 1px 4px -1px; border-radius: 2px; cursor: pointer; background-color: rgb(255, 255, 255); width: 28px; height: 83px;">
                            <button class="zoomIn" draggable="false"  aria-label="Zoom in" type="button" style="background: none; display: block; border: 0px; margin: 0px; padding: 0px; position: relative; cursor: pointer; user-select: none; width: 28px; height: 27px; top: 0px; left: 0px;">
                                <div style="overflow: hidden; position: absolute; width: 15px; height: 15px; left: 7px; top: 6px;">
                                    <i class="fa fa-plus"></i>
                                </div>
                            </button>
                            <div style="position: relative; overflow: hidden; width: 67%; height: 1px; left: 16%; background-color: rgb(230, 230, 230); top: 0px;">
                            </div>
                            <button class="zoomOut" draggable="false"  aria-label="Zoom out" type="button" style="background: none; display: block; border: 0px; margin: 0px; padding: 0px; position: relative; cursor: pointer; user-select: none; width: 28px; height: 27px; top: 0px; left: 0px;">
                                <div style="overflow: hidden; position: absolute; width: 15px; height: 15px; left: 7px; top: 6px;">
                                    <i class="fa fa-minus"></i>
                                </div>
                            </button>
                             <div style="position: relative; overflow: hidden; width: 67%; height: 1px; left: 16%; background-color: rgb(230, 230, 230); top: 0px;">
                            </div>
                            <button class="zoomOff" draggable="false"  aria-label="Zoom out" type="button" style="background: none; display: block; border: 0px; margin: 0px; padding: 0px; position: relative; cursor: pointer; user-select: none; width: 28px; height: 27px; top: 0px; left: 0px;">
                                <div style="overflow: hidden; position: absolute; width: 15px; height: 15px; left: 7px; top: 6px;">
                                   <i class="fa fa-circle"></i>
                                </div>
                            </button>
                        </div>
                    </div>
                </div>







    <!-- /navigation -->
    <div class="container" style="width:500px;display:none;">
        <div id="main">
            <div id="printer_data_loading" style="display: none">
                <span id="loading_message"><%= GetGlobalResourceObject("Resource", "LoadingPrinterDetails")%></span><br />
                <div class="progress" style="width: 100%">
                    <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                    </div>
                </div>
            </div>
            <!-- /printer_data_loading -->
            <div style="display:none;">
            <div id="printer_details" style="display: none">
                <span id="selected_printer"> <%= GetGlobalResourceObject("Resource", "Nodata")%></span>
                <button type="button" class="btn btn-success" onclick="changePrinter()"> <%= GetGlobalResourceObject("Resource", "Change")%></button>
            </div></div>
            <br />
            <!-- /printer_details -->
            <div class="row" style="display:none;">
                <div class="col m3">
                   
                    <!-- /printer_select -->
                </div>
                <div class="col m3" >
               
                    <div id="print_form" style="display: none" class="flex">
                        
                <input type="text" id="entered_name" />
                      <%--  <label>ZPL String</label>     --%>
                          <label> <%= GetGlobalResourceObject("Resource", "ZPLString")%> </label>     
                    </div>                    
                    <!-- /print_form -->
                </div>

                <div class="col m4 s4">
                    <button type="button" class="btn btn-lg btn-primary" onclick="sendData();" value="Print"><%= GetGlobalResourceObject("Resource", "PrintLabel")%> </button>
                </div>
            </div>



        </div>
        <!-- /main -->
        <div id="error_div" style="width: 500px; display: none">
            <div id="error_message"></div>
            <button type="button" class="btn btn-lg btn-success" onclick="trySetupAgain();"><%= GetGlobalResourceObject("Resource", "TryAgain")%> </button>
        </div>
        <!-- /error_div -->
    </div>
    <!-- /container -->

    <style>
        
        .TenantListboxPicker{
            background:transparent !important;
        }
.chk {
    position: relative;
    display: inline;
    user-select: none;
}

    .chk label {
        cursor: pointer;
        position: relative;
        left: -24px;
    }




        .chk label:before, .chk label:after {
            content: "";
            position: absolute;
            left: 0;
            top: 0;
            background: url(https://png.icons8.com/ios/50/27ae60/exercise-filled.png);
        }

    .chk input[type="checkbox"] {
        outline: 0;
        margin-right: 10px;
        opacity: 0;
    }

/* staging*/
.staging label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAOOSURBVFhH7ZdLSFVRFIaVsiIIKSSoQdMmBb1sVkQEWQ16gGAjy7eQUZNMEq7QexhJUPSgGpQNHBUVRTSz7AH2GDWoyKIIqTRDDbRvXf59Pefcc/Tc8mbB/eHnnL3Wv9Zerv24x7wccoiBqqqq5TU1NW08d8j0b6C0tHRadXX1QfiTAkdgj1yTj7q6uqUU1KXCjIOwRu7JQ0jXRnjvZ3mXSDJ5oJBl8JkrLMDJ6551jfkPBbr2lK6t4v2dbL1wgUL+HuyEUsxzFWG0vdacSCSmmh/fBsbD5uP9NqZ8s2cdDQ0N05k02LUncLEkKWA76zQw+0vNHCuYNLJrQeArhNlfanXtSJyuBcFWWI8+e0tN0mKSv3CFwTG7Fgb011x8bW3tdpn/DGFdg4+5iBdJMi7CTjlskPvPQNJ7nqSDjA9k0rWoU07RU3gWwAT+D/KF0fSdaDYppR84XzkxonUyjwvrPPrDMHK/YksteQza/i1V6CjowFaPqF3mMYFu3FNO3rUefwf6YxE8Afuk66moqJivFKPAcUUC60KZzGmI2K8Ws0uSFLDtc/76+vrZMocCzRZPrhsyjwJHEfwowWcOyFy5UsDuO+WMB3h+19juwEJJk2C8X74RmcYEuktOzw1QKfMoopY6omuddsqxVXlsFxSSBOOMCrQuk69bMeGXPUbfUvOXrOT50mMbgE2evZbP+Kbzo98oe8YFGtCWQHfZ38Xkv+xxFOH4ZALYy7u3a8OML/JsDPAkdJr3sjV6C1f6WEB/xsWFLjWObU4wUVTqWOAUzyLmjWK7ZPbDJZ4oKm1s0P2jFsdzSCY/XGITypQxiM94DzqMG+uc/2WB2Fdj38tzZ2Vl5RyZ04A/chK7Z8lhV9QeuylkTmGs2CSc01ugvljanU/8Yj9pkviAL3QSCtpMXvfTliTj8/ZxIYnFNjufTH7gsOvCAh+4LjFOuKAAe8J+yrCnFUi+ebDf2b3Evts0Np/NK1t3MjAIHC3BBB52KElZiC8TlmipvR8dQSZUkh847DuuzSNMkYSt0tjvd5o/DskxVF5ePsPy8H45TANt/gLTRIL9soYETdA+i5L/sPP8AVvhQ5eM91Mw+BmVRrTnXAy8j+00HFKOR9I12bwqIT4ILIZp+wfbVUniIJ+YW8Ec8CvLvVCa34ddCSS7Q1F98DVssdMtdywQPxMeJ/Yt/Aav25eR3DnkkENmyMv7BZgKMcTruTqDAAAAAElFTkSuQmCC);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.staging input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAPiSURBVFhH7VdJaBNhFI64IogoIuhBoTSZUCyotd5cKIJVDy5QcGv+SS05WdGLCxYquB/FIlTUoh60aWcipW4o6k2tC9Tl5EGlVRQpLrWiFazfm3n/ZGY6SSbdopAPHjPz3vfe//L9y0wCeeThA+GEKFE0tUlJCJVd/waK4hUTQpo4CPut6Gp/SBfdHMo9CpsrF6CpDmrMbE79BSVjHM4d3KoZzWmiN5gQ85mSO4Q1dSGaeSYbs1tO1TNVUw/ZVYM9DSbUJYouOukZsW9F8co5nDJ6oB2Kxf9cNkZrDdfaZXfrxlE8pEdWYQf/MZtUbwb6A2OMxJFG4bWaiW7V0OgTJaEWM8UCGjwjOaMy1Upz1aJ0qrlREI9NHZWpNlUTR/yo5obSoq4c0alG0VIo9SLZWHrVvAB+XOaH9egmdg8Nnqpp6uOwVjWPKRnhucs1UcPhoQFK3bEaM1QT+7NRLdUuD8QrxpY0xMbjuQ72XsbdxmO2BzWxhks6gV/9SpKDLdEV7M4IQ3ldHE63XuGzpjyjmeu3glOTCGqR9Taizu608LPLMeVltvh9qHTMyxA7gVo9Jk90K1eqZnOJJBC8JItB6o3sHgCv9UoW1tXtTLEQSojdMl7ctnkauz0B7rpkPXGV3UmEWmMzMOgHg6CJTwXa1pkcsuDe5eD9hO87F+2ks5CpBqDGXsllV1qAd0Hy0fA2dieRaqq9VRPttMvRYLX04b6RUwxk2yCpDG6XWSvFYY+gY6qDemQxlHopfaQapnOftdZwGMN/3cppUVcbfiDbBglorFwe9rjeHnDY01Qj+JEI9CscqpmJ56HUHqeJk5KD+3fSj2ercS7vCxjntFXPa6qh0AZJGC7j0r6AXTwF6r/h3A52O2EvPhzGZX0DKh6lPDTaxy4nrOIgsitrDGYNSmTMlcH/ssFgS2RpSIvuUrRotKhp23R2D0C6QeicxUaqxqbaSScFuy0MqkH6YoFPt2Jm/DMGKmOKA6kGwWG/FjHj1SYNz+fo44IpNH6tjLHLCTouDIImHkiVULhOJtkNxbu9XmVeDYa1LbNQu9eeb5kmdhCHxqNx2d9lJLoBwgFHss3oxU9F6CD3ivs1NFpuTLXto8NtJAq35AR9x2H6mrySYPXE4UPdK57R0FTf3EYxiepAjIteHBqf+iBOSigJsZxebcZnUfIP+w9YPQo8TBYTp4iTyVDjrMzB/T2s3wZq1njWxSPiGONhXG7BP1Cs1Hv9iMtMyQy8W9HIDY8aX/AGUZg1eBgfD7q4hUF60PBrWqu0uznsCyWtsclQ/zjW11tcv6LBtmz+/+SRRx4OBAJ/AUaWPhFDVMQbAAAAAElFTkSuQmCC);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}

/* de-nesting*/
.de-nesting label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAJhSURBVFhH7ZhNaxNRFIYTFUTEhR+I3UhRRBALgiCuXAlCV4JEXGRjvj/IIgtB3ARUsL9BsELpH/ADtBRF6FrcuehGXYmUiuCqjajP0XdgOky8tzMTE2geeJn0npz3vJ3emwnNTZggut3uvmazeTALyTI7arXaLfQD/cpC1Wp1NdOgGC7FDUqjSqVyXvbpqdfrpzB9FR2SUBvopqz9KZfLh7jtJ4rF4n4tbQHT26EhafRNlv7wJ6zT+FMGdl1m7aTKf2BtdAFpehgxMa0TckpvGW3Adrt9mMY7BJpDTwMzXs/rLaMNGCGPyXszI+Cq1sYqoO3JFTPj+klLFvASP2+GBiXVE1kmJy6gYVvBTnpS8XE1XSgUdssuOYMCjg2ugHYXqM3Y0yALcXe399hzBaT22OpZiTlr9qCQvRuPgMvhAWnFnD7X47J34wrYaDSOUn8WDEipdQ7PBVn74QpoUHsQGZRI/5oxkEnAkHZuQOr3w4OSihkfZOmPT0A7edTXogO3I/r76J4s/fEJOFII91IBv/Z6vV1aHh8IFj4Ai2josGVmGZ3/m8CBfa0i5Gf6tuyZYYuZFUVww+PsLA1v44yGJb7ZXNd4b/Lc+jMEvfwfNKOZybHvgbZXMLvBb3xEy96USqUD9BXQ1U6ns1fL2cBp3kOw18Gfg9df2AanVXbSarWO0fcx6EfvBv2TIBEEuhYyD7SgshP652L6OyqnB7NGxNzu4guVnfD+RzH9d1VOD3tvGsPvkQEllZ3Y3o309jm151TOBgwvYvwcraAmS34frIIeO1xvuC7hdUXLE3YyudxvKyQsVEhG6J8AAAAASUVORK5CYII=);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.de-nesting input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAKgSURBVFhH7ZjLaxNBHMfXB4iIBx+IgojU7M4arAhC8aQXQfAkaEW0zcxGqXroUSheAlrQv0FQQbw07OzWWtRQfIBn8eahF/UkUiqCJx9YvxN/kXSz6cw+YgLNB74UZna/v29357Wx+vQhdk+d2Tg4e35LHiLL/GABv8YC8QtaykNOwOdzDepIUYsrlEVuyA+TfXb2+8LGU3weVyipnEB8Z9LzyNqc4tTFrbbvDRysjWyipmXgtUzEFUwu/pUszWFSXIZ+1w3qf/lcISjto+463Q54J2qEQIuuvLCLLuluQDcY3YaQ13HzbUfymYYZQt2jS7obcBlL1hqEfafMEGqeWnsoIGCSv1ZmmHEfqcmy/dJRhPzRWjCZ4PmILNMTF1ChhoKa6WnFQr7Xqg6vI7v0tAvYM2gD4imwUAyq3SAPJd72dAGx9d1X/blJ8gW1UZC9Hl1AzMS5liIZhKXtZ7E6uofs9egCDsiRHTB9HC2URlgZFu2gNETWZmjHIMBaeStaLI1WqtGWfsAmrd6AWGomo8XSCD7vydIck4Bq5uG6hWjBJFLLC3STLM0xCdhVsDY9+/sfii9WpbKWmnuH5gmAp/gQ++VYp2X74qQ6i1KElVHHKgT71Aj5v4Q3doki6HFl+QBe9Zs4o07JCUpnqbwheOSFaa+Iw+bxTksd36hqBnAOVGPFlvycMzO2nVqNYdPlzXhSw07ITxWejG+g5nw49rKyHuPyRdMr+YyCjLq1FKt8J4bMh3/3S/G23Y8EqcD3xOmmcA09oG4t6pO25X7Jx6k7OyhwpaVAIJ5StxbsGHdj7r9B3dlRX2R4Rd8iBcrUrUWN3eZ71TZnh/wQdeeDK/kRmM+qrdAJvKvGCytRn1ySv8J6V2O+OEHNfVYzlvUHCQs3OcnJ9xMAAAAASUVORK5CYII=);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}

/* docking*/
.docking label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAJlSURBVGhD7ZjNLgNRFMf7GmInkQhbsfERCYkw7cyohSfwEp5DJBYW9jUtlUhY2NqJWIkgEdFgQVoJYjHuqXuM0aNz535Mb/FPfgn3njM5P3d6idx//krcijvoBP52vuw9O2XvNR/4++z7Eb7dHWFDj7Gh6/myH8YI/Bcm5vMyu9OUKHuNFglO83Rsl0mSQKyWEZVArJRJK4FYJSMrgVghoyqBdFRGlwTSERndEkimMqYkkExkTEsgRmWykkCMyMhIFLe8cPdkKgasUbU/oVVG9iQWq24Y3o/GgDWqth1aZFReJ10igJKM6mdCpwggJaPjg61bBEglo0MCMCECCMnokgBMiQBtZZxSsZ9tPlKNMpgUAUBmtuIO8/GjsNPYoBpkMS0CMJk9Pn4UJ/AvqWJZshBhb9AzHz8KEzmji+XI6EQafPwobHGFKhZhgf3pcXoxGTZqY5881eISAKx9rYEe6KWeKQL74Zf4+FFmNud72FHdUA0iLO0WmsN9H/4noBZ6qGeJwS6mUnGAjx9PYaswxAru6MZklg/mwrfbZBmogVrqGSLAKwW/KvjYdFRlVg9nyOG/AjVUrwhCEhhVmZ3jaVIAgD2qR4RUEhgVGbfih0dnky0SsAZ7VE8SUhIYFRm4aq+vJj4l4GvZ61dJAqMiA7dS/Wa8iewNpUUCoyIDt5PsDaVVAqN6AaTFiAQmKxmjEhjTMplIYEzJZCqB0S3TEQmMLpmOSmBUZayQwMjKWCWBSStjpQRGVMZqCUySTFdIYLhMrUUi8B+6RgLjVJ3efOCtffx3xjtnrDulYh/f/s8vTy73Di6NfAnaV+dGAAAAAElFTkSuQmCC);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.docking input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAJlSURBVGhD7ZjNLgNRFMf7GmInkQhbsfERCYkw7cyohSfwEp5DJBYW9jUtlUhY2NqJWIkgEdFgQVoJYjHuqXuM0aNz535Mb/FPfgn3njM5P3d6idx//krcijvoBP52vuw9O2XvNR/4++z7Eb7dHWFDj7Gh6/myH8YI/Bcm5vMyu9OUKHuNFglO83Rsl0mSQKyWEZVArJRJK4FYJSMrgVghoyqBdFRGlwTSERndEkimMqYkkExkTEsgRmWykkCMyMhIFLe8cPdkKgasUbU/oVVG9iQWq24Y3o/GgDWqth1aZFReJ10igJKM6mdCpwggJaPjg61bBEglo0MCMCECCMnokgBMiQBtZZxSsZ9tPlKNMpgUAUBmtuIO8/GjsNPYoBpkMS0CMJk9Pn4UJ/AvqWJZshBhb9AzHz8KEzmji+XI6EQafPwobHGFKhZhgf3pcXoxGTZqY5881eISAKx9rYEe6KWeKQL74Zf4+FFmNud72FHdUA0iLO0WmsN9H/4noBZ6qGeJwS6mUnGAjx9PYaswxAru6MZklg/mwrfbZBmogVrqGSLAKwW/KvjYdFRlVg9nyOG/AjVUrwhCEhhVmZ3jaVIAgD2qR4RUEhgVGbfih0dnky0SsAZ7VE8SUhIYFRm4aq+vJj4l4GvZ61dJAqMiA7dS/Wa8iewNpUUCoyIDt5PsDaVVAqN6AaTFiAQmKxmjEhjTMplIYEzJZCqB0S3TEQmMLpmOSmBUZayQwMjKWCWBSStjpQRGVMZqCUySTFdIYLhMrUUi8B+6RgLjVJ3efOCtffx3xjtnrDulYh/f/s8vTy73Di6NfAnaV+dGAAAAAElFTkSuQmCC);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}


/*fast moving*/
.fastmoving label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    /*background: url(data:image/png;base64,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);*/
    background: url(../Images/exercise-filled-inactive.png);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.fastmoving input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(../Images/exercise-filled-active.png);
    /*background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAO5SURBVFhH7Vg7bxNBEDYgARLPAkEJAiQg9AnxrnGTCMd3dkCRhUQNUgJ1AAmRlBR0vCoqlEgQoCPQUCBFIH5AipBIhJfvHCWYCCqgMN/czd2tz6895xJR+JNGPu/Mfjs3Ozu7e4kOOmiC5Xxyl2WKAduQV21TPLAMOUlCz9RGOrJh841BZTyx2TJTQ7Ypp+HAX/xWmgnbTNtm8nylUNjCNGuDZcpxfqxCyUjmEZm5sBO6Amfni6Y8y3TtwyULnFzJdO/G1D0JD+gJBl7F7/egTZS5rcpOkeeLg+m9TB8dHhE5aWV7D9aLmmWIj9Df+JZPHqskEpvc3PN0ctJJhezpE7C7if6f1b5ss1Ay5GEeMhpCRD/V/7YhlouGvEQOsLkDOyt74EjBETxzswM3b5PD6K9EmQIgrKXcqaNspg+VpEoM8aZopvexWWTY53r34wVmVE6KZLmvbw+b6EElUIhmZwtdW9mkbcxnMtvANxXin2K1HkKdfWm0uqOCXhTT+1blpgrB6tZQO4YlLidLuZ4D4FNX/odwXjeEn+wNJHLONADSZiRwEAJuVq0fyPmlTPII/20K2lngVFCCDPmSVfHDNlLdiMhD1L1fmK7r3NwSSJkxz0E8/1mXvRsJf8ePgiP6Dto5cVLtWzRSZ1gVHzA1txG9W4je66gOukVc3RZTo6yqRclMXYQR1ShfiqaQrG4JOMnbnZ6DxO2OI8puP6fe3mN1LbBTPPIMPSmZ4gKrWyKqg8QdHg8ywepaBAME8l85iAjeDwydsG/8FJvyLqtrYWfFtcBQrGpXdkZUBwmRFgkMBwJDLHmz9zirtNCOg5HKDF+C/HsGHTZZpYV2HKS93R8Phdru79/BqvoA+SuvA+RTlGn2Dq26kXe3OvElcFC8YFVj0ACKg1SXRlgVOxCxy1Vj4bbIqsagt8KbzPsdDblCRyNWxwbnuGWIH8o4c9qzRVdDvyMEUXwXx2naA52qEYT36hh2Tpqs1gNy8WkVAeoVEbO6bSym09sRuWcqNwLwmNX6oHsrOi6oRJiGGbr4sElk0LTSbKiclE5tH4DpSggCWyVEZHEpTw6H86XptZNWqymuVOWcy/W17buxByKoiSQJTsJYhWNWXnY5u4FfA50pcy/u0Dl1rs7FHW1z9mD6EA+zNvB0t/j0EeynbpQbf/ogLvqcwvTxwfl4hNtXvUG1hKIWdbVGBU2dk2ean99g+xu/sE0Nade5uLBUSO+0sjJDJxA+qk044jynRmnjJxs276CDWiQS/wAxTj3cfSniDgAAAABJRU5ErkJggg==);*/
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}

/*Is Mixed Material Allowed*/

.mixedm label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,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);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.mixedm input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,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);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}

/*Quarantine*/

.Quarantine label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,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);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.Quarantine input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,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);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}

/*isActive*/

.isActivess label:before {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAANrSURBVGhD7ZrLaxNRFMaz9i2+/iELwZnmzjQJLVRcuBCsXSiIutUarRt1IW5ciLSIjWSmhlIpiC8wRhPUpQVduLFFENuCrmzHcyZnUCbnZl53Ji7ywQ9KMvfc75t7586dSXMDDTRQdjJr5kHNMsu6ZVSAh8AL3TbaiGaL5/RZRZsvlPBYavZ/SNhij2YZk2CwBWyBaScUeKxlNLEt1qBy2Qs7ByPTYGSDNRoFy1gHKpkHgqlxTLfFN9ZUAuCkrGo1c5y6SU+F+cJOCDDLmVCLmC1XyzuoW7UafjR8CM7YO77jVGhjn9S9GuXnSgfgLC0znaWMWMa+yUYy0XT6wHeUAbAaKplmsP7fZzvIFDFDduKpszpxhbMn9mqmLWh74UwoX2LjgktzrPsM3uy4gmly+tVx50LrBPtdB3GF7IUTbTuS37EjgCGav687ra0bPcKItUijAiEm+ULp4IVoOzddAsJMkM1gwcFv+CLq8YfwuPXpLHs8rKKvyWZv4fYaro/wu9gEyELc/nzOGbZNtg0E2dTr+n6yK5f7PMEUUE2cEH8RRbIrF64MfGN1JAvhjsplsisXBKlyjVWRNAQCi9EDsisXXB8vucYco4tl5+jSGPsdh4oQCD42k125IMh7rrEfDPF4o+I8+TkdKoyqEAg+TpBdufAgrvG/eCE8M0FhVIZAQgUJM7XufDnfZUoW5kxDbQgk3NQKcbGXFoqO/f1ilzl/mDRCIGEv9grX2E9QmLRCIDAiU2RXrig3xE6YS11mn/66llqIDiFuiO7zeYQtiiyMH1UhcItiWMY+sttbEKTJFZERFEbdSLhBGmQzWHrNPMUV6YUsjMoQCFzoJ8lmsOh16DpXqBf+MCmE+BH5cReG8CpXLAgvjOoQCHgK3iz6RY+7q1zBIEbqReUhYKVayS/md5G9aMJXMHzRfiBGyVY8QYEZvnCGWMY9shNf+LoS5uZbtoMswFtBXd9GdpKpXy+x4Rr9qOwltid8xQ/F8fdAtlPV4CxQHsITTjMYmdR/6IGRuDu0NLSduk1PndVMrHAmkiG+Qu0x6iYb0d0ff4aOvAPoRqzBVJo6XC3vpvLZyw0EezMw0gA2eaPd4LHYBkJM9DUAJ3wDeMQyRvDsAnMwWs/AtPsPA/g3fobf4TGht+IDDTSQAuVyfwDdSxtBGQ/GPQAAAABJRU5ErkJggg==);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    filter: grayscale();
    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
    filter: grayscale(100%);
}

.isActivess input[type="checkbox"]:checked + label:after {
    width: 50px;
    height: 50px;
    background: #fff;
    cursor: pointer;
    left: 0px;
    background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAANrSURBVGhD7ZrLaxNRFMaz9i2+/iELwZnmzjQJLVRcuBCsXSiIutUarRt1IW5ciLSIjWSmhlIpiC8wRhPUpQVduLFFENuCrmzHcyZnUCbnZl53Ji7ywQ9KMvfc75t7586dSXMDDTRQdjJr5kHNMsu6ZVSAh8AL3TbaiGaL5/RZRZsvlPBYavZ/SNhij2YZk2CwBWyBaScUeKxlNLEt1qBy2Qs7ByPTYGSDNRoFy1gHKpkHgqlxTLfFN9ZUAuCkrGo1c5y6SU+F+cJOCDDLmVCLmC1XyzuoW7UafjR8CM7YO77jVGhjn9S9GuXnSgfgLC0znaWMWMa+yUYy0XT6wHeUAbAaKplmsP7fZzvIFDFDduKpszpxhbMn9mqmLWh74UwoX2LjgktzrPsM3uy4gmly+tVx50LrBPtdB3GF7IUTbTuS37EjgCGav687ra0bPcKItUijAiEm+ULp4IVoOzddAsJMkM1gwcFv+CLq8YfwuPXpLHs8rKKvyWZv4fYaro/wu9gEyELc/nzOGbZNtg0E2dTr+n6yK5f7PMEUUE2cEH8RRbIrF64MfGN1JAvhjsplsisXBKlyjVWRNAQCi9EDsisXXB8vucYco4tl5+jSGPsdh4oQCD42k125IMh7rrEfDPF4o+I8+TkdKoyqEAg+TpBdufAgrvG/eCE8M0FhVIZAQgUJM7XufDnfZUoW5kxDbQgk3NQKcbGXFoqO/f1ilzl/mDRCIGEv9grX2E9QmLRCIDAiU2RXrig3xE6YS11mn/66llqIDiFuiO7zeYQtiiyMH1UhcItiWMY+sttbEKTJFZERFEbdSLhBGmQzWHrNPMUV6YUsjMoQCFzoJ8lmsOh16DpXqBf+MCmE+BH5cReG8CpXLAgvjOoQCHgK3iz6RY+7q1zBIEbqReUhYKVayS/md5G9aMJXMHzRfiBGyVY8QYEZvnCGWMY9shNf+LoS5uZbtoMswFtBXd9GdpKpXy+x4Rr9qOwltid8xQ/F8fdAtlPV4CxQHsITTjMYmdR/6IGRuDu0NLSduk1PndVMrHAmkiG+Qu0x6iYb0d0ff4aOvAPoRqzBVJo6XC3vpvLZyw0EezMw0gA2eaPd4LHYBkJM9DUAJ3wDeMQyRvDsAnMwWs/AtPsPA/g3fobf4TGht+IDDTSQAuVyfwDdSxtBGQ/GPQAAAABJRU5ErkJggg==);
    background-size: 35px;
    background-repeat: no-repeat;
    background-position: center;
    background-color: transparent !important;
    border-color: transparent;
    box-shadow: 0 3px 5px -1px rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.14), 0 1px 18px 0 rgba(0,0,0,.12);
    border-radius: 100%;
    transition: box-shadow 0.3s ease-in-out;
}


.switch3D input[type=checkbox] {
  height: 0;
  width: 0;
  visibility: hidden;
}

.switch3D label {
    cursor: pointer;
    text-indent: -9999px;
    width: 60px;
    height: 36px;
    background: grey;
    display: block;
    border-radius: 100px;
    position: relative;
    box-shadow: var(--z1);
}

.switch3D label:after {
    content: '';
    position: absolute;
    top: 4px;
    left: 5px;
    width: 27px;
    height: 27px;
    background: #fff;
    border-radius: 90px;
    transition: 0.3s;
}

.switch3D input:checked + label {
  background: var(--sideNav-bg);
}

.switch3D input:checked + label:after {
  left: calc(100% - 5px);
  transform: translateX(-100%);
}

.switch3D label:active:after {
  width: 50px;
}



#divMap{
    -webkit-transition: -webkit-transform .6s;
    -moz-transition: -moz-transform .6s;
    transition: transform .6s;
}

.dRotate:after{
    content:'';
}



     .dRotate .spanBin{
            -webkit-box-shadow: 0 4px 5px 0 rgba(0, 0, 0, 0.14), 0 1px 10px 0 rgba(0, 0, 0, 0.12), 0 2px 4px -1px rgba(0, 0, 0, 0.3);
          box-shadow: 0 4px 5px 0 rgba(0, 0, 0, 0.14), 0 1px 10px 0 rgba(0, 0, 0, 0.12), 0 2px 4px -1px rgba(0, 0, 0, 0.3);
      }

        .dRotate .binSelect {
 
            box-shadow: 0 24px 38px 3px rgba(0, 0, 0, 0.14), 0 9px 46px 8px rgba(0, 0, 0, 0.12), 0 11px 15px -7px rgba(0, 0, 0, 0.2) !important;
      }

.dRotate{


        margin-bottom: 0;
    -webkit-transform: translateZ(0);
    -moz-transform: translateZ(0);
    -ms-transform: translateZ(0);
    -o-transform: translateZ(0);
    transform: translateZ(0);
    -webkit-backface-visibility: hidden;
    backface-visibility: hidden;
    -webkit-transform-style: preserve-3d;
    -moz-transform-style: preserve-3d;
    -ms-transform-style: preserve-3d;
    -o-transform-style: preserve-3d;
    transform-style: preserve-3d;
    -webkit-transform-origin: center top;
    -moz-transform-origin: center top;
    -ms-transform-origin: center top;
    -o-transform-origin: center top;
    transform-origin: center top;
    -webkit-transform: rotateX(-60deg) rotateZ(-40deg) translateX(50px) translateY(300px);
    -moz-transform: rotateX(-60deg) rotateZ(-40deg) translateX(50px) translateY(300px);
    -ms-transform: rotateX(-60deg) rotateZ(-40deg) translateX(50px) translateY(300px);
    -o-transform: rotateX(-60deg) rotateZ(-40deg) translateX(50px) translateY(300px);
    transform: rotateX(-60deg) rotateZ(-40deg) translateX(50px) translateY(300px);
    -webkit-transition: -webkit-transform .6s;
    -moz-transition: -moz-transform .6s;
    transition: transform .6s;

    position: relative;
    width: 1000px;
    left: -50%;
    zoom: 1.5;
    top: -110px;
}

/*.zoom3{
   left:50% !important;
   zoom:3;
}*/

.dRotate .spanBin{
    position:relative
}

.dRotate .spanBin::after{
    content:'';
    display:block;
    position:absolute;
}

        .partNumber {
  /*white-space: nowrap !important; 
  width: 200px !important; 
  overflow: hidden !important;
  text-overflow: ellipsis !important;*/ 

   overflow-wrap: break-word;
  word-wrap: break-word;

  -ms-word-break: break-all;
  /* This is the dangerous one in WebKit, as it breaks things wherever */
  word-break: break-all;
  /* Instead use this non-standard one: */
  word-break: break-word;

  /* Adds a hyphen where the word breaks, if supported (No Blink) */
  -ms-hyphens: auto;
  -moz-hyphens: auto;
  -webkit-hyphens: auto;
  hyphens: auto;

}
            .partNumber span {
                color: #2196F3 !important;
    font-size: 7Pt !important;
            }

    </style>

    <script>
        (function () {
           function validate() {
               if (document.getElementById('switch').checked) {
                   document.querySelector('#divMap').classList.add('dRotate');
                   document.querySelector('#MainContent_MMContent_divpnlChart').classList.add('zoom3');
                } else {
                   document.querySelector('#divMap').classList.remove('dRotate');
                   document.querySelector('#MainContent_MMContent_divpnlChart').classList.remove('zoom3');
                }
            }

            document.getElementById('switch').addEventListener('change', validate);
        })();
    </script>
    <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mMaterialManagement/LocationWebClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mMaterialManagement/LocationWebClientPrintDemo.ashx", "locationManager")%>
    <%-- <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/ShipperID/mMaterialManagement/LocationWebClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +  "/ShipperID/mMaterialManagement/LocationWebClientPrintDemo.ashx", "locationManager")%>--%>
</asp:Content>
