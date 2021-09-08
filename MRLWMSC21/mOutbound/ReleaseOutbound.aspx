<%@ Page Title="Release  Outbound:." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="ReleaseOutbound.aspx.cs" Inherits="MRLWMSC21.mOutbound.ReleaseOutbound" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="smngrRevertOutbound" SupportsPartialRendering="true"></asp:ScriptManager>

      <script src="../mInventory/Scripts/angular.min.js"></script>
  <script src="../mInventory/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="ReleaseOutbound.js"></script>
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

        <style>
            .pagination ul {
                display: inline-block;
                padding: 0;
                margin: 0;
            }

            .pagination li {
                display: inline;
            }

                .pagination li a {
                    margin: 0px 2px;
                    box-shadow: var(--z1);
                    display: inline-block !important;
                    width: 20px !important;
                    height: 20px !important;
                    text-align: center !important;
                    background: #fff;
                    border-radius: 2px;
                    padding: 1px;
                    line-height: 20px;
                    text-decoration: none;
                    color: black;
                }

                .pagination li.active a {
                    box-shadow: var(--z1);
                    padding: 0px;
                    display: inline-block !important;
                    border: 2px solid var(--sideNav-bg) !important;
                    background-color: var(--sideNav-bg) !important;
                    width: 20px !important;
                    height: 20px !important;
                    text-align: center;
                    line-height: 20px;
                    color: #fff;
                }

                .pagination li:hover.active a {
                    background-color: #f2f2f2;
                }

                .pagination li:hover:not(.active) a {
                    background-color: #f2f2f2 !important;
                    color: black;
                }

            table .material-icons {
                color: #424040;
            }


            .PopupSpaceOutbound {
                height: fit-content;
            }




          

          

    </style>
    <div class="container">
    <asp:UpdateProgress ID="uprgReleaseOB" runat="server" AssociatedUpdatePanelID="upnlReleaseOB">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                            
                                <div style="align-self:center;" >
                                    <div class="spinner">
                                        <div class="bounce1"></div>
                                        <div class="bounce2"></div>
                                        <div class="bounce3"></div>
                                    </div>

                                </div>
                                  
                                </div>
                                
                                
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="upnlReleaseOB" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                            <ContentTemplate>
                                 <div ng-app="Myapp" ng-controller="ReleaseOutbound" class="">
                                     <div ng-show="blockUI">
                                         <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                                             <div style="align-self: center;">
                                                 <img width="60" src="../Images/preloader.svg" />
                                             </div>
                                         </div>
                                     </div>
                               <div class="divlineheight"></div>
                                     <div class="row">
                                         <div class="">
                                             <div class="col m3 s3 offset-m5 offset-s5">
                                                 <div class="flex">
                                                     <input type="text" id="txtTenant" required="required" />
                                                     <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                                 </div>
                                             </div>
                                             <div class="col m3 s3">
                                                 <div class="flex">
                                                     <%--<input type="text" id="txtOBDNumber" ng-click="getOBD()" onkeydown="return false;" required="required" />--%>
                                                     <input type="text" id="txtOBDNumber" ng-click="getOBD()" required="required" />

                                                     <label><%= GetGlobalResourceObject("Resource", "OBDNumber")%> </label>
                                                 </div>
                                             </div>
                                             <div class="col m1 s1 p0">
                                                 <gap5></gap5>
                                                 <button type="button" ng-click="GetOBDdetails()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                                             </div>
                                         </div>
                                     </div>
                
                                     <div ng-if="Releaseinfo.length > 0">
                                         <table class='table-striped' id="tbldatasonumbers">
                                             <thead>
                                                 <tr class=''>
                                                     <th><%= GetGlobalResourceObject("Resource", "Tenant")%> </th>
                                                     <th><%= GetGlobalResourceObject("Resource", "Customer")%> </th>
                                                     <th>Warehouse </th>
                                                     <th number><%= GetGlobalResourceObject("Resource", "OBDNumber")%></th>
                                                     <th style="text-align: center" middle><%= GetGlobalResourceObject("Resource", "OBDDate")%></th>
                                                     <th number><%= GetGlobalResourceObject("Resource", "TotalQty")%>  </th>
                                                     <th number><%= GetGlobalResourceObject("Resource", "ReleaseQty")%>  </th>
                                                     <th>&nbsp;</th>
                                                     <th style="display: none"><%= GetGlobalResourceObject("Resource", "Release")%> </th>
                                                     <th><%= GetGlobalResourceObject("Resource", "Release")%> </th>
                                                 </tr>
                                             </thead>

                                             <tbody>
                                                 <tr dir-paginate="rlinfo in Releaseinfo |itemsPerPage:25" pagination-id="nonAvaible" class='mytableOutboundBodyTR' total-items="TotalRecords">

                                                     <td>{{ rlinfo.TenantName }} </td>
                                                     <td>{{ rlinfo.CustomerName }}</td>
                                                     <td>{{ rlinfo.WarehouseCode  }}</td>
                                                     <td number>{{ rlinfo.OBDRefNo }}</td>
                                                     <td style="text-align: center" middle>{{ rlinfo.OBDDate }}<span class="animateee" ng-if="rlinfo.IstodayOBD==1"></span></td>
                                                     <%--  <td>{{ rlinfo.SONumber }}</td>
                                                                  <td>{{ rlinfo.SODate| date:'dd-MM-yyyy' }}</td>--%>
                                                     <td number>{{ rlinfo.SOQty }}</td>
                                                     <td number>{{ rlinfo.ReleaseQty }}</td>
                                                     <td>&nbsp;</td>
                                                     <td class="text-center" style="display: none">
                                                         <button type="button" ng-click="ReleaseItem(rlinfo.OutboundID)" class="addbuttonOutbound" style="width: 90px;">Release <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button></td>
                                                     <%--  <td style="text-align:center">
                                                                    <a style="text-decoration: none;" ng-click="openDialog('Add New Revision',rlinfo.OBDRefNo,rlinfo.DeliveryStatusID);" target="_blank"  href="">
                                                                    <i class="material-icons vl">remove_red_eye</i></a></td>--%>
                                                     <td><a target="_blank" href="../mOutbound/ReleaseOBDItems.aspx?id={{rlinfo.OutboundID}}"><i class="material-icons">launch</i></a></td>
                                                 </tr>
                                                 <tr ng-if="Releaseinfo.length == 0">
                                                     <%--<td colspan="8" style="text-align:center !important;">No Data Found</td>--%>
                                                     <td colspan="8" style="text-align: center !important;"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></td>
                                                 </tr>
                                             </tbody>
                                             <tfoot style="display:none;">
                                                 <tr class="mytableReportFooterTR">
                                                     <td colspan="15">
                                                         
                                                     </td>
                                                 </tr>
                                             </tfoot>

                                         </table>

                                         <%--<div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;display:none;">
                                             <dir-pagination-controls class="getPageId" direction-links="true" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
                                         </div>--%>

                                          <div class="row">
                                         <div class="col m12" flex end>
                                             <div class="divpaginationstyle" style="float: right">
                                                 <dir-pagination-controls direction-links="true" pagination-id="nonAvaible" boundary-links="true" on-page-change="GetOBDInfo(newPageNumber)"> </dir-pagination-controls>
                                             </div>
                                         </div>
                                     </div>
                                     </div>

                                    
        
                                     
        
                                    <div id="divContainer" class="PopupContainerInbound" style="display:none" style="height:300px;width:1020px">
                                        <div id="divInner" class="PopupInnerOutbound" style="height:300px;width:1020px">
                                            <%--<div class="PopupHeadertextOutbound"> OBD Item Details</div>--%>
                                            <div class="PopupHeadertextOutbound"> <%= GetGlobalResourceObject("Resource", "OBDItemDetails")%></div>
                                                <span id="spanClose" class="fa fa-times PopupSpanCloseOutbound" aria-hidden="true"></span>&emsp;
                                                    <div class="PopupPaddingOutbound">
                                                        <div class="PopupSpaceOutbound">
                                                            <br />
                             
                                                         <div style="text-align:left;color:var(--sideNav-bg) !important;font-family:Arial;font-size:11pt;font-weight:bold;">OBD Item Details</div>
                                                             <div class="lineheight"></div>
                                                            <br />
                                                            <table>
                                                                <tr><td width="33%">
                                                                  <%--  Total Volume: {{MainVolume}}--%>
                                                                      <%= GetGlobalResourceObject("Resource", "TotalVolume")%>: {{MainVolume}}
                                                                    </td>
                                                                    <td width="1%">&nbsp;</td>
                                                                   <%-- <td width="32%">Total Weight: {{MainWeight}}</td>--%>
                                                                     <td width="32%"> <%= GetGlobalResourceObject("Resource", "TotalWeight")%>: {{MainWeight}}</td>
                                                                     <td width="1%">&nbsp;</td>
                                                                   <%-- <td style="color:red" width="33%">Suggestion: {{AvailableContainer}}</td>--%>
                                                                     <td style="color:red" width="33%"><%= GetGlobalResourceObject("Resource", "Suggestion")%> : {{AvailableContainer}}</td>
                                                                </tr>
                                                            </table>

                                                             <div>
                                                                 <div>
                                                                     <table class="table-striped" style="width: 1300px;">
                                                                         <thead>
                                                                             <tr class="">
                                                                                 <th></th>

                                                                                 <th><%= GetGlobalResourceObject("Resource", "SONumber")%></th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "SODate")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "LineNumber")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "Item")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "Description")%>  </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "SOQty")%>   </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "DispatchQty")%>  </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "PendingQty")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "DeliveryQty")%></th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "VolumeCBM")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "MWeight")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "TotalVolume")%> </th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "TotalWeight")%></th>
                                                                                 <th><%= GetGlobalResourceObject("Resource", "RevertQty")%> </th>
                                                                                 <th></th>
                                                                             </tr>
                                                                         </thead>
                                                                         <tbody class="mytableOutboundBodyTR">
                                                                             <tr ng-repeat="OBD in OBDwiseItems" class="mytableOutboundBodyTR">
                                                                                 <td style="text-align: center;">
                                                                                     <div class="checkbox">
                                                                                         <input type="checkbox" ng-model="OBD.IsSelected" id="ng-change-example1{{$index +1 }}" ng-change="calcualtedimension()" /><label for="ng-change-example{{$index +1 }}"> </label>
                                                                                     </div>
                                                                                 </td>
                                                                                 <td class="aligntext">{{OBD.SONumber}}</td>
                                                                                 <td>{{OBD.SODate}}</td>
                                                                                 <td style="text-align: right;">{{OBD.LineNumber}}</td>
                                                                                 <td class="aligntext">{{OBD.MCode}}</td>
                                                                                 <td class="aligntext">{{OBD.MDescription}}</td>
                                                                                 <td style="text-align: right;">{{OBD.SOQty}}</td>
                                                                                 <td style="text-align: right;">{{OBD.DispatchQty}}</td>
                                                                                 <td style="text-align: right;">{{OBD.PendingQty}}</td>
                                                                                 <td class="alignnumbers">
                                                                                     <input type="text" ng-model="OBD.DeliveryQty" ng-keyup="checkQty(OBD)" style="text-align: right; border: 0px; border-bottom: 1px solid; border-color: var(--paper-grey-300) !important;" /></td>
                                                                                 <td style="text-align: right;">{{OBD.VolumeinCBM}}</td>
                                                                                 <td style="text-align: right;">{{OBD.MWeight}}</td>
                                                                                 <td style="text-align: right;">{{OBD.TotalVolume}}</td>
                                                                                 <td style="text-align: right;">{{OBD.TotalWeight}}</td>
                                                                                 <td class="alignnumbers">
                                                                                     <input type="text" ng-model="OBD.RevertQty" onkeypress="return isNumber(event)" ng-keyup="checkRevertQty(OBD)" style="text-align: right; border: 0px; border-bottom: 1px solid; border-color: var(--paper-grey-300) !important;" /></td>
                                                                                 <%-- <td><button type="button" id="btnRevert" class="btn btn-primary"  ng-click="revertLineItmes(OBD)">Revert <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>--%>
                                                                                 <td>
                                                                                     <button type="button" id="btnRevert" class="btn btn-primary" ng-click="revertLineItmes(OBD)"><%= GetGlobalResourceObject("Resource", "Revert")%> <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>
                                                                             </tr>

                                                                         </tbody>
                                                                     </table>
                                                                 </div>
                                                             </div>
                                                            <table>
                                                            <tr>
                                                               
                                                                                        <td colspan="5">
                                                                                            <div ng-if="DisplayReleaseData">
                                                                                                  <%--<div class="checkbox"><input type="checkbox" id="isobd" ng-model="$parent.IsOBDPicking" /> <label for="isobd">Is OBD Picking</label>--%>
                                                                                                <div class="checkbox"><input type="checkbox" id="isobd" ng-model="$parent.IsOBDPicking" /> <label for="isobd"><%= GetGlobalResourceObject("Resource", "IsOBDPicking")%> </label>
                                                                                            </div>&nbsp;&nbsp;&nbsp;
                                                                                                   <%-- <button type="button" id="btnRelease" class="btn btn-primary"  ng-click="saveBulkReleaseItems()">Release <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>--%>
                                                                                                 <button type="button" id="btnRelease" class="btn btn-primary"  ng-click="saveBulkReleaseItems()"><%= GetGlobalResourceObject("Resource", "Release")%> <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>&nbsp;&nbsp;<%--<button type="button" id="btnClose" class="btn btn-primary" ng-click="closepopup();">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>--%>
                                                                                                <button type="button" id="btnClose" class="btn btn-primary" ng-click="closepopup();"><%= GetGlobalResourceObject("Resource", "Close")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>
                                                                                            </div>
                                                                                            </td>
                                                                                           
                                                                                    </tr></table>
                                                            <div>
                                                                <table id="Table2" class="mytablechildOutbound">
                                                                    <thead>
                                                                        <tr class="mytableOutboundchildHeaderTR">
                                                                           <%-- <th>SO No.</th>--%>
                                                                             <th><%= GetGlobalResourceObject("Resource", "SONo")%></th>
                                                                             <th><%= GetGlobalResourceObject("Resource", "PartNo")%> </th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "MFGDate")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "ExpDate")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "ExpDate")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "SNo")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "BatchNo")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "ProRefNo")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "Qty")%></th>
                                                                            <th><%= GetGlobalResourceObject("Resource", "Status")%></th>
                                                                          <%--  <th style="text-align: center;"  ng-repeat="(key, val) in resultantassigndata[0]">{{ key }}
                                                                            </th>--%>

                                                                        </tr>
                                                                    </thead>
                                                                    <tbody  class="mytableOutboundBodyTR">
                                                                        <tr ng-repeat="row in resultantassigndata "  class="mytableOutboundBodyTR">
                                                                            <td>{{row.SONumber}}</td> 
                                                                              <td>{{row.MCode}}</td>   
                                                                              <td>{{row.MfgDate}}</td>   
                                                                              <td>{{row.ExpDate}}</td>   
                                                                              <td>{{row.SerialNo}}</td>   
                                                                              <td>{{row.BatchNo}}</td>   
                                                                              <td>{{row.ProjectRefNo}}</td> 
                                                                            <td>{{row.Quantity}}</td> 
                                                                             <td>{{row.ErrorMessage}}</td> 
                                                                           <%-- <td ng-repeat="(key, val) in row">{{ val }}
                                                                            </td>--%>
                                                                        </tr>

                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                          
                                                        </div>
                                                    </div>
                                                </div>                
                                        </div>
     
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
    </div>
</asp:Content>
