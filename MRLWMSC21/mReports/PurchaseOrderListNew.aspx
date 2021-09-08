<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" Title="Purchase Order Report" CodeBehind="PurchaseOrderListNew.aspx.cs" Inherits="MRLWMSC21.mReports.PurchaseOrderListNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtPODate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $(document).ready(function () {
                $("#txtFromdate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $(this).focus();
                        $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                    }
                });
                $("#txttodate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $(this).focus();
                    }
                });

                $('#txtPODate,#txtFromdate, #txttodate').keypress(function () {
                    return false;
                });
            });
        });


    </script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="PurchaseOrderListNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnTenantID").val(<%=cp.TenantID%>);
        });
    </script>
    <style>
        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }

        .module_Green {
            border-right: none !important;
            border-left: none !important;
        }

        .mytableReportBodyTR tr:nth-child(even) {
            background-color: #fff !important;
        }

    </style>
   
    <div ng-app="myApp" ng-controller="PurchaseOrderListNew" class="container">
        <div ng-show="blockUI">
  <div style="width:100%; height:100vh; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                                
                                <div style="align-self:center;" >
                                        <div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>

                                </div>
                                  
                                </div>
  
</div>
       
        <input type="hidden" id="hdnTenantID" />
        <div>
            <div class="row m0">
                           <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtTenant"  required=""  />                               
                                 <label> Tenant</label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtPartNo" ngchange="getskus()" ng-click="getskus()" required=""  />
                     
                                 <label> <%= GetGlobalResourceObject("Resource", "PONumber")%></label>
                            </div>
                        </div>

                        <div class="col m3">
                            <div class="flex">
                                <input type="text"  ng-model="fromdate" id="txtPODate" onpaste="return false" required="" />
                      
                                 <label><%= GetGlobalResourceObject("Resource", "PODate")%> </label>
                            </div>
                        </div>
                        
                         <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtPOStatus" ngchange="getskus1()" ng-click="getskus1()" required="" />
                       
                                   <label> <%= GetGlobalResourceObject("Resource", "POStatus")%></label>
                            </div>
                         </div>
                       
            </div>
            <div class="row m0">
                 <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtPOType" ngchange="getskus2()" ng-click="getskus2()" required="" />
                                <%--<label>PO Type</label>--%>
                                <label><%= GetGlobalResourceObject("Resource", "POType")%> </label>
                            </div>
                        </div>
                         <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtSupplier" ngchange="getskus3()" ng-click="getskus3()" required="" />
                               <%-- <label>Supplier</label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "Supplier")%></label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" ng-model="fromdate" onpaste="return false" id="txtFromdate" required="" />
                             <%--   <label>From Date</label>--%>
                                   <label> <%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                            </div>
                        </div>
                        
                        <div class="col m3">
                            <div class="flex">
                                <input type="text"  ng-model="todate" onpaste="return false" id="txttodate" required="" />
                               <%-- <label>To Date</label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "ToDate")%></label>
                            </div>
                        </div>
                       

            </div>
            <div class="row m0">
                 <div class=" offset-m9 col m3">
                          
                            <div class="flex__ end">
                               <%-- <button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                                 <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            &nbsp;
                                    <div class="exportto">
                                       <%-- <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>--%>
                                         <a href="#" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "ExportTo")%> &nbsp;<i class="material-icons">cloud_download</i></a>
                                        <ul class="export-menu">
                                            <%-- <li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;PDF</span></li>--%>
                                     <%--<li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "PDF")%></span></li>--%>
                                   <%-- <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>--%>
                                     <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "Excel")%> </span></li>
                                  
                                        </ul>
                                    </div>
                                </div>
                       </div>
            </div>
            </div>
      
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth" ng-show="BIllingReport!=undefined && BIllingReport!=null && BIllingReport.length!=0">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                        
                            <tr class="mytableReportHeaderTR">

                                <th ng-click="sort('Client')"> <%= GetGlobalResourceObject("Resource", "PONumber")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('Client')"><%= GetGlobalResourceObject("Resource", "PODate")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "Tenant")%><span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "Supplier")%> <span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "POType")%> <span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th  ng-click="sort('VLPDNumber')"><%= GetGlobalResourceObject("Resource", "TotalValue")%> <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th  ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Currency")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <%--<th ng-click="sort('MType')">Ex. Rate<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">PO Tax<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Status")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>



                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td>{{BLR.PONumber}}</td>
                                <td>{{BLR.PODate}}</td>
                                <td>{{BLR.Tenant}}</td>
                                <td>{{BLR.Supplier}}</td>
                                <td>{{BLR.POType}}</td>
                                <td style="text-align: center !important;">{{BLR.TotalValue}}</td>
                                <td >{{BLR.Currency}}</td>
                                <%--<td>{{BLR.ExRate}}</td>
                                <td>{{BLR.Tax}}</td>--%>
                                <td align="right">{{BLR.Status}}</td>



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

                    </table>
                </div>

                <table id="tbldata"></table>
                <td class="lineheight"></td>
            </div>
        </div>
    </div>
</asp:Content>
