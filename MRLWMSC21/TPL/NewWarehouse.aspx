<%@ Page Title=" Warehouse Create :." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewWarehouse.aspx.cs" Inherits="MRLWMSC21.TPL.NewWarehouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="NewWarehouse.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB-amPYw4EvJGyYfY16HzhF2lqpw--FcHM&libraries=places"></script>
    <link href="tpl.css" rel="stylesheet" />

    <script type="text/javascript">
        function filterDigits(eventInstance) {
            eventInstance = eventInstance || window.event;
            key = eventInstance.keyCode || eventInstance.which;
            if ((47 < key) && (key < 58) || key == 8)
            {
                return true;
            }
            else
            {
                if (eventInstance.preventDefault)
                    eventInstance.preventDefault();
                eventInstance.returnValue = false;
                return false;
            }
        }
    </script>
    <style>
        .blockStyle {
            height: 13px;
            background-color: red;
            padding: 6px;
            position: relative;
            border-radius: 100%;
            border: 1px solid #FFFFFF;
            box-shadow: 1px 2px 3px #CCCCCC;
        }

        .tooltip_ {
            position: relative;
        }

        .tooltip__ {
            position: absolute;
            background-color: #333;
            padding: 5px;
            /*border:3px solid #4f91f9;*/
            border-radius: 8px;
            color: #fff;
            width: 22vw;
            margin-top: -55px;
            margin-left: -20px;
            opacity: 0;
            visibility: hidden;
            text-align: left;
            /*box-shadow:1px 2px 3px #808080;*/
        }

        .tooltip_:hover .tooltip__ {
            visibility: visible;
            opacity: 1;
            z-index: 9999;
        }






        /* Tooltip */
        .tooltip {
            display: inline;
            position: relative;
        }

            .tooltip:hover:after {
                background: #333;
                background: rgba(0,0,0,.8);
                border-radius: 5px;
                bottom: 26px;
                color: #fff;
                content: attr(title);
                left: 20%;
                padding: 5px 15px;
                position: absolute;
                z-index: 98;
                width: 220px;
            }

            .tooltip:hover:before {
                border: solid;
                border-color: #333 transparent;
                border-width: 6px 6px 0 6px;
                bottom: 20px;
                content: "";
                left: 50%;
                position: absolute;
                z-index: 99;
            }



        .tool-tip {
            display: inline-block;
        }

        .tool [data-tooltip]:hover:before,
        [data-tooltip]:hover:after {
            visibility: visible;
            opacity: 1;
        }

        .tool span {
            position: absolute;
            right: 0px;
        }

            .tool span:first-child {
                display: none !important;
            }

        .tool [data-tooltip]:after {
            bottom: 100%;
        }

        .tool [data-tooltip]:before {
            bottom: 100%;
        }
    </style>
    <div class="module_yellow">
        <div class="ModuleHeader">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Master Data</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">New Warehouse </span></div>
        </div>
    </div>
    <div class="pagewidth">
    <div class="angulardiv" ng-app="MyApp" ng-controller="NewWarehouse">
         <div flex end>
                                    <button type="button" ng-click="displaylist()" class=" btn btn-primary  backtolist"><i class="material-icons vl">arrow_back</i> <%= GetGlobalResourceObject("Resource", "BackToList")%></button>
                                </div>  
        <gap5></gap5>
        <div>
            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvTDHeader" style=""> <%= GetGlobalResourceObject("Resource", "WarehouseCreation")%></div>
            <div class="ui-Customaccordion" id="dvTDBody">
                <table class="internalData" width="100%">
                    <tr>
                        <td>
                            <div>
                                <%--<div style="float: right; margin: 5px;">
                                    <button type="button" ng-click="displaylist()" class="addbuttonOutbound btn btn-primary"> <%= GetGlobalResourceObject("Resource", "BackToList")%></button>
                                </div>--%>
                                <%--<div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader">Warehouse Creation </div>--%>
                                <%--<div class="ui-Customaccordion" id="PrimaryInformationBody">--%>
                            
                                <div class="row" style="">
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                            </div>
                                            <div>
                                                <select ng-model="warehousedata.AccountId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in AccountData" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "Account")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" id="Text17" ng-model="warehousedata.WHName" required="" maxlength="50" />
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" id="Text18" ng-model="warehousedata.WHCode" required="" maxlength="3" onkeypress="return blockSpecialChar(event)"  />
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "WHCode")%></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                            </div>
                                            <div>
                                                <select ng-model="warehousedata.RackingRType" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in racktypes" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <label>  <%= GetGlobalResourceObject("Resource", "RackingType")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <select ng-model="warehousedata.WHtype" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in whtypes" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <label> <%= GetGlobalResourceObject("Resource", "WHType")%> </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" ng-model="warehousedata.WHAddress" required="" maxlength="100" />
                                                <label> <%= GetGlobalResourceObject("Resource", "WHAddress")%> </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div style="display: none !important;">
                                                <select ng-model="warehousedata.Inout" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in inouts" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                   <span class="errorMsg"></span>
                                                <label><%= GetGlobalResourceObject("Resource", "InOut")%> </label>
                                             
                                            </div>
                                            <div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="txtfloorspace" ng-model="warehousedata.FloorSpace" required="" max="2,147,483,647"  />
                                                <label><%= GetGlobalResourceObject("Resource", "FloorSpace")%> </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="Text21" ng-model="warehousedata.FloorSpace" required="" />
                                                <label>Floor Space(Sq. Mt.)</label>
                                            </div>--%>
                                            <div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="txtlength" ng-model="warehousedata.Length" required="" max="2,147,483,647" />
                                                <label>  <%= GetGlobalResourceObject("Resource", "Length")%> </label>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%-- <div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="Text22" ng-model="warehousedata.Length" required="" />
                                                <label>Length </label>
                                            </div>--%>
                                            <div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="txtwidth" ng-model="warehousedata.Width" required="" max="2,147,483,647" />
                                                <label> <%= GetGlobalResourceObject("Resource", "Width")%></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="Text24" ng-model="warehousedata.Width" required="" />
                                                <label>Width</label>
                                            </div>--%>
                                            <div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="txtheight" ng-model="warehousedata.Height" required="" max="2,147,483,647" />
                                                <label> <%= GetGlobalResourceObject("Resource", "Height")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <input type="number" min="0" oninput="validity.valid||(value='');" id="Text23" ng-model="warehousedata.Height" required="" />
                                                <label>Height</label>
                                            </div>--%>
                                            <div>
                                                <select ng-model="warehousedata.Country" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in countrynames" ng-change="getCurrency(0)" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "Country")%> </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <select ng-model="warehousedata.Country" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in countrynames" ng-change="getCurrency(0)" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>Country </label>
                                            </div>--%>
                                            <div>
                                                <select ng-model="warehousedata.StateId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in Statesdata" ng-change="getCities(0)" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label><%= GetGlobalResourceObject("Resource", "State")%></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <select ng-model="warehousedata.StateId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in Statesdata" ng-change="getCities(0)" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>State</label>
                                            </div>--%>
                                            <div>
                                                <select ng-model="warehousedata.CityId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in CityData" ng-change="getZipCode(0)" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label><%= GetGlobalResourceObject("Resource", "City")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <select ng-model="warehousedata.CityId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in CityData" ng-change="getZipCode(0)" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>City</label>
                                            </div>--%>
                                            <div>
                                                <select ng-model="warehousedata.ZipCodeId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in ZipCodeData" ng-change="getlatitudelongitude()" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "ZipCode")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <select ng-model="warehousedata.ZipCodeId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in ZipCodeData" ng-change="getlatitudelongitude()" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>Zip Code</label>
                                            </div>--%>
                                            <div>
                                                <select ng-model="warehousedata.Currency" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in currencynames" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "Currency")%></label>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m4 s4">
                                     <div class="flex">
                                            <%--<div>
                                                <input type="text" id="txtlatitude"  disabled="true" ng-model="warehousedata.Langitude" readonly />
                                                <label>Langitude</label>
                                            </div>--%>
                                         <div></div>
                                         <select ng-model="warehousedata.Time" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in timepreference" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "TimeZone")%></label>
                                         </div>
                                        </div>
                                   
                                    <div class="col m4 s4">
                                        <%--<div class="flex">
                                            <div>
                                                <input type="text" id="txtlangitude" disabled="true" ng-model="warehousedata.Latitude" readonly />
                                                <label>Latitude </label>
                                            </div>
                                        </div>--%>
                                        <div class="flex">
                                                <input type="text" id="Text19" ng-model="warehousedata.Location" required="" maxlength="50">
                                                <label> <%= GetGlobalResourceObject("Resource", "Location")%></label>
                                            </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <%--<div>
                                                <select ng-model="warehousedata.Currency" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in currencynames" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>Currency</label>
                                            </div>--%>
                                            <div>
                                                <%--<select ng-model="warehousedata.Time" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in timepreference" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>Time Zone</label>--%>
                                            </div>
                                        </div>
                                        <div class="flex">

                                           

                                                <input type="checkbox" id="Chkisactive"  ng-click="getchkvalue()">
                                                 <span><%= GetGlobalResourceObject("Resource", "Active")%></span>
                                            

                                        </div>
                                    </div>
                                </div>
                 <%--               <div class="row">
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <div>
                                                <select ng-model="warehousedata.Time" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in timepreference" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <label>Time Zone</label>
                                            </div>
                                            <div>
                                                <input type="text" id="Text19" ng-model="warehousedata.Location" required="">
                                                <label>Location</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">

                                            <div class="">
                                                <input type="checkbox" id="Chkisactive"  ng-click="getchkvalue()" ng-checked="true">
                                                Active
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="ui-SubHeading ui-SubHeadingBar" id="PContactHeader">  <%= GetGlobalResourceObject("Resource", "PrimaryContactPerson")%> </div>
                                <div class="ui-Customaccordion" id="PContactBody">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text31" ng-model="warehousedata.pName" required="" maxlength="50"/>
                                                    <label>Name </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text32" ng-model="warehousedata.Pmobile" required="" maxlength="10" onkeypress="filterDigits(event)" />
                                                    <label><%= GetGlobalResourceObject("Resource", "Mobile")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text33" ng-model="warehousedata.pEmail" required="" maxlength="50">
                                                    <label> <%= GetGlobalResourceObject("Resource", "Email")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text34" ng-model="warehousedata.PAddress" required="" maxlength="100">
                                                    <label><%= GetGlobalResourceObject("Resource", "Address")%></label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="ui-SubHeading ui-SubHeadingBar" id="SContactHeader"><%= GetGlobalResourceObject("Resource", "SecondaryContactPerson")%> </div>
                                <div class="ui-Customaccordion" id="SContactBody">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text27" ng-model="warehousedata.sname" required="" maxlength="50"/>
                                                    <label><%= GetGlobalResourceObject("Resource", "Name")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input id="Text28" type="text" onkeypress="filterDigits(event)" ng-model="warehousedata.SMobile" required="" maxlength="10">
                                                    <label> <%= GetGlobalResourceObject("Resource", "Mobile")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text29" ng-model="warehousedata.SEmail" required="" maxlength="50">
                                                    <label><%= GetGlobalResourceObject("Resource", "Email")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="Text30" ng-model="warehousedata.SAddress" required="" maxlength="100">
                                                    <label><%= GetGlobalResourceObject("Resource", "Address")%></label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--</div>--%>
                                
                                <div class="row">
                                    <gap5></gap5>
                                    <div flex end>
                                        <button type="button" id="btncreatewh" ng-click="CreateNewWareHouse()" class="addbuttonOutbound btn btn-primary"> <%= GetGlobalResourceObject("Resource", "CreateWareHouse")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %> </button>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <!--- Dock Creation --->
            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvTDDockHeader" style=""> <%= GetGlobalResourceObject("Resource", "DockCreation")%> </div>
            <div class="ui-Customaccordion" id="dvTDDockBody">
                <div class="row">
                    <div class="col s12">
                        <gap5></gap5>
                        <div flex end>
                            <button type="button" id="btncreatedc" data-ng-click="getDocData(null)" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "CreateDock")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %> </button>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col s12">
                        <table class="table-striped ">
                            <tr>
                                <th>  <%= GetGlobalResourceObject("Resource", "Warehouse")%></th>
                                <%--<th> <%= GetGlobalResourceObject("Resource", "DockNo")%></th>--%>
                                <th>Dock</th>
                                <%--<th>  <%= GetGlobalResourceObject("Resource", "DockName")%></th>--%>
                                <th> <%= GetGlobalResourceObject("Resource", "DockType")%></th>
                                <th>Dock PDF </th>
                                <th>  <%= GetGlobalResourceObject("Resource", "Edit")%></th>
                            </tr>
                            <tr ng-repeat="info in DockListdata">
                                <td>{{info.WarehouseName}}</td>
                                <td>{{info.DockNumber}}</td>
                                <%--<td>{{info.DockName}}</td>--%>
                                <td>{{info.DockType}}</td>
                                <td  class="rolehide">
                                <div>
                                    <span id="pdfDownload" ng-click="PDFDownload(info.DockID)"  ><i class='material-icons' >picture_as_pdf</i><em class='sugg-tooltis' style='left:-100px;'>Download PDF</em></span>
                                </div>
                                </td>
                                <td>
                                    <div style="cursor: pointer;" data-ng-click="getDocData(info)"><i class="material-icons ss">edit</i><em class="sugg-tooltis">Edit</em></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- Modal -->
                <div id="DockModal" class="modal fade">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                <h4 class="modal-title" style="display: inline !important;"> <%= GetGlobalResourceObject("Resource", "AddDock")%></h4>
                                <button type="button" data-dismiss="modal" style="cursor:pointer" class="pull-right modalclose" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mySupForm">
                                <div class="row">
                                    <div class="col s12">
                                        <div class="col m4 s4">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="warehousenames" ng-model="warehousename" required="" readonly />
                                                    <input type="hidden" id="warehouseid" ng-model="warehouse" />
                                                    <span class="errorMsg"></span>
                                                    <label> <%= GetGlobalResourceObject("Resource", "Warehouse")%> </label>
                                                    <%--<select id="warehouseid" ng-model="warehouse" class="DropdownGH" ng-options="WH.ID as WH.Name for WH in warehouselist" required="">
                                                        <option value="" selected hidden></option>
                                                    </select>
                                                    <span class="errorMsg"></span>
                                                    <label>Select Warehouse</label>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m4 s4">
                                            <div class="flex">
                                                <input type="text" id="docknum" ng-model="docnumber" maxlength="8"   required="" />
                                                <span class="errorMsg"></span>
                                                <%--<label> <%= GetGlobalResourceObject("Resource", "DockNumber")%></label>--%>
                                                <label>Dock</label>
                                            </div>
                                        </div>
                                        <%--<div class="col m4 s4">
                                            <div class="flex">
                                                <input type="text" id="dockname" ng-model="docname" maxlength="10" required="" />
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "DockName")%></label>
                                            </div>
                                        </div>--%>
                                        <div class="col m4 s4" style="padding-left: 15px !important;">
                                        <div class="flex">
                                            <select class="DropdownGH" id="doctype" ng-model="doctype" required="" ng-options="DC.ID as DC.Name for DC in doctypelist">
                                                <option value="" selected hidden />
                                            </select>
                                            <span class="errorMsg"></span>
                                            <label> <%= GetGlobalResourceObject("Resource", "DockType")%> </label>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                                <div class="row" style="display:none !important;">
                                    <div class="col m4 s4" style="padding-left: 15px !important;">
                                        <div class="flex">
                                            <select class="DropdownGH" id="doctype" ng-model="doctype" required="" ng-options="DC.ID as DC.Name for DC in doctypelist">
                                                <option value="" selected hidden />
                                            </select>
                                            <span class="errorMsg"></span>
                                            <label> <%= GetGlobalResourceObject("Resource", "DockType")%> </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" flex end>
                                <input type="hidden" id="DockID" ng-model="dockids" />
                                <%--<input type="reset" value="reset" />--%>
                                <button type="button" class="btn btn-primary" style="color: #fff !important;" ng-click="myDockclear();"> <%= GetGlobalResourceObject("Resource", "Clear")%></button>
                                <button type="button" class="btn btn-primary" style="color: #fff !important;" data-dismiss="modal"> <%= GetGlobalResourceObject("Resource", "Close")%></button>
                                &nbsp;<div><img src="../Images/bx_loader.gif" id="imgloader" style="width:70px;display:none;" />
                                <button type="button" id="btnsavedck" class="btn btn-primary" ng-click="CreateNewDock()">  <%= GetGlobalResourceObject("Resource", "Save")%> </button></div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal -->
            </div>
            <!--- Dock Creation --->
            <!--- Zones Creation -->
            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvTDZoneHeader" style=""> <%= GetGlobalResourceObject("Resource", "ZoneCreation")%> </button> </div>
            <div class="ui-Customaccordion" id="dvTDZoneBody">
                <div class="row">
                    <gap5></gap5>
                    <div class="col s12">
                        <div flex end>
                            <button type="button" id="btncreatezn" data-ng-click="getZoneData(null)" class="btn btn-primary" ng-click="myZoneclear()"> <%= GetGlobalResourceObject("Resource", "CreateZone")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %> </button>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col s12">
                        <table class="table-striped ">
                            <tr>
                                <th> <%= GetGlobalResourceObject("Resource", "Warehouse")%></th>
                                <th> <%= GetGlobalResourceObject("Resource", "ZoneCode")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "Description")%></th>
                            <%--    <th><%= GetGlobalResourceObject("Resource", "Active")%></th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "Edit")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "Delete")%></th>
                                <th></th>
                            </tr>
                            <tr ng-repeat="info in ZoneListdata">
                                <td>{{info.WarehouseName}}</td>
                                <td>{{info.ZoneCode}}</td>
                                <td>{{info.ZoneDesc}}</td>
                               <%-- <td>{{info.IsActive==1?"Yes":"No"}}</td>--%>
                                <td>
                                    <%--<div id="dveditzone" class="tooltip"  ng-if="info.IsDockZone=='True'" data-tooltip="this Zone is ISDockZone." style="cursor: pointer;" data-ng-click="getZoneData(info)"><i class="material-icons ss">mode_edit</i></div>--%>

                                    <%--<div class="tooltip_">
                                        
                                        <div id="dveditzone" ng-if="info.IsDockZone=='True'" style="cursor: pointer;" data-ng-click="getZoneData(info)"><i class="material-icons ss">mode_edit</i></div>
                                        <div class="tooltip__">
                                            <div style="text-align: center !important; font-size: 14px;">Could not change since IsDock Zone</div>
                                        </div>

                                    </div>--%>


                                    <div id="dveditzone" style="cursor: pointer;" data-ng-click="getZoneData(info)"><i class="material-icons ss">edit</i><em class="sugg-tooltis">Edit</em></div>
                                </td>
                                <td>
                                    <%--   <span ng-click="deleteWareHouseData(info.WarehouseID)"><i class="material-icons ss">mode_delete</i></span>--%>
                                    <%--                     <button type="button" id="btncreatenew" ng-show="CreateWareHouseShow" ng-click="GoToList()" class="addbuttonOutbound btn btn-primary"  Title="Additional warehouses cannot be created as the Warehouse Limit is reached.">Create New <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
                                    <div style="cursor: pointer;" data-ng-click="deleteZoneData(info)"><i class="material-icons ss">delete</i><em class="sugg-tooltis">Delete</em></div>
                                </td>

                                <td ng-if="info.IsDockZone!='True'">
                                    <button type="button" id="btnaddLoc" class="btn btn-primary" ng-click="AddLoc(info)"><%=MRLWMSC21Common.CommonLogic.btnfaNew %> <%= GetGlobalResourceObject("Resource", "AddLoc")%> </button>
                                </td>
                                <td ng-if="info.IsDockZone=='True'"></td>

                            </tr>
                        </table>
                    </div>
                </div>
                <!-- Modal -->
                <div id="ZoneModal" class="modal fade">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                <h4 class="modal-title" style="display: inline !important;"> <%= GetGlobalResourceObject("Resource", "AddZone")%> </h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" style="cursor:pointer;" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="myZoneForm">
                                <div class="row">
                                    <div class="col s12">
                                        <div class="col m4 s4">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" id="warehousenamesz" ng-model="warehousename" required="" readonly />
                                                    <input type="hidden" id="warehouseidz" ng-model="warehouse" />
                                                    <span class="errorMsg"></span>
                                                    <label> <%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m4 s4">
                                            <div class="flex">
                                                <input type="text" id="zncode" ng-model="zonecode" required="" maxlength="2" Title="Cannot Edit the Zone Code as Locations are mapped to this Zone." />
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "ZoneCode")%></label>
                                            </div>
                                        </div>
                                        <div class="col m4 s4">
                                            <div class="flex">
                                                <textarea id="zndesc" required="" maxlength="150" ng-model="zonedesc" Title="Cannot Edit the Zone Description as Locations are mapped to this Zone."></textarea>
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "ZoneDescription")%></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <input type="hidden" id="ZoneID" ng-model="zoneids" />
                                <%--<input type="reset" value="reset" />--%>
                                <button type="button" class="btn btn-primary" style="color: #fff !important;" ng-click="myZoneclear();"> <%= GetGlobalResourceObject("Resource", "Clear")%></button>
                                <button type="button" class="btn btn-primary" style="color: #fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%> </button>
                                <button type="button" class="btn btn-primary" ng-click="CreateNewZone()"> <%= GetGlobalResourceObject("Resource", "Save")%>  </button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal -->
            </div>
            <!--- Zones Creation -->
        </div>
    </div>
</div>
</asp:Content>
