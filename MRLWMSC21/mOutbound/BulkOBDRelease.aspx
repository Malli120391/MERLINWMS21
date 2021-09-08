<%@ Page Title=".: Bulk OBD Release :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="BulkOBDRelease.aspx.cs" Inherits="MRLWMSC21.mOutbound.BulkOBDRelease" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>


    <script src="BulkOBDRelease.js"></script>

     <script type="text/javascript">
         $(document).ready(function () {
             $("#txtFromDate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date(),
                 onSelect: function (selected) {
                     $("#txtTodate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                 }
             });
             $("#txtTodate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date()
             });

             $("#txtToDate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date(),
                 onSelect: function (selected) {
                     $("#txtTodate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                 }
             });
             $("#txtTodate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date()
             });
             $("#txtDueDate").datepicker({
                 todayBtn: 1,
                 singleDatePicker: true,
                 showDropdowns: true,
                 autoclose: true,
                 forceParse: false,
                  dateFormat: "dd-M-yy",
                 startDate: "today",
                 onClick: function (ValidThru) {
                     $("#txtDueDate").datepicker("option", "minDate", ValidThru, { dateFormat: "dd-M-yy" })
                 }
             });
             $("#txtDueDate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date()
             });

         });

    </script>


    <div class="dashed"></div>
    <div class="pagewidth">
        <div ng-app="MyApp" ng-controller="FormCtrl">
            <inv-preloader is="show" style="display:none"></inv-preloader>
            <div ng-show="blockUI">
                <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                    <div style="align-self: center;">
                        <div class="spinner">
                            <div class="bounce1"></div>
                            <div class="bounce2"></div>
                            <div class="bounce3"></div>
                        </div>

                    </div>

                </div>
            </div>


            <div class="row">
                <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtFromDate" required="" />
                            <label>From Date</label>
                            
                    </div>

                </div>
                <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtToDate" required="" />
                            <label>To Date</label>
                            
                    </div>

                </div>
                <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtWarehouse" required="" />
                            <label>Warehouse</label>
                            <span class="errorMsg"></span>
                    </div>

                </div>

                <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtTenant" required="" />
                            <label>Tenant</label>
                            
                    </div>

                </div>

                
                
                 <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtOBD" required="" />
                            <label>OBD Number</label>
                            
                    </div>

                </div>

            </div>
            <div class="row">
                 <%--<div class ="col s4 m2">
                    <div class ="flex">
                        <select  style="padding-right:30px !important; margin-right:30px !important;"  required="" id="dropPriority">
                           
                            <option value=""> Select OBD Priority</option>
                            <option value="1"> Normal</option>
                           <option value="2"> High Priority</option>
                        </select>
                            
                    </div>

                </div>--%>
                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text"  id="txtPriority" required=""/>
                        <label>Priority</label>

                    </div>

                </div>
                <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtDueDate" required="" />
                            <label>Due Date</label>
                            
                    </div>

                </div>
                 <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtSO" required="" />
                            <label>SO Number</label>
                            
                    </div>

                </div>
                 <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtCustomer" required="" />
                            <label>Customer</label>
                            
                    </div>

                </div>
                 <div class ="col s4 m2">
                    <div class ="flex">
                       <input type="text" id="txtDeliveryStatus" required="" />
                            <label>Delivery Status</label>
                        <span class="errorMsg"></span>
                            
                    </div>

                </div>
                 <div class ="col s4 m2">
                    <div class ="flex">
                       <button type="button" id="btnSearch" ng-click="Getgedetails(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            
                    </div>

                </div>

            </div>

            <div class="row" >
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th><input type="checkbox" ng-model="$parent.AllCheck" id="parent" ng-click = "SelectAll()" /></th>
                            <th>SL.NO</th>
                            <th>Tenant</th>
                            <th>Warehouse</th>
                            <th>Customer</th>
                            <th>SO Number</th>
                            <th>OBD Number</th>
                            <th>OBD Date</th>
                            <th>Priority</th>
                            <th>Due Date</th>
                            <th>Total Qty.</th>
                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="row in BillingData  |itemsPerPage:noofrecords" total-items="Totalrecords"">
                            <td><div class="checkbox"><input type="checkbox" id="rsncheck{{$index+1}}" ng-model="row.IsSelected" /><label for="rsncheck{{$index+1}}"></label></div></td>
                            <td>{{$index+1}}</td>
                            <td>{{row.TenantCode}}</td>
                            <td>{{row.WHCode}}</td>
                            <td>{{row.CustomerCode}}</td>
                            <td>{{row.SONumber}}</td>
                            <td>{{row.OBDNumber}}</td>
                            <td>{{row.OBDDate}}</td>
                            <td>{{row.Prio}}</td>
                            <td>{{row.DueDate}}</td>
                            <td>{{row.TotalQuantity}}</td>
                            
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr ng-show="BillingData.length == 0">
                            <td colspan="11" style="text-align: center !important">No Data Found</td>
                        </tr>
                    </tfoot>
                </table>
                <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="Getgedetails(newPageNumber)"> </dir-pagination-controls>
            </div>

            </div>
            <div flex end>
                <div id="divdock" class="flex" style ="z-index:999;" ng-hide="DisplayReleaseData"  >
                    <input type="text" id="txtdock" required="" />
                    <label>Dock</label>
                    <span class="errorMsg"></span>

                </div>&emsp;
         
                <div>
                    <button id="btncreate" type="button" ng-click="BulkRelease()" class="btn btn-primary">Release Outbound <i class="fa fa-folder-open" aria-hidden="true"></i></button>
                 </div>&emsp;
            </div>
            

        </div>
    </div>
    <br />
    

</asp:Content>

