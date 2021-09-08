<%@ Page Title="Group Delivery PickNote:." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="GroupDeliveryPickNote.aspx.cs" Inherits="MRLWMSC21.mOutbound.GroupDeliveryPickNote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <script src="../mInventory/Scripts/angular.min.js"></script>

    <style>
        [align="right"] {
        text-align:center !important;
        }
    </style>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="GroupDeliveryPickNote.js"></script>
  <%--  <script type="text/javascript" src='<%=ResolveUrl("~/mMisc/Scripts/angularjs-dropdown-multiselect.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/mMisc/Scripts/underscore-min.js") %>'></script>--%>
    <script src="../mInventory/Scripts/dirPagination.js"></script>

    <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <div class="angulardiv" ng-app="MyApp" ng-controller="pickitemslist">
      

        <div style="font-weight:bold;">
          <%--<input type="checkbox" ng-model="issorterimport" ng-change="checksorterdatawithavaibleQty()"  />Import From Sorter (For HH Items)</div><div class="lineheight"></div>--%>
       
            <table style="width:30%">
                <tr>
                    <td>
                        <label>{{VLPDTitle}}</label>: {{VLPDNumber}}
                    </td>
                    <td><label>Store</label>: {{WHCode}}</td>
                    
                </tr>
            </table>
            <table class="table-striped">
                <thead>
                    <tr class="mytableOutboundHeaderTR">
                        <th>Line #</th>
                        <th>Part No#</th>
                        <th>Description</th>
                        <th>Location</th>
                        <%--  <th style="width:15%;">CCode</th>--%>
                        <th>Mfg. Date</th>
                        <th>Exp. Date</th>
                        <th>Batch No.</th>
                        <th>Serial No.</th>
                        <th>Project Ref. No.</th>
                        <th>MRP</th>
                        <th>Assigned Qty.</th>
                        <th>Picked Qty.</th>
                        <th>Pending Qty.</th>
                        <th>Pick</th>
                    </tr>
                </thead>

                <tbody>
                    <tr ng-repeat="MATERIALINFO in DeliveryList">
                        <td class="alignnumbers">{{$index + 1}}  </td>

                        <td>{{ MATERIALINFO.MCode }}</td>
                        <td>{{ MATERIALINFO.MDescription }}</td>
                        <td class="aligntext">{{ MATERIALINFO.Location }}</td>
                        <%-- <td style="width:15%;" class="aligntext">{{ MATERIALINFO.CartonCode }}</td>--%>
                        <td class="aligntext">{{ MATERIALINFO.MfgDate }}</td>
                        <td class="alignnumbers">{{ MATERIALINFO.ExpDate }}</td>
                        <td class="alignnumbers">{{ MATERIALINFO.BatchNo }}</td>
                        <td class="alignnumbers">{{ MATERIALINFO.SerialNo }}</td>
                        <td class="alignnumbers">{{ MATERIALINFO.ProjectRefNo }}</td>
                        <td class="alignnumbers">{{ MATERIALINFO.MRP }}</td>
                        <td align="right" class="alignnumbers">{{MATERIALINFO.AssiginQty}}</td>
                        <td align="right" class="alignnumbers">{{MATERIALINFO.PickedQty}}</td>
                        <td align="right" class="alignnumbers">{{MATERIALINFO.PendingQty}}</td>
                        <td class="alignnumbers">
                            <a style="text-decoration: none;" ng-click="openDialog('Add New Revision',DeliveryList,MATERIALINFO.AssignID);" target="_blank" href=""><span class="material-icons vl">touch_app</span></a>
                        </td>
                    </tr>
            </table>
  

        <div id="divContainer" class="PopupContainerInbound" style="display:none" style="height:300px;">
            <div id="divInner" class="PopupInnerOutbound" style="width:47%;">
                <div class="PopupHeadertextOutbound">Picking Item</div>
                    <span id="spanClose" class="fa fa-times PopupSpanCloseOutbound" aria-hidden="true"></span>&emsp;
                        <div class="PopupPaddingOutbound">
                            <div class="PopupSpaceOutbound">
                                <br />

                                    <div class="row">
                                        
                                        <div class="col m4">
                                            <div class="flex" >
                                                <input type="number" id="txtqty" ng-model="PickRequestedQty" min="0" class=""  style="width: 100% !important;" required="" />
                                                <span class="errorMsg"></span>
                                                <label>Enter Quantity     </label>
                                            </div>
                                            &nbsp;&nbsp;
                                        </div>
                                        <div class="col m4">
                                            <div class="flex" >
                                                <%--  <asp:TextBox ID="txtcontainer" class="txtcontainer" runat="server" required=""></asp:TextBox>--%>
                                                <input type="text" id="txtcontainer" class="dr" ng-model="container" ng-change="containerList()" ng-keyup="containerList()" style="width: 100% !important;" required="">
                                              <%--  <span class="errorMsg"></span>--%>
                                                <label>Intermediate Container</label>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                            </div>
                                            &nbsp;&nbsp;
                                        </div>

                                        <div class="col m4">
                                       
                                            <div class="flex__">
                                                <button type="button" id="btnCreateInbound" ng-click="pickItem();" class="btn btn-primary" style="width: 86px;">Pick <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
                                                &nbsp; &nbsp;
                                                <button type="button" id="btnClose" class="btn btn-primary" style="width: 86px;">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>
                                            </div>
                                        </div>
                                    </div>
                                <table class="tablestyle"> </table>
                                                               
                                <%-- <div style="text-align:left;color:Green;margin-left:1%;font-family:Arial;font-size:11pt;font-weight:bold;">Picked Items</div>
                                 <div class="lineheight"></div>
                                 <table class="mytablechildOutbound" style="width:98%;">
                                                    <thead>
                                                        <tr class="mytableOutboundchildHeaderTR">                                                       
                                                            <th>Item</th>
                                                            <th>Location</th>
                                                            <th>Batchno</th> 
                                                            <th>Qty</th>                                                                                         
                                                        </tr>
                                                    </thead>
                                                    <tbody class="mytableOutboundBodyTR">
                                                       <%--  <tr ng-repeat="GroupRequiredItemsInfo in RequiredItems" class="mytableOutboundBodyTR">--%>
                                                         <%-- <tr class="mytableOutboundBodyTR">
                                                            <td class="aligntext">INV998877</td>
                                                            <td class="aligntext">10DE3</td>
                                                            <td class="alignnumbers">BATCH32</td>
                                                            <td class="alignnumbers">2.00</td>                   
                                                        </tr>
                                                    </tbody>
                                                </table>--%>



                                <table class="table-striped">
                                    <thead>
                                        <tr class="mytableOutboundHeaderTR">
                                            <th>Line #</th>
                                            <th>Part No.</th>
                                            <th>Location</th>
                                            <th>From Carton</th>
                                            <th>Picked Qty</th>
                                            <th>Delete</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr ng-repeat="MATERIALINFO in PickedList">
                                            <td class="alignnumbers" align="right">{{$index + 1}}  </td>
                                            <td>{{ MATERIALINFO.MCode }}</td>
                                            <td>{{ MATERIALINFO.Location }}</td>
                                            <td class="aligntext">{{ MATERIALINFO.CartonCode }}</td>
                                            <td class="aligntext" align="right">{{ MATERIALINFO.PickedQty }}</td>
                                            <td class="alignnumbers" align="center">
                                                <a align="center" style="text-decoration: none; text-decoration: none; cursor: pointer;" ng-click="Delete(MATERIALINFO.AssignID)" target="_blank" href=""><i class='material-icons'>delete</i></a>
                                            </td>
                                        </tr>
                                </table>
                            </div>

                        </div>
                    </div>                
            </div>
        </div>
        
        <%--<table class="mytable" style="width: 95%; margin-left: 3%; font-family: 'Open Sans'; font-size: 10.1pt;">
                  <thead>
                  <tr style="height:30px;background:#1a79cf;color:#FFFFFF;">
                        <th>Line #</th>
                        <th>Part #</th>
                        <th>SUoM/ Qty.</th>
                        <th>Del. Doc. Qty.</th>
                        <th>Batch No.</th>                                       
                        <th >Picked Qty</th>
                        <th>Alv Qty.</th>
                        <th>HU</th>
                        <th>Location</th>
                        <th></th> 
                      <th></th> 
                  </tr>
             </thead>
             <tbody>
                 <tr ng-repeat = "PK in Pickitem" class="trPO1">
                    <td>{{$index + 1}}  </td>
                    <td >{{ PK.Part }}</td>
                    <td >{{ PK.SUoM }}</td>
                    <td >{{ PK.DelQty }}</td>
                    <td >{{ PK.Batch }}</td>
                    <td >{{ PK.Picked }}</td>
                    <td >{{ PK.Avl }}</td>
                    <td >{{ PK.HU }}</td>
                    <td >{{ PK.Location }}</td>
                    <td ><a style="text-decoration: none;"  href="../mInventory/PickItem.aspx?so={{ PK.Id }} & gd= {{PK.gg}} & hd= {{PK.hd}} ">Pick <img src="../Images/redarrowright.gif"></a></td>
                    <td></td>
                 </tr>
             </tbody>

         </table>--%><br />
    </div>
        </div>
</asp:Content>
