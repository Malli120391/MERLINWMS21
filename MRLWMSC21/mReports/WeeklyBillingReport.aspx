<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/mReports/mReport.master" CodeBehind="WeeklyBillingReport.aspx.cs" Inherits="MRLWMSC21.mReports.WeeklyBillingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <script type="text/javascript">
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

            $('#txtFromdate, #txttodate').keypress(function () {
                return false;
            });
        });

    </script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="WeeklyBillingReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <%-- <script type="text/javascript">
         $(document).ready(function () {
             $("#hdnTenantID").val(<%=cp.TenantID%>);
           });
    </script>--%>
    <style>
        .buttonrowstyle {
            margin:0px !important;
        }
    </style>
    <link href="../Content/app.css" rel="stylesheet" />
   
    <table class="tbsty">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                        <div><a href="../Default.aspx">Home</a> / Reports / <span class="FormSubHeading">3PL / Billing Report</span></div>
                </td>
             </tr>
        </tbody>
    </table>
    <div ng-app="myApp" ng-controller="WeeklyBillingReport" class="pagewidth">
        <div class="divlineheight"></div>
        <div>
            <div class="divlineheight"></div>
            <div class="row">
                <div class="col m5">
         
                </div>
                <div class="col m7">
                    <div class="flex__ right">
                        <div class="flex"><input type="text" id="txtTenant" placeholder="Tenant" ngchange="getTenant()" ng-click="getTenant()" class="TextboxInventoryAuto" style="margin-bottom:0px !important;" /><span class="errorMsg"></span></div>&nbsp;&nbsp;
                        
                        <%--<input list="dpo" id="txtTenantName" placeholder="Tenant" class="TextboxInbound" ng-model="tn.TenantName" class="form-control" ng-change="GetAllTenants()"/>
                            <datalist id="dpo" >
                           <option  ng-repeat="tn in Tenants" id="{{tn.TenantId}}"  value="{{tn.TenantName}}"></option>
                            </datalist>--%>
                        <div class="flex"><input type="text" placeholder="From Date" class="" style="width: 120PX;margin-bottom:0px !important;" onpaste="return false;" ng-model="fromdate" id="txtFromdate" required=""/><span class="errorMsg"></span></div>&nbsp;&nbsp;
                        <div class="flex"><input type="text" placeholder="To Date" class="" style="width: 120PX;margin-bottom:0px !important;" onpaste="return false;"  ng-model="todate" id="txttodate" required=""/><span class="errorMsg"></span></div>&nbsp;&nbsp;
                        <button type="button" ng-click="Getgedetails()"  class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        &nbsp;
                            <div class="exportto">
                                <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>
                                <ul class="export-menu">
                                    <li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;PDF</span></li>
                                    <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>
                                    <li><span id="btnTxt" class="buttons button4" ng-click="exportTxt()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;Txt</span></li>
                                    <li><span id="btnWord" class="buttons button2 hidden" ng-click="exportWord()"><i class="fa fa-file-word-o" aria-hidden="true"></i>&nbsp;&nbsp;Word</span></li>
                                    <li><span id="btnCsv" class="buttons button1" ng-click="exportCsv()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;CSV</span></li>
                                    <li><span id="btnXML" class="buttons button6" ng-click="exportXml()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;XML</span></li>
                                </ul>
                            </div>
                    </div>

                </div>

            </div>

        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <div class="divlineheight"></div>
     
                    <div class="divlineheight"></div>
                    <div style="display: none; justify-content:space-between; padding:0px 15px;" class="divBillTo">
                        
                                            <div class="common" style="width:30%;">
                                              <h3> Bill To:</h3>
                                            <div class="fixed__"  ng-repeat="ADD in Address">
                                            
                                                    <div>
                                                        {{ADD.TenantName}}
                                                        <br />
                                                        {{ADD.Address1}}
                                                        <br />
                                                        {{ADD.Address2}}
                                                        <br />
                                                        {{ADD.City}}
                                                        <br />
                                                        {{ADD.State}}
                                                        <br />
                                                        {{ADD.ZIP}}<br />
                                                    </div>
                                                </div>
                                            </div>
                                                    <div class="common" style="width:30%;">
                                                                   <h3> Bill From:</h3>
                                                              
                                                                        <%--<div>
                                                                            TRANSCRATE INTERNATIONAL LOGISTICS
                                                                            <br />
                                                                            540 SAFAT MIRQAB,
                                                                            <br />
                                                                            KUWAIT 13006 JASSIM BUDAI STREET OFFICE-23,
                                                                            <br />
                                                                            PLOT 31 BLOCK 3
                                                                            <br />
                                                                        </div>--%>
                                                        <div class="fixed__"  ng-repeat="ACC in AccAddress">
                                                            <div>
                                                                {{ACC.Account}}
                                                                <br />
                                                                {{ACC.ComapanyName}}
                                                            </div>
                                                        </div>
                                                    </div>
                                 
                        </div>
                    <div class="divlineheight"></div>
                    <table class="mytableOutbound" id="tbldata">
                        <thead>
                            <tr class="mytableReportHeaderTR">

                                <th colspan="4" ng-click="sort('MType')">Inbound & Outbound Charges<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr class="fixed__" ng-repeat="INB in InboundReport">
                                <td>{{INB.Week}}</td>
                                <td style="text-align: center">{{INB.Charge}}</td>
                                 <td style="width:120px;">{{INB.QTY}}</td>
                                <td style="width:120px;">{{INB.UnitCost}}</td>
                            </tr>
                            <tr class="fixed__">
                                <td>Total</td>
                                <td>INR. {{ICoulmn}}</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                               
                            </tr>
                        </tbody>
                    </table>
                    <div class="divlineheight"></div>
                    <table class="mytableOutbound" id="tbldatas1">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th colspan="2" ng-click="sort('MType')">Storage Charges<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>
                        </thead>

                        <tbody class="mytableReportBodyTR">
                            <tr class="fixed__" ng-repeat="SP in StorageReport">
                                <td>{{SP.Week}}</td>
                                <td align="center">{{SP.Charge}}</td>                                
                            </tr>
                            <tr class="fixed__">
                                <td>Total </td>
                                <td>INR. {{column}}</td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="divlineheight"></div>
                    <table class="mytableOutbound" id="tbldatas2">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th colspan="2" ng-click="sort('MType')">Inventory Charges<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>


                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr class="fixed__" ng-repeat="IP in InventoryReport">
                                <td>{{IP.Week}}</td>
                                <td style="text-align: center">{{IP.Charge}}</td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="divlineheight"></div>
                    <table class="mytableOutbound" id="tbldatas3">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th colspan="2" ng-click="sort('MType')">Grand Total<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>


                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr class="fixed__">
                                <td></td>
                                <td>INR. {{GrandTotal}}</td>

                            </tr>
                        </tbody>
                    </table>

                </div>

                <table id="tbldataInb" ></table>
                <%-- <table id="tbldataSto"></table>
                 <table id="tbldataInv"></table>
                <table id="tbldataGT"></table>--%>

                <div class="divlineheight"></div>
            </div>
        </div>
    </div>
    <style>
        .fixed__ td {
            width: 50%;
            text-align: left !important;
        }
        .fixed_tr{
            
    width: 70% !important;
    text-align: center !important;
        }

        .common {
            border: 1px solid #455b7c;
        }

        .common h3{
            background:#455b7c;
            color: #fff;
            margin:0;
            padding:3px;
            font-size: 14px;
    font-weight: normal;
        }

        .common div{
            padding: 5px;
    font-size: 14px;
    font-weight: normal;
    line-height: 24px;
        }
    </style>
</asp:Content>
