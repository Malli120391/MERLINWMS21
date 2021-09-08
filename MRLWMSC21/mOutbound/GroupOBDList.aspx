<%@ Page Title="Group OBD List:." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="GroupOBDList.aspx.cs" Inherits="MRLWMSC21.mOutbound.GroupOBDList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
       <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="GroupOBDList.js"></script>
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
     <!--    Pagination Style    -->
   <style>
         .pagination ul {
  display: inline-block;
  padding: 0;
  margin: 0;

}

.pagination li {
  display: inline;
   
}

       .flex__ div {
        margin-left:5px;
       }

.pagination li a {
    color: black;
    float: left;
    padding: 5px 10px;
    text-decoration: none;
   
    border-radius: 5px;
    border: 0px solid #FFFFFF;
    box-shadow: 1px 3px 5px #CCCCCC;
    border-spacing: 1px;
    text-decoration: none;
    color: #000;
    margin: 0px 2px;
    box-shadow: var(--z1);
    display: inline-block !important;
    width: 20px !important;
    height: 20px !important;
    text-align: center !important;
   
    border-radius: 2px;
    padding: 1px;
    line-height: 20px;
    

}
           /*.mytableOutbound {
                   width: 150% !important;
        border-spacing: 2px !important;
        border: 0px !important;
        border-radius: 0px !important;
        background-color: #d9efa7 !important;
           }
           .mytableOutboundHeaderTR {
        height: 30px;
        background: #77a066;
        color: #FFFFFF;
        background-color: #94c12c;
        text-align: justify;
        color: #333333;
    }
           .mytableOutboundBodyTR {
        height: 25px;
        background-color: #ffffff !important;
    }*/
.pagination li.active a {
background-color: var(--sideNav-bg) !important;
    color: white;
    border: 2px solid #FFFFFF !important;
    box-shadow: var(--z1);
    padding: 0px;
    display: inline-block !important;
    border: 1px solid var(--sideNav-bg) !important;
    /*background-color: #e67f22 !important;*/
    width: 20px !important;
    height: 20px !important;
    line-height: 20px;
    text-align: center;

}

.pagination li:hover.active a {
  background-color: #40ad8a;
}


       .option:first-child {
            color: #999;
       }
       .obd{
           position: relative;
    top: -9px;
    left: 0px;
        }


    </style>
    <div class="dashed"></div>
    <!--    Pagination Style    -->
    <div ng-app="myApp" ng-controller="GroupOutbound" class="pagewidth">
   
        <div id="divListArea">
            <div>
                <div>
                    <div class="row">
                       <div class="col m2 offset-m5">
                            <div class="flex">
                                
                                <!-- Globalization Tag is added for multilingual  -->
                                <input type="text" id="txtTenant" required="" />
                               <%-- <label>Tenant</label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                            <div class="col m2">
                            <div class="flex">
                                <input type="text" id="txtWH" required="" ng-click="getWH()" />
                                <%--<label>VLPD</label>--%>
                                <label> <%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                       <div class="col m2">
                            <div class="flex">
                                <input type="text" id="txtVLPD" required="" ng-click="getVLPD()" />
                                <%--<label>VLPD</label>--%>
                                <label> <%= GetGlobalResourceObject("Resource", "VLPD")%></label>
                            </div>
                        </div>
                        <div class="col m1">
                            <br />
                           <%-- <button type="button" ng-click="Getdetails()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                             <button type="button" ng-click="Getdetails()" class="btn btn-primary obd"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        </div>
                    </div>
                </div>
            </div>
            <div style="width:100%;overflow:auto;">
                <table class="table-striped" cellpadding="0" cellspacing="0" >
                    <thead>
                        <tr class="">
                            <th sno><%= GetGlobalResourceObject("Resource", "SNo")%> </th>
                            <th><%= GetGlobalResourceObject("Resource", "Tenant")%></th>
                            <th><%= GetGlobalResourceObject("Resource", "WHCode")%> </th>
                            <th><%= GetGlobalResourceObject("Resource", "GroupOBDNo")%> </th>
                            <th><%= GetGlobalResourceObject("Resource", "CreatedDate")%></th>   
                            <th><%= GetGlobalResourceObject("Resource", "CreatedUser")%> </th>
                            <th><%= GetGlobalResourceObject("Resource", "Status")%>  </th>
                            <th> <%= GetGlobalResourceObject("Resource", "AssignStock")%> </th>
                            <th><%= GetGlobalResourceObject("Resource", "Pick")%> </th>
                            <th>View</th>
                             <th><%= GetGlobalResourceObject("Resource", "Verify")%> </th>
                        </tr>
                    </thead>
                   
                    <tbody>
                    <tr class="" dir-paginate="VLPD in GroupOBDList|orderBy:sortKey:reverse|filter:search|itemsPerPage:25" pagination-id="main">
                        <td sno>{{VLPD.SNO}}</td>
                        <td>{{VLPD.tenantName}}</td>
                        <td>{{VLPD.WareHouse}}</td>
                        <td>{{VLPD.GRPNumber}}</td>
                        <td>{{VLPD.CreatedDate}}</td>
                       <%-- <td style="text-align:center;">{{VLPD.CreatedDate' }}</td>--%>
                        <td >{{ VLPD.createduser }}</td>
                        <td >{{ VLPD.Status }}</td>
                       
                        <td>                          
                            <div >
                               <%-- <button type="button" ng-if="VLPD.StatusId<=2"  ng-click="changeVLPDStatus(VLPD.ID)"  class="btn btn-primary">Initiate Pick Up <i class="fa fa-check" aria-hidden="true"></i></button>--%>
                                 <button type="button" ng-if="VLPD.StatusId<=2"  ng-click="changeVLPDStatus(VLPD.ID)"  class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "InitiatePickUp")%> <i class="fa fa-check" aria-hidden="true"></i></button>
                                
                            </div>                                                         
                                                           
                            </td>
                        <td>
                             <a style="text-decoration: none;" ng-if="VLPD.StatusId>2" target="_blank"  ng-href="../mOutbound/GroupDeliveryPickNote.aspx?VLPDID={{VLPD.ID}}"><%=MRLWMSC21Common.CommonLogic.btnfapick %></a>
                        </td>
                     
                        <td>
                            <a style="text-decoration: none;" class="{{VLPD.ID}}" id="asoview" href="" ng-click="openDialog('send Id',VLPD.ID,VLPD.GRPNumber)"><%=MRLWMSC21Common.CommonLogic.btnfaeye %></a>

                        </td>
                        <td >
                             <div>                           
                               
                                    <a   ng-if="VLPD.StatusId==3 " id="btnverify"  ng-click="VerifyVLPD(VLPD.ID)"  style="width:76px;"><%=MRLWMSC21Common.CommonLogic.btnfaverify %></a>                                   
                          <%-- <button type="button"  id="btnverify" class="btn btn-primary" ng-click="VerifyVLPD(VLPD.ID)" ng-hide="{{VLPD.VLPDStatus=='Closed'}} || {{VLPD.VLPDStatus=='Open'}} || {{VLPD.VLPDStatus=='Picking Completed'}}" style="width:76px;">Verify <%=MRLWMSC21Common.CommonLogic.btnfaEdit %></button>                                   
                       --%>
                                 </div>

                        </td>
                    
                       <%-- <td>
                             <button type="button" ng-if="VLPD.VLPDTYPEID==2" id="btnPdf" class="button button3" ng-click="exportPdf(VLPD)">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                        </td>--%>
                    <%--    <td style="text-align:center; width:3%;"> 
                            <a href="#"  Title="Grp VLPD Generated" style="color:green;"  ng-if="VLPD.FLG != 0" ><i class="fa fa-check" aria-hidden="true"></i></a>
                             </td>--%>
                   </tr>
    </tbody>
                </table> </div>
    <div class="lineheight"></div> 
              <div id="divContainer" class="PopupContainerInbound" style="display:none" style="height:300px">
            <div id="divInner" class="PopupInnerOutbound">
                <%--<div class="PopupHeadertextOutbound">OBD Details</div>--%>
                <div class="PopupHeadertextOutbound">{{VLPDNO}}</div>
                    <span id="spanClose" class="fa fa-times PopupSpanCloseOutbound" aria-hidden="true"></span>&emsp;
                        <div class="PopupPaddingOutbound">
                            <div class="PopupSpaceOutbound">
                              
                             
                            
                                 <table class="table-striped">
                                                    <thead>
                                                        <tr class="">  
                                                            <%-- <th style="text-align:left !important;">OBD No.</th>--%>
                                                             <th style="text-align:left !important;"> <%= GetGlobalResourceObject("Resource", "OBDNo")%></th>
                                                            <%--<th style="text-align:left !important;">OBD Date</th> --%>
                                                            <th style="text-align:left !important;"> <%= GetGlobalResourceObject("Resource", "OBDDate")%> </th> 
                                                           <%-- <th style="text-align:left !important;">Customer</th> --%>
                                                             <th style="text-align:left !important;"><%= GetGlobalResourceObject("Resource", "Customer")%> </th> 
                                                           <%-- <th style="text-align:left !important;">Assigned Qty.</th> --%>
                                                             <th style="text-align:left !important;"><%= GetGlobalResourceObject("Resource", "AssignedQty")%>  </th> 
                                                                
                                                        </tr>
                                                    </thead>
                                                    <tbody class="">
                                                         <tr ng-repeat="VL in OBDList" class="mytableOutboundBodyTR">
                                                         <%-- <tr class="mytableOutboundBodyTR">--%>
                                                              <td class="aligntext">{{VL.OBDNumber}}</td>
                                                            <td class="alignnumbers">{{VL.OBDDate | date:'dd-MMM-yyyy'}}</td>
                                                            <td class="aligntext">{{VL.CustomerName}}</td>                                                           
                                                            <td style="text-align:center;">{{VL.AssignedQuantity}}</td>    
                                                                
                                                        </tr>
                                                    </tbody>
                                                </table>
                               <br/>
                                <div style="" flex end> <%--  <button type="button" id="btnClose"  class="btn btn-primary" >Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>--%>
                                      <button type="button" id="btnClose"  class="btn btn-primary" > <%= GetGlobalResourceObject("Resource", "Close")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>
                    </div>
                            </div>
                        </div>
                    </div>                
            </div>
        
       
     <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
            
        </div>
         <br />  
    </div>
</asp:Content>
