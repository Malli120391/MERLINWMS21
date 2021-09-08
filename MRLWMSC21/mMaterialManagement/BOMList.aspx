<%@ Page Title="BOM List" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="BOMList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.BOMList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
     <script src="../Scripts/angular.min.js"></script>
     <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <script src="BOM.js"></script>
 

    <div class="angulardiv container" ng-app="ListMyApp" ng-controller="BOMList">
      
        <div style="">
            <div class="row">
                <div class="col m3 s3 offset-m1">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtTenant"  ng-model="search.Tenant" required=""/>

                            <label>Tenant</label>


                        </div>
                    </div>
                </div>
                <div class="col s3 m3">
                    <div class="flex">
                        <div>
                           
                            <input type="text" id="txtParentPartNo"  required="" ng-model="search.PartNo" required="" />
                           <label>Part No.</label>

                        </div>
                    </div>
                </div>
                <div class="col s3 m3">
                    <div class="flex">
                        <div>
                            <input type="text" id="bomrefNo"  ng-model="search.BomRefNo" required=""/>
                           <label>BOM Ref. No.</label>
                        </div>
                    </div>
                </div>
                <div class="col s3 m2">
                    <gap5></gap5>
                    <button type="button" ng-click="getBOMHeaderData()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                             <a style="text-decoration: none;" target="_blank" href="../mMaterialManagement/BOM.aspx">
                                 <button type="button" type="button" class="btn btn-primary" ng-click="changemenulink()">Add<i class="material-icons">add</i></button></a>

                </div>
            </div>
        </div>
        
        <div ng-if="BOMList!=undefined && BOMList!=null && BOMList.length!=0">
             <table  class="table-striped">
                    <thead>
                        <tr>
                            <th>S. No.</th>
                            <th>Tenant</th>
                            <th>Bom Ref. No.</th>
                            <th>Part No.</th>
                           <th>UOM</th>
                            <th>Remarks</th>
                            <th>Created Date</th>
                            <th>Created User</th>
                            <th></th>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr dir-paginate="bom in BOMList  |itemsPerPage:25" pagination-id="main">
                            <td>{{bom.SNO}}</td>
                            <td>{{bom.Tenant}}</td>
                            <td>{{bom.BOMRefNo}}</td>
                            <td>{{bom.MCode}}</td>
                             <td>{{bom.UOM}}</td>
                             <td>{{bom.Remarks}}</td>
                             <td>{{bom.CreatedDate}}</td>
                            <td>{{bom.CreatedUser}}</td>
                            <td><a target="_blank" href="../mMaterialManagement/BOM.aspx?BOMID={{bom.BOMID}}"><i class="material-icons">edit</i></a></td></td>


                        </tr>
                    </tbody>
                </table>
         <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
        </div>
        <br />
        <br />
        <br />
       
    </div>
</asp:Content>
