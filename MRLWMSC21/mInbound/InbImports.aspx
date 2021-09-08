<%@ Page Title="Inbound Import:." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="InbImports.aspx.cs" Inherits="MRLWMSC21.mInbound.InbImports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">

    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>

    <script src="InbImports.js"></script>
   
    <style>
        .module_login {
            border: 0px;
        }

        .upld {
            width: 17px;
            margin-right: 8px;
            position: relative;
            top: 6px;
        }

        .sssss {
            width: 22px;
            height: 16px;
            margin-right: 5px;
        }

        #MainContent_IBContent_btnGetTemplate {
            display: inline-flex;
        }

        .txt_Blue_Small {
            min-height: 27px;
        }
    </style>
    <div class="dashed"></div>
    <div class="pagewidth">

        <div ng-app="MyApp" ng-controller="createinbound">
            <div class="divlineheight"></div>
            <div class="float end">
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
                <!-- Globalization Tag is added for multilingual  -->
                <div>
                    <input id="filetype" type="file" title="Import To Excel" name="upload" xlsx-model="excel" multiple style="width: 190px;"></div>

                <div>
                    <%--<button Title="Import To Excel" id="btnimport" type="button" ng-click="ImportData(excel)"  class="btnGetTemplate btn btn-sm btn-primary" style="width:140px;" > Get Inward List <i class="fa fa-folder-open" aria-hidden="true"></i></button>--%>
                    <button title="Import To Excel" id="btnimport" type="button" ng-click="ImportData(excel)" class="btnGetTemplate btn btn-sm btn-primary" style="width: 140px;"><%= GetGlobalResourceObject("Resource", "GetInwardList")%> <i class="fa fa-folder-open" aria-hidden="true"></i></button>
                </div>

                <div class="float end">
                    <div id="divColumnContainer" runat="server">
                        <%--<input type="checkbox" id="cbCheckAll" class="cbCheckAll" checked="checked" />&nbsp;Check All<br /><br />--%>
                        <asp:CheckBoxList ID="cbListColumns" runat="server" Visible="false" RepeatColumns="8" CssClass="cbListColumns" Style="width: 100%;"></asp:CheckBoxList>
                    </div>
                    &nbsp;
        <div style="text-align: center;">
            <%--<asp:LinkButton ID="btnGetTemplate" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" OnClick="btnGetTemplate_Click" style="">Get Template <i class="material-icons vl">file_download</i></asp:LinkButton>--%>
            <asp:LinkButton ID="btnGetTemplate" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" OnClick="btnGetTemplate_Click" Style=""> <%= GetGlobalResourceObject("Resource", "GetTemplate")%>  <i class="material-icons vl">file_download</i></asp:LinkButton>
        </div>
                    <div style="text-align: center;">
                        <asp:FileUpload ID="fuExcel" Visible="false" CssClass="fuExcel" runat="server" AllowMultiple="false" accept="excel/*" />
                        <input type="hidden" id="hdnExcelName" runat="server" class="hdnExcelName" value="" />
                        <asp:LinkButton ID="btnUpload" runat="server" Visible="false" CssClass="btnUpload btn btn-sm btn-primary" OnClick="btnUpload_Click" Style="margin: 20px;"><svg class="upld" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 24 24" xml:space="preserve" width="24" height="24"><g class="nc-icon-wrapper" fill="#0e0e0e"><polygon fill="#0e0e0e" points="6,8 12,1 18,8 13,8 13,17 11,17 11,8 "></polygon> <path data-color="color-2" fill="#0e0e0e" d="M22,21H2v-6H0v7c0,0.552,0.448,1,1,1h22c0.552,0,1-0.448,1-1v-7h-2V21z"></path></g></svg> <%= GetGlobalResourceObject("Resource", "Upload")%></asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="row" style="margin: 0;" style="width: 80%;">
                <div class="col-md-12 col-lg-12 table-responsive" style="overflow-x: auto; margin: 15px; padding: 0;">

                    <table class="table-striped">
                        <thead class="mytableOutboundHeaderTR">
                            <tr style="height: 30px;">

                                <th><%= GetGlobalResourceObject("Resource", "TenantCode")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "SupplierCode")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "PONumber")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "PODate")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "LineNumber")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "PartNo")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "UoM")%></th>
                                <th>UOM Qty.</th>
                                <th><%= GetGlobalResourceObject("Resource", "MfgDate")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "ExpDate")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "BatchNo")%></th>
                                <th>Serial No</th>
                                <th><%= GetGlobalResourceObject("Resource", "ProjectRefNo")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "InoviceNo")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "InvoiceDate")%></th>
                                <th>PO Qty.</th>
                                <th><%= GetGlobalResourceObject("Resource", "InvoiceQty")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "WarehouseCode")%></th>
                                <th>MRP</th>
                            </tr>
                        </thead>
                        <tbody class="mytableOutboundBodyTR">
                            <tr ng-repeat="inb in inbdata">
                                <td>{{inb.TenantCode}}</td>
                                <td>{{inb.SupplierCode}}</td>
                                <td>{{inb.PONumber}}</td>
                                <td>{{inb['PODate(dd/MM/yyyy)']}}</td>
                                <td>{{inb.LineNumber}}</td>
                                <td>{{inb.PartNo}}</td>
                                <td>{{inb.UoM}}</td>
                                <td number>{{inb.UOMQty}}</td>
                                <td>{{inb['MfgDate(dd/MM/yyyy)']}}</td>
                                <td>{{inb['ExpDate(dd/MM/yyyy)']}}</td>
                                <td>{{inb.BatchNo}}</td>
                                <td>{{inb.SerialNo}}</td>
                                <td>{{inb.ProjRefNo}}</td>
                                <td>{{inb.InvoiceNo}}</td>
                                <td>{{inb['InvoiceDate(dd/MM/yyyy)']}}</td>
                                <td number>{{inb.POQty}}</td>
                                <td number>{{inb.InvoiceQuantity}}</td>
                                <td>{{inb.WarehouseCode}}</td>
                                <td number>{{inb.MRP}}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="divlineheight"></div>
            </div>
            <div align="right" style="margin-right: 1%;">
                <%--  <img src="../Images/bx_loader.gif" id="imgLLoadingSAP" style="width:60px;display:none;" />--%>
                <%--<button type="button" id="btnclick" ng-click="CreateInbound();" class="btnGetTemplate btn btn-sm btn-primary" >Create Inbound <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
                <button type="button" id="btnclick" ng-click="CreateInbound();" class="btnGetTemplate btn btn-sm btn-primary"><%= GetGlobalResourceObject("Resource", "CreateInbound")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
                <br />
            </div>
        </div>
    </div>

    <div style="padding: 10px; color: red; font-size: 10pt;">
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
    </div>


</asp:Content>
