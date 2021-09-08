<%@ Page Title="JOB Order List" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="JobOrderList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.JobOrderList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
     <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
        <script src="JobOrder.js"></script>
       
    <div class="angulardiv container" ng-app="JOBListMyApp" ng-controller="JOBOrderList">

        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">

                <div style="align-self: center;">
                    <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader/>

                </div>

            </div>

        </div>
       <div>
            <div class="row">
                <div class="col m3 s3 offset-m1">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtTenant"  ng-model="search.Tenant" required=""/>

                            <label>Tenant</label>


                        </div>
                    </div>
                </div>
                 <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtBOMRefNo"  ng-model="search.BomRefNo" required=""/>
                           <label>BOM Ref. No.</label>
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                           
                            <input type="text" id="txtJobRefNo"  ng-model="search.JOBRefNo" required=""/>
                             <label>Job Order Ref. No.</label>

                        </div>
                    </div>
                </div>
               
                <div class="col m2 s3">
                     <gap5></gap5>
                    
                                   <button type="button"  ng-click = "getJOBHeaderData()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                            <button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()">Add<i class="material-icons">add</i></button>
                       
                </div>
            </div>
        </div>
        
        <div ng-if="JOBList!=undefined && JOBList!=null && JOBList.length!=0">
             <table class="table-striped">
                    <thead>
                        <tr class="">
                            <th>S. No.</th>
                            <th>Tenant</th>
                             <th>JOB Ref. No.</th>
                            <th>Bom Ref. No.</th>
                            <th>BOM Part No.</th>
                           <th>UOM</th>
                            <th number>Quantity</th>
                             <th>Status</th>
                             <th>JOB Order Type</th>
                            <th>Created Date</th>
                            <th>Created User</th>
                            <th></th>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr class="" dir-paginate="job in JOBList  |itemsPerPage:25" pagination-id="main">
                            <td align="right">{{job.SNO}}</td>
                            <td>{{job.Tenant}}</td>
                            <td>{{job.JOBRefNo}}</td>
                            <td style="width:150px !important;">{{job.BOMRefNo}}</td>
                            <td>{{job.MCode}}</td>
                             <td>{{job.UOM}}</td>
                             <td number>{{job.Quantity}}</td>
                             <td>{{job.Status}}</td>
                             <td>{{job.JOBOrderType}}</td>
                            <td>{{job.CreatedDate}}</td>
                            <td>{{job.CreatedUser}}</td>
                            <td><a target="_blank" href="../mMaterialManagement/JobOrder.aspx?JOBID={{job.JOBOrderId}}"><i class="material-icons">edit</i></a></td>


                        </tr>
                    </tbody>
                </table>
            <gap></gap>
                 <div flex end>
                  <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
                </div> 
            <gap></gap>
        </div>
       
    </div>
</asp:Content>
