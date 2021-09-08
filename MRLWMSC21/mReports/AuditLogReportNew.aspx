<%@ Page Title="Table Audit Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="AuditLogReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.AuditLogReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/CommonWMS.js"></script>
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>    
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="AuditLogReportNew.js"></script>
    <script>      

        function LoadClickHandlers() {
            $('.trAuditHeader').click(function () {
                $(this).next().toggle();
            });
        }
    </script>

    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />

    <div data-ng-app="MyApp" data-ng-controller="AuditLogReportNew" class="container">
        
        <div class="">
            <div class="row"> 
              
                <div class="">   
                       <div class="">
                           <div class="col m3 offset-m4">
                               <div class="flex">
                                   <select id="ddlCategory">
                                       <option value="1"><%= GetGlobalResourceObject("Resource", "Inbound")%></option>
                                       <option value="2"><%= GetGlobalResourceObject("Resource", "Outbound")%></option>
                                       <option value="3"><%= GetGlobalResourceObject("Resource", "InternalTransfers")%> </option>
                                       <option value="4"><%= GetGlobalResourceObject("Resource", "CycleCounts")%> </option>
                                   </select>
                                   <span class="errorMsg"></span>
                               </div>
                           </div>
                           <div class="col m3">
                               <div class="flex">
                                   <input type="text" id="txtRefNo" placeholder="Reference No." ng-click="getRefNo()" class="TextboxInventoryAuto" />
                                   <span class="errorMsg"></span>
                               </div>
                           </div>
                           <div class="col m2">
                               <gap5></gap5>
                               <button type="button" data-ng-click="GetALdetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                               <a ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%> <i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                           </div>
                        </div> 
                </div> 
            </div>
           
      

            <div class=" m0">
                <div>
                     <div class="">
                                    <td>
                                        <table style="width:100%;" id="tbldata" class=" table-striped">
                                            <thead>
                                                <tr>
                                                    <%--<th>S.No</th>
                                                    <th>Description</th>
                                                    <th>Operation</th>
                                                    <th>Activity By</th>
                                                    <th>Activity Timestamp</th>
                                                    <th>&nbsp;</th>--%>
                                                    <th><%= GetGlobalResourceObject("Resource", "SNo")%></th>
                                                    <th><%= GetGlobalResourceObject("Resource", "Description")%> </th>
                                                    <th> <%= GetGlobalResourceObject("Resource", "Operation")%></th>
                                                    <th> <%= GetGlobalResourceObject("Resource", "ActivityBy")%></th>
                                                   <%-- <th>Activity Timestamp</th>--%>
                                                     <th> <%= GetGlobalResourceObject("Resource", "ActivityTimestamp")%></th>
                                                    <th>&nbsp;</th>
                                                </tr>                                            
                                            </thead>
                                            <tbody data-ng-repeat="Header in AuditList">
                                                <tr class="trAuditHeader">
                                                    <td>{{$index +1}}</td>
                                                    <td>{{Header.TableName}}</td>
                                                    <td>{{Header.Operation}}</td>
                                                    <td>{{Header.ActivityUserName}}</td>
                                                    <td>{{Header.ActivityTimestamp}}</td>
                                                    <td><span class="fa fa-chevron-down"></span></td>
                                                </tr>
                                                <tr class="trAuditDetails">
                                                    <td>&nbsp;</td>
                                                    <td colspan="4">
                                                        <table style="width:100%">
                                                            <thead>
                                                                <tr>
                                                                 <%--   <td>S.No</td>
                                                                    <td>Description</td>                                                           
                                                                    <td>Old Value</td>
                                                                    <td>New Value</td>--%>
                                                                       <td><%= GetGlobalResourceObject("Resource", "SNo")%> </td>
                                                                    <td><%= GetGlobalResourceObject("Resource", "Description")%> </td>                                                           
                                                                    <td><%= GetGlobalResourceObject("Resource", "OldValue")%> </td>
                                                                    <td> <%= GetGlobalResourceObject("Resource", "NewValue")%></td>
                                                                </tr>
                                                            </thead>
                                                                <tbody>
                                                                <tr data-ng-repeat="Details in Header.Details">
                                                                    <td>{{$index +1}}</td>
                                                                    <td>{{Details.DataPoint}}</td>                                                                
                                                                    <td>{{Details.OldValue}}</td>
                                                                    <td>{{Details.NewValue}}</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>                               
                                </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
