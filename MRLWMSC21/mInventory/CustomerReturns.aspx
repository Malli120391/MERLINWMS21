<%@ Page Title="Customer Returns" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CustomerReturns.aspx.cs" Inherits="MRLWMSC21.mInventory.CustomerReturns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
     <script src="Scripts/angular.min.js"></script>
    <script src="CustomerReturns.js"></script>
     <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

     <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>
  

    <div ng-app="MyApp" ng-controller="Customerreturns" class="container">
         <div ng-show="blockUI">
   <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                   
                    <img width="60" src="../Images/Preloader.svg"/>
                </div>

            </div>

        </div>
  
</div>
        <div>
             <!-- Globalization Tag is added for multilingual  -->

              <div class="" align="right" >
                        <div class="row">
                             <div class="col m3 s3">
                                <div class="flex">
                                    <input type="text" ID="atcStoreNo" SkinID="txt_Auto" class="ui-autocomplete-input" required="" />
                                    <label><%= GetGlobalResourceObject("Resource", "Store")%></label>
                                    <span class="errorMsg"></span>
                                 </div>
                            </div>
                            <div class="col m3 s3 ">
                                <div class="flex">
                                <input type="text" ID="txtTenant"  SkinID="txt_Hidden_Req" class="ui-autocomplete-input" required="" />
                                    <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                    <span class="errorMsg"></span>
                                <asp:HiddenField runat="server" ID="hifTenant"/>
                                </div>
                            </div>
                            <div class="col m3 s3">
                                <div class="flex">
                                    <input type="text" ID="txtobdnumber"  required="" class="ui-autocomplete-input" ng-click="OBDNumber()" />
                                    <label><%= GetGlobalResourceObject("Resource", "OBDNumber")%></label>                                    
                                    <span class="errorMsg"></span>
                                 </div>
                            </div>
                            <div class="col m3 s3">
                                <div class="flex">
                                    <input type="text" ID="atcInvoice" SkinID="txt_Auto" class="ui-autocomplete-input" required="" />
                                    <label><%= GetGlobalResourceObject("Resource", "Invoice")%></label>
                                   <%-- <span class="errorMsg"></span>--%>
                                </div>
                            </div>
                           
                            <div class="col m12 s12">
                                <gap5></gap5>
                                 <flex end><button  id="btnimport" type="button" ng-click="GetDetails()"  class="btn btn-primary right"><%= GetGlobalResourceObject("Resource", "search")%><i class="material-icons">search</i></button> </flex>
                            </div>
                        </div>
                    </div>

             <table class="table-striped">
                <thead>
                     <tr>
                         <th> <%= GetGlobalResourceObject("Resource", "Select")%></th>
                         <th> <%= GetGlobalResourceObject("Resource", "LineNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "Part")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "BatchNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "MfgDate")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "ExpDate")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "SerialNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "ProjectRefNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "MRP")%></th>   <%--added by lalitha on 06/03/2019--%>
                         <th> <%= GetGlobalResourceObject("Resource", "PickedQty")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "PendingReturnedQty")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "ReturnQty")%></th>
                        
                     </tr>
                 </thead>
                 <tbody>
                     <tr ng-repeat="cus in Cutomerreturn">
                         <td>  <div class="checkbox"><input type="checkbox" ng-model="cus.Isselected"  id="ng-change-example{{$index+1}}" ng-change="calcualtedimension()"/><label for="ng-change-example{{$index+1}}"></label></div></td>
                         <td >{{ cus.Line }}</td>
                         <td>{{ cus.MCode }}</td>
                         <td>{{ cus.BatchNo }}</td>
                         <td>{{ cus.MfgDate }}</td>
                         <td>{{ cus.ExpDate }}</td>
                         <td>{{ cus.SerialNo }}</td>
                         <td>{{ cus.ProjectRefNo }}</td>
                          <td>{{ cus.MRP }}</td>  <%--MRP added by lalitha on 06/03/2019--%>
                         <td>{{ cus.PickedQty }}</td>
                         <td>{{ cus.PendingReturnQty }}</td>
                         <td><input width100 type="text" ng-model="cus.ReturnQty" onkeypress="return isNumber(event)" ng-Keyup="checkreturnqty(cus)" style="text-align:right; border:0px; border-bottom:1px solid; border-color: var(--paper-grey-300) !important;" /></td>
                         
                     </tr>
                 </tbody>
          </table>
            
            <br />
               <button  id="btnimport" type="button" ng-click="Transfer()" ng-if="hide"  class="btn btn-primary right"> Return <i class="material-icons vl">keyboard_return</i></button> 
        </div>
    </div>
</asp:Content>
