<%@ Page Title="OutboundRevert" Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OutboundRevert.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundRevert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
        <script src="../Scripts/angular.min.js"></script>
     <script src="../mReports/Scripts/dirPagination.js"></script>
    <script src="OutboundRevert.js"></script>
    <script>
        $(document).ready(function () {
             $("#txtMFGdate").attr('readonly','readonly'); 
            $("#txtMFGdate").datepicker({ dateFormat: "dd-M-yy" });
              $("#txtEXPdate").attr('readonly','readonly');
            $("#txtEXPdate").datepicker({ dateFormat: "dd-M-yy" });
        });
    </script>
    <style>
.flex input[type="text"], input[type="number"], select
{
    width:100% !important;
}
        .mytableOutboundHeaderTR tr th {
            border-bottom: 3px double !important;
        }

        .c__input__ {
            width: 50px;
    border: 0;
    border-bottom: 1px solid var(--paper-grey-300);
    padding: 4px 0;
        }

        .child thead {
            opacity:0;
        }

            .child thead tr th {
                padding:0px;
            }

        .child tr td {
            border-bottom:0px !important;
            padding:0px !important;
        }

    </style>

<div class="dashed"></div>
    <div class="container">

        <div  ng-app="myApp" ng-controller="RevertOBD" >
           
            <div class="row" style="margin: 0px;">
                <div class="col m3 s3">
                    <div class="flex">
                        <input type="text" id="txtTenant" required="" />
                        <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                        <span class="errorMsg"></span>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <select ng-model="RevertSearchData.RevertType" class="DropdownGH" ng-options="SC.Id as SC.Name for SC in RevertTypes" ng-change="getOBDItems()">
                            <option value=""><%= GetGlobalResourceObject("Resource", "Select")%></option>
                        </select>
                        <label><%= GetGlobalResourceObject("Resource", "RevertType")%> </label>
                        <span class="errorMsg"></span>
                    </div>
                </div>

                <div class="col m3 s3">
                    <div class="flex">
                        <input type="text" id="txtNo" ng-model="RevertSearchData.OBDNO" required="" />
                        <label><%= GetGlobalResourceObject("Resource", "OBDNoItem")%> </label>
                    </div>
                </div>

                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" placeholder=" Mfg. Date" ng-model="RevertSearchData.MFGDate" id="txtMFGdate" />
                        </div>
                    </div>
                </div>
                <div class="col m3 s3" ng-show="ishide">
                    <div class="flex">
                        <div>
                            <input type="text" placeholder=" Exp. Date" ng-model="RevertSearchData.EXPDate" id="txtEXPdate" />
                        </div>
                    </div>
                </div>  
                <div class="col m3 s3" ng-show="ishide">
                                <div class="flex">
                                    <input type="text" id="txtBatchNo" placeholder="Batch No" ng-model="RevertSearchData.BatchNo" />
                                </div>
                            </div>
                            <div class="col m3 s3" ng-show="ishide">
                                <div class="flex">
                                    <input type="text" id="txtSerialNo" placeholder="S. No." ng-model="RevertSearchData.SNO" />
                                </div>
                            </div>

                            <div class="col m3 s3" ng-show="ishide">
                                <div class="flex">
                                    <input type="text" id="txtProjectRefNo" placeholder="Project Ref No." ng-model="RevertSearchData.ProjectRefNo" />
                                </div>
                            </div>
            </div>
              
            <div class="row">
  <div class="flex" style="    float: right;">
      <button type="button" ng-click="Getdetails()"   class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            <%--<button type="button" ng-click="advancesearch()"  class="btn btn-primary">Advance Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
      <button type="button" ng-click="advancesearch()"  class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "AdvanceSearch")%><%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                           <%-- <button type="button" ng-click="Getdetails()"   class="btn btn-primary">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
       
                           <%-- <button type="button" ng-click="clearDetails()"   class="btn btn-primary">Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>--%>
       <button type="button" ng-click="clearDetails()"   class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Clear")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>
                             
                            </div>
            </div>
            <div class="row" style="margin: 0;padding: 0px 10px;"  >
            <div class="" ng-if="OBDData!=undefined && OBDData.length!=0">
                <table class="mytableOutbound" style="">
                    <thead class="mytableOutboundHeaderTR">
                        <tr style="height: 30px;">
                           <%-- <th>OBD No.</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "OBDNo")%> </th>
                            <%-- <th><div ng-if="RevertSearchData.RevertType==3">OBD Date</div></th>--%>
                             <th><div ng-if="RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "OBDDate")%> </div></th>
                             <%--<th><div  ng-if="RevertSearchData.RevertType==3">Document Type</div></th>--%>
                            <th><div  ng-if="RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "DocumentType")%></div></th>
                           <%-- <th><div ng-if="RevertSearchData.RevertType==3">Group OBD No.</div></th>--%>
                             <th><div ng-if="RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "GroupOBDNo")%></div></th>
                            <th>SO NO.</th>
                            <%-- <th><div ng-if="RevertSearchData.RevertType==3">PGI Done NameM</div></th>--%>
                             <th><div ng-if="RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "PGIDoneNameM")%> </div></th>
                             <th><div  ng-if="RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "PGIDate")%> </div></th>
                             <%--<th><div ng-if="RevertSearchData.RevertType==3">Customer</div></th>--%>
                            <th><div ng-if="RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "Customer")%></div></th>
                            <%--<th><div ng-if="RevertSearchData.RevertType!=3">Part No.</div></th>--%>
                            <th><div ng-if="RevertSearchData.RevertType!=3"> <%= GetGlobalResourceObject("Resource", "PartNo")%> </div></th>
                           <th colspan="5">
                               <div ng-if="RevertSearchData.RevertType!=3">
                               <table style="width:100%;" border="0">
                                   <tr>
                                       <td style="text-align:center !important;" colspan="5">
                                           <%= GetGlobalResourceObject("Resource", "MSPS")%>  <%--    <a ng-click="()">+</a>--%>

                                       </td>
                                   </tr>
                                   <tr>
                                    <%--   <td>Batch No.</td>--%>
                                          <td><%= GetGlobalResourceObject("Resource", "BatchNo")%> </td>
                                     <%--  <td>MFG Date</td>--%>
                                         <td> <%= GetGlobalResourceObject("Resource", "MFGDate")%> </td>
                                       <%--<td>EXP Date</td>--%>
                                       <td><%= GetGlobalResourceObject("Resource", "EXPDate")%> </td>
                                      <%-- <td>S. No.</td>--%>
                                        <td> <%= GetGlobalResourceObject("Resource", "SNo")%></td>
                                     <%--  <td>Proj. Ref. No.</td>--%>
                                         <td> <%= GetGlobalResourceObject("Resource", "ProjRefNo")%> </td>
                                   </tr>
                               </table></div>
                           </th>
                            <%--<th><div ng-if="RevertSearchData.RevertType!=3">SO Qty.</div></th>--%>
                            <th><div ng-if="RevertSearchData.RevertType!=3"> <%= GetGlobalResourceObject("Resource", "SOQty")%></div></th>
                            <%--<th><div  ng-if="RevertSearchData.RevertType!=3">Cust. PO Qty.</div></th>--%>
                            <th><div  ng-if="RevertSearchData.RevertType!=3"><%= GetGlobalResourceObject("Resource", "CustPOQty")%> </div></th>
                           <%-- <th><div ng-if="RevertSearchData.RevertType!=3">OBD Qty.</div></th>--%>
                             <th><div ng-if="RevertSearchData.RevertType!=3"> <%= GetGlobalResourceObject("Resource", "OBDQty")%></div></th>
                            <%--<th><div  ng-if="RevertSearchData.RevertType!=3">Picked Qty</div></th>--%>
                            <th><div  ng-if="RevertSearchData.RevertType!=3"> <%= GetGlobalResourceObject("Resource", "PickedQty")%></div></th>
                            <%--<th><div ng-if="RevertSearchData.RevertType==1">Revert Qty.</div></th>--%>
                            <th><div ng-if="RevertSearchData.RevertType==1"><%= GetGlobalResourceObject("Resource", "RevertQty")%> </div></th>
                            <%-- <th><div ng-if="RevertSearchData.RevertType==1 || RevertSearchData.RevertType==3">Revert</div></th>--%>
                             <th><div ng-if="RevertSearchData.RevertType==1 || RevertSearchData.RevertType==3"> <%= GetGlobalResourceObject("Resource", "Revert")%></div></th>
                        </tr>
                    </thead>
                    <tbody class="mytableOutboundBodyTR">
                        <tr dir-paginate="ROI in OBDData |itemsPerPage:100" pagination-id="main">
                            <td>{{ROI.OBDNumber}}</td>
                            <td><div ng-if="RevertSearchData.RevertType==3">{{ROI.OBDDate}}</div></td>
                              <td><div ng-if="RevertSearchData.RevertType==3">{{ROI.DocumentType}}</div></td>
                              <td><div ng-if="RevertSearchData.RevertType==3">{{ROI.VLPDNumber}}</div></td>
                            <td>{{ROI.SONumber}}</td>
                             <td><div ng-if="RevertSearchData.RevertType==3">{{ROI.PGIDoneName}}</div></td>
                             <td><div  ng-if="RevertSearchData.RevertType==3">{{ROI.PGIDate}}</div></td>
                            <td><div ng-if="RevertSearchData.RevertType==3">{{ROI.Customer}}</div></td>
                            <td><div ng-if="RevertSearchData.RevertType!=3">{{ROI.MCode}}</div></td>
                            <td colspan="5">
                                <table class="child" style="width:100%;" border="0">
                                    <thead>
                                        
                                   <tr>
                                       <%--<th style="padding:0px !important;">Batch No.</th>--%>
                                       <th style="padding:0px !important;"><%= GetGlobalResourceObject("Resource", "BatchNo")%>  </th>
                                     <%--  <th style="padding:0px !important;">MFG Date</th>--%>
                                         <th style="padding:0px !important;"><%= GetGlobalResourceObject("Resource", "MFGDate")%> </th>
                                    <%--   <th style="padding:0px !important;">EXP Date</th>--%>
                                          <th style="padding:0px !important;"><%= GetGlobalResourceObject("Resource", "EXPDate")%> </th>
                                      <%-- <th style="padding:0px !important;">S. No.</th>--%>
                                        <th style="padding:0px !important;"><%= GetGlobalResourceObject("Resource", "SNo")%> </th>
                                      <%-- <th style="padding:0px !important;">Proj. Ref. No.</th>--%>
                                        <th style="padding:0px !important;"><%= GetGlobalResourceObject("Resource", "ProjRefNo")%></th>
                                   </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                                <td style="padding:0px !important;"><div ng-if="RevertSearchData.RevertType!=3">{{ROI.BatchNo}}</div></td>
                                                <td style="padding:0px !important;"><div ng-if="RevertSearchData.RevertType!=3">{{ROI.MfgDate}}</div></td>
                                                <td style="padding:0px !important;"><div ng-if="RevertSearchData.RevertType!=3">{{ROI.ExpDate}}</div></td>
                                                <td style="padding:0px !important;"><div ng-if="RevertSearchData.RevertType!=3">{{ROI.SerialNo}}</div></td>
                                                <td style="padding:0px !important;"><div ng-if="RevertSearchData.RevertType!=3">{{ROI.ProjectRefNo}}</div></td>
                                        </tr>
                                        </tbody>
                                </table>
                            </td>
                            <td><div ng-if="RevertSearchData.RevertType!=3">{{ROI.soqty}}</div></td>
                             <td><div  ng-if="RevertSearchData.RevertType!=3">{{ROI.CustPOQuantity}}</div></td>
                             <td><div ng-if="RevertSearchData.RevertType!=3">{{ROI.OBDQty}}</div></td>
                             <td><div ng-if="RevertSearchData.RevertType!=3">{{ROI.PickedQuantity}}</div></td>
                            <td><div ng-if="RevertSearchData.RevertType==1"><input type="text" class="c__input__" onkeypress="return isNumber(event)" ng-model="ROI.RevertQty" /></div></td>
                            <td><div  ng-if="RevertSearchData.RevertType==1 || RevertSearchData.RevertType==3"><%--<button type="button" ng-click="RevertOBDData(ROI)"  class="btn btn-primary">Revert </button>--%>
                                <button type="button" ng-click="RevertOBDData(ROI)"  class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Revert")%> </button>
                                </div></td>
                        </tr>
                    </tbody>
                </table>
                 <div style="float: right !important; font-family: Arial; font-size: small;margin-right:1%;">
                                           <dir-pagination-controls  class="getPageId"   direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>
                                       </div>
            </div>
                <div ng-if="OBDData!=undefined && OBDData!=null && OBDData.length!=0 && RevertSearchData.RevertType==2" style="float:right;padding-right:35px;padding-top:35px;">
                     <button type="button" ng-click="RevertOutBound()"  class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "RevertOutBound")%> </button>
                </div>
        </div>
        </div>
        </div>
</asp:Content>
