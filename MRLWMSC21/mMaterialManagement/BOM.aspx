<%@ Page Title="BOM Creation" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="BOM.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.BOM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
     <script src="../Scripts/angular.min.js"></script>

    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <script src="BOM.js"></script>
 


    <div class="angulardiv container" ng-app="MyApp" ng-controller="BOMDetails">
        <div class="row"><flex end><a style="text-decoration:none;"  href="../mMaterialManagement/BOMList.aspx"><button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()"><i class="material-icons vl">arrow_back</i>&nbsp;Back to List</button></a></flex></div>
       
        <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader">BOM Header <span class="ui-icon"></span></div>
        <div class="ui-Customaccordion" id="PrimaryInformationBody">
            <br />
            <div class="row">
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%--  <asp:TextBox ID="txtrefno" runat="server" required="" Enabled="false"></asp:TextBox>--%>
                            <input type="text" ng-model="BOMHeaderData.BOMRefNo" readonly="true" required="" />
                            <label>BOM Ref. No.</label>
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%-- <asp:DropDownList runat="server" ID="ddlaccount" runat="server" required="" />--%>
                            <select ng-model="BOMHeaderData.AccountId" ng-options="tnt.ID as tnt.Name for tnt in AccountData" style="width: 280px;" required="">
                                <option value="">Select Account</option>
                            </select>
                              <label>Account</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                             <input type="text" id="txtTenant"  ng-model="BOMHeaderData.Tenant" required=""/>
                            <span class="errorMsg"></span>
                            <label>Tenant</label>
                           
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                              <input type="text" id="txtParentPartNo"  ng-model="BOMHeaderData.MCode" required=""/>
                            <span class="errorMsg"></span>
                            <label>Part No.</label>
                           <%-- <asp:HiddenField ID="hifMMID" runat="server" Value="0" />--%>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%--  <asp:TextBox ID="txtUOM" runat="server" required="" MaxLength="30" />--%>
                            <input type="text" id="txtUOM" required="" ng-model="BOMHeaderData.UOM" required=""/>
                            <label>UOM </label>
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%--<asp:TextBox class="p1save excessds" Style="height: 20px;" TextMode="multiline" ID="txtremarks" runat="server" required="" />--%>
                            <input type="text" id="txtremarks" required="" ng-model="BOMHeaderData.Remarks" required=""/>
                            <label>Remarks</label>
                        </div>
                    </div>
                </div>

                <div class="col m6">
                    <gap></gap>
                    <flex end>                    
                        <button type="button" type="button" ID="lnkSavePrimaryInfo" class="btn btn-primary" ng-click="saveBOMHeader()">Save <i class="fa fa-save"></i></button>
                    </flex>
                </div>
            </div>
        </div>

        <div ng-show="IsheaderCreated">
            <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader1">BOM Line Item Details <span class="ui-icon"></span></div>
            
        <div class="ui-Customaccordion" id="PrimaryInformationBody1">
           <gap></gap>
                <div flex end>
                    <button type="button" id="lnkAddSupplier" class="btn btn-primary " ng-click="cleardataWhenOpen()" data-toggle="modal">Add Line Items <i class="material-icons">add</i></button>
                     
                </div> <gap></gap>
            <div>
                
                <table align="center" class="table-striped" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr class="">
                            <th>S. No.</th>
                            <th>Part No.</th>
                            <th>UOM</th>
                            <th>Quantity</th>
                          <%--  <th>Enter Qty.</th>--%>
                            <th>Update</th>
                            <th>Delete</th>
                          

                        </tr>
                    </thead>

                    <tbody dir-paginate="bom in BomDetailsdata  |itemsPerPage:25" pagination-id="main">
                        <tr class="">
                            <td>{{$index+1}}</td>
                            <td>{{bom.MCode}}</td>
                            <td>{{bom.UOM}}</td>
                           <%-- <td>{{bom.Quantity}}</td>--%>
                            <td><div class="flex"><input type="text" style="width:120px !important;margin: 0; -webkit-padding-before: 0px !important;-webkit-padding-after: 0px !important;" ng-model="bom.UPdatedQuantity" onkeypress="return isNumber(event)" required/></div></td>
                            <td><a ng-click="updateBOMDetails(bom)"><i class="material-icons">update</i><em class="sugg-tooltis" style="left: 32px;">Update</em></a></td>
                            <td><a ng-click="DeleteBOMDetails(bom)"><i class="material-icons">delete</i><em class="sugg-tooltis" style="left: 32px;">Delete</em></a></td>
                            


                        </tr>
                    </tbody>
                </table>

                 <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
                <!-- Modal -->
                <div id="SupModal" class="modal">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                <h4 class="modal-title" style="display: inline !important;">Add Line Items</h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" onclick="myKitclear();" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mySupForm">

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                               <%-- <asp:TextBox ID="txtlinepartno" runat="server" required="" MaxLength="30" />--%>
                                                <input type="text" id="txtlinepartno"  ng-model="BomDetails.MCode" required=""/>
                                                <span class="errorMsg">*</span>
                                                <label>Part No. </label>


                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                               <%-- <asp:TextBox ID="txtRTUoM" runat="server" required="" MaxLength="30" />--%>
                                                <input type="text" id="txtRTUoM" ng-model="BomDetails.UOM" required=""/>
                                                <span class="errorMsg">*</span>                                               
                                                <label>UoM / Qty. </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" ID="txtQuantity"  required="" MaxLength="30" ng-model="BomDetails.Quantity" onkeypress="return isNumber(event)" required=""/>
                                                <span class="errorMsg">*</span>
                                                <label>Quantity </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>










                            </div>
                            <div class="modal-footer">
                                <input type="hidden" id="MMT_SUPPLIER_ID" />
                                <%--<asp:CheckBox runat="server" ID="chkBDDelete" Text="Delete" onclick="CheckIsDelted(this);" />--%>
                                <button type="button" class="btn btn btn-primary" style="color: #fff !important;" ng-click="cleardata();">Clear</button>
                                <button type="button" class="btn btn btn-primary" style="color: #fff !important;" data-dismiss="modal">Close</button>
                                <button type="button" type="button" ID="lnkSaveSecondaryInfo" class="btn btn-primary" ng-click="saveBOMDetails()">Save</button>
                                     <%--   <asp:LinkButton runat="server" ID="lnkButUpdate" ValidationGroup="updateBOMItems"  CssClass="ui-btn ui-button-large">
                                                                Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                        </asp:LinkButton>--%>
                                    

                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal -->
                <div class="row" style="margin-top: 8% !important; margin: auto !important;" id="SupDetailsTable">
                    <%--Supplier Details Table Append--%>
                </div>
            </div>
        </div>
        </div>
       
       
        
    </div>
</asp:Content>
