<%@ Page Title="Pick Performance Report" Language="C#" AutoEventWireup="true" MasterPageFile="~/mReports/mReport.master"  CodeBehind="PerformanceReport.aspx.cs" Inherits="MRLWMSC21.mReports.PerformanceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
            <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txtTodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $("#txtTodate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });

            $('#txtFromdate, #txtTodate').keypress(function () {
                return false;
            });
        });

    </script>


    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
   
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="PerformanceReport.js"></script>
   
     <div ng-app="myApp" ng-controller="PickPerformance" class="container">
          <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />
                </div>
            </div>
        </div>
         <div class="row">
             <div class="">
                 <div class="">

                      <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>

                     <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtTenant" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                    
                     <div class="col m3">
                         <div class="flex">
                             <input type="text" required="" ng-model="fromdate" id="txtFromdate" onpaste="return false" />
                             <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                         </div>
                     </div>
                     <div class="col m3">
                         <div class="flex">
                             <input type="text" required="" ng-model="todate" id="txtTodate" onpaste="return false" />
                             <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                         </div>
                     </div>
                     <div class="col m12">
                         <gap5></gap5>
                        <flex end> <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                         <button type="button" ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%> <i class="fa fa-file-excel-o" aria-hidden="true"></i></button></flex>
                     </div>
                    
                 </div>
             </div>

         </div>
          <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;" >
                <div class="divmainwidth" >
                <table class=" table-striped" id="tbldatas" >
                    <thead>
                        <tr class="">
                           <%-- <th ng-click="sort('Client')">S.No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Picking Lines</th>
                            <th ng-click="sort('OBDNumber')">Picking Performance</th>
                            <th ng-click="sort('OBDNumber')">WHCode</th>
                            <th ng-click="sort('OBDNumber')">Location Zone Code</th>
                            <th ng-click="sort('OBDNumber')">User Name</th>
                            <th ng-click="sort('OBDNumber')">Picked On</th>--%>
                             <th ng-click="sort('Client')"> <%= GetGlobalResourceObject("Resource", "SNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "PickingLines")%> </th>                            
                            <th ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "WHCode")%> </th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "LocationZoneCode")%></th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "UserName")%> </th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "PickedOn")%></th>
                            <th ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "PickingPerformance")%></th>
                        </tr>
                    </thead>
                    <tbody class="">
                        <tr dir-paginate="PL in PList | itemsPerPage:25">
                            <td align="right">{{$index +1}}</td>
                            <td align="left">{{PL.PickingLines}}</td>
                            
                            <td >{{PL.WHCode}}</td>
                            <td >{{PL.LocationZoneCode}}</td>
                            <td align="left">{{PL.UserName}}</td>
                            <td align="left">{{PL.PickedOn}}</td>
                            <td >{{PL.PickingPerformence}}</td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr class="mytableReportFooterTR">
                            <td colspan="15">
                                <div class="divpaginationstyle">
                                    <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                </div>
                            </td>
                        </tr>
                    </tfoot>

                </table></div>

                <table id="tbldata"></table>
            </div>
        <div class="divlineheight"></div>
              
          </div>   
               
         </div>
     
</asp:Content>