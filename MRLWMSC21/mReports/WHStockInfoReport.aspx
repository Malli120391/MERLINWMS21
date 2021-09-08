<%@ Page Language="C#" Title="Warehouse Stock Information Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="WHStockInfoReport.aspx.cs" Inherits="MRLWMSC21.mReports.WHStockInfoReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="WHStockInfoReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

       <script type="text/javascript">
        $(document).ready(function () {

            $("#txtDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
            });

        });
    </script>

    <style>
        .tableLoader::before {
            height: 392px;
        }
    </style>
    <div ng-app="myApp" ng-controller="WHStockInfoReport" class="container">
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>
       <div class="row">
            <div class="col m3">
                <div class="flex">
                    <asp:TextBox runat="server" ID="txtWarehouse" ClientIDMode="Static" required=""></asp:TextBox>
                    <label>Warehouse</label>
                    <span class="errorMsg"></span>
                    <asp:HiddenField runat="server" ID="hdnWarehouse" ClientIDMode="Static" Value="0" />
                </div>
            </div>

            <div class="col m3">
                <div class="flex">
                    <asp:TextBox runat="server" ID="txtTenant" ClientIDMode="Static" required=""></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdnTenant" ClientIDMode="Static" Value="0" />
                    <%--<input type="text" id="txtTenant" required="" />--%>
                    <label>Tenant</label>
                    <span class="errorMsg"></span>
                </div>
            </div>

          

           <div class="col m3">
                <div class="flex">
                    <asp:TextBox runat="server" ID="txtDate" ClientIDMode="Static" required=""></asp:TextBox>
                    <label>Date</label>
                    <span class="errorMsg"></span>
                </div>
            </div>

           <div class="col m3">
                <div class="flex">
                    <asp:TextBox runat="server" ID="txtItem" ClientIDMode="Static" required=""></asp:TextBox>
                    <label>Item Code</label>
                    <asp:HiddenField runat="server" ID="hdnItem" ClientIDMode="Static" Value="0" />
                     <asp:HiddenField runat="server" ID="hdnCount" ClientIDMode="Static" Value="0" />
                </div>
            </div>

           <%-- <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtWarehouse" required="" />
                    <label>Warehouse</label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtDate" required="" />
                    <label>Date</label>
                    <span class="errorMsg"></span>
                </div>
            </div>

           <div class="col m3">
               <div class="flex">
                   <input type="text" id="txtItem" required="" />
                   <label>Item Code</label>
               </div>
           </div>            --%>
        </div>

        <div class="row">
            <div class="col m4"></div>
            <div class="col m4">
                <div class="hideContent" style="text-align:center;font-size:15px;font-weight:500">
                    <span>3PL Warehouse</span>                   

                    <%--============ Commented By M.D.Prasad On 20-Aug-2020 for hiding dynamic content
                        <span ng-show="warehouse == 1">3PL Warehouse-01, Amghara</span>
                    <span ng-show="warehouse == 4027">3PL Warehouse-01, Sharq</span>
                    <span ng-show="warehouse != 1 && warehouse != 4027">3PL Warehouse-02, Amghara</span>--%>
                </div>
            </div>
            <div class="col m4" flex end>
                <gap5></gap5>
                <button type="button" ng-click="GetWHStockDataReport()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                <button type="button" class="btn btn-primary" ng-click="exportExcelData()" style="display:none;">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                <asp:LinkButton runat="server" ID="lnkExportData" CssClass="btn btn-primary" OnClick="lnkExportData_Click">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></asp:LinkButton>
                <button type="button" class="btn btn-primary" style="background-color:red !important;" ng-click="GetWHDataReportPDF()">Export <i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
            <table style="width:70%;">
                <tr>
                    <td style="height:20px;width:150px;"><b>Company : </b></td><td>{{TenantData.TenantName}}</td>
                </tr>
                <tr>
                    <td style="height:20px;"><b>Address : </b></td><td>{{TenantData.Address1}}</td>
                </tr>
                <tr>
                    <td style="height:20px;"><b>Contact Person : </b></td><td>{{TenantData.PCPName}}</td>
                </tr>
                <tr>
                    <td style="height:20px;"><b>Email ID : </b></td><td>{{TenantData.PCPEmail}}</td>
                </tr>
                <tr>
                    <td style="height: 20px;"><b>Doc. Date : </b></td>
                    <td>{{TenantData.CurrentDate}}</td>
                </tr>
            </table>
            </div>
        </div>

        <div class="row">
            <div class="col m12">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr>
                                <th>S. No.</th>
                                <th>Item Code</th>
                                <th ng-show="TenantID == 58">Collection</th>
                                <th ng-show="TenantID == 80">Brand</th>
                                <th ng-show="TenantID == 52 || TenantID == 39">PO Number</th>
                                <th ng-show="TenantID == 58">Cus-Design & Color</th>
                                <th ng-show="TenantID == 52 || TenantID == 39">Airway Bill No.</th>

                                <th ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39">Description</th>

                                <th ng-show="TenantID == 58">Size</th>
                                <th ng-show="TenantID == 80">Model</th>
                                <th ng-show="TenantID == 52 || TenantID == 39">Serial No.</th>
                                <th>Category</th>
                                <th>UoM/Qty.</th>
                                <th ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39 && TenantID != 80">Mfg. Date</th>
                                <th ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39 && TenantID != 80">Exp. Date</th>
                                <th ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39 && TenantID != 80">Batch No.</th>
                                <th>On hand Qty.</th>
                                <th>Allocated Qty.</th>
                                <th>Good Qty.</th>
                                <th>Damaged Qty.</th>
                                <th>Volume (CBM)</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="WHS in WHStockData | itemsPerPage:25">
                                <td>
                                    <span ng-show="WHS.MCode != 'Total'">{{$index+1}}</span>
                                    <span ng-show="WHS.MCode == 'Total'"></span>
                                </td>
                                <td>{{WHS.MCode}}</td>
                                <td ng-show="TenantID == 58">{{WHS['Collection']}}</td>
                                <td ng-show="TenantID == 80">{{WHS['PONumber']}}</td>
                                <td ng-show="TenantID == 52 || TenantID == 39">{{WHS['PONumber']}}</td>
                                <td ng-show="TenantID == 58">{{WHS['Cus-Design & Color']}}</td>
                                <td ng-show="TenantID == 52 || TenantID == 39">{{WHS['AirwayBillNo']}}</td>

                                <td ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39">{{WHS['AirwayBillNo']}}</td>

                                <td ng-show="TenantID == 58">{{WHS['Size']}}</td>
                                <td ng-show="TenantID == 80">{{WHS['SerialNo']}}</td>
                                <td ng-show="TenantID == 52 || TenantID == 39">{{WHS['SerialNo']}}</td>
                                <td>{{WHS['Category']}}</td>
                                <td>{{WHS.UoMQty}}</td>
                                <td ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39 && TenantID != 80">{{WHS.MfgDate}}</td>
                                <td ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39 && TenantID != 80">{{WHS.ExpDate}}</td>
                                <td ng-show="TenantID != 58 && TenantID != 52 && TenantID != 39 && TenantID != 80">{{WHS.BatchNo}}</td>
                                <td>{{WHS['OnhandQty']}}</td>
                                <td>{{WHS['AllocatedQty']}}</td>
                                <td>{{WHS.GoodQty}}</td>
                                <td>{{WHS.DamagedQty}}</td>
                                <td>{{WHS.ItemVolume}}</td>  
                            </tr>
                        </tbody>
                         <tfoot>
                            <tr>
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>
                <table id="tbldata"></table>
            </div>
            <div class="divlineheight"></div>

        </div>
</asp:Content>

