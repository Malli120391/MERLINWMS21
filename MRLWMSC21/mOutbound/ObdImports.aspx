<%@ Page Title="Outbound Import:." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="ObdImports.aspx.cs" Inherits="MRLWMSC21.mOutbound.OdbImports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <script src="../Scripts/angular.min.js"></script>
      <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
  
  
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
   
    <script src="ObdImports.js"></script>

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
        .txt_Blue_Small {
            min-height: 27px;
        }

    

    </style>
    <div class="dashed"></div>
    <div class="container">
    <div class="float end">
        <div id="divColumnContainer" runat="server">
            <asp:CheckBoxList ID="cbListColumns" runat="server" RepeatColumns="6" CssClass="cbListColumns" Style="width: 100%;" Visible="false"></asp:CheckBoxList>
        </div>
       <%-- <div style="text-align: center;">
            <asp:LinkButton ID="btnGetTemplate" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" OnClick="btnGetTemplate_Click" Style=""> <%= GetGlobalResourceObject("Resource", "GetTemplate")%> <i class="material-icons vl">file_download</i></asp:LinkButton>
        </div>--%>
        <div style="text-align: center;">
            <asp:FileUpload ID="fuExcel" CssClass="fuExcel" runat="server" AllowMultiple="false" accept="excel/*" Visible="false" />
            <input type="hidden" id="hdnExcelName" runat="server" class="hdnExcelName" value="" />
            <asp:LinkButton ID="btnUpload" runat="server" CssClass="btnUpload ui-button-small" OnClick="btnUpload_Click" Style="margin: 20px;" Visible="false"><svg class="upld" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 24 24" xml:space="preserve" width="24" height="24"><g class="nc-icon-wrapper" fill="#0e0e0e"><polygon fill="#0e0e0e" points="6,8 12,1 18,8 13,8 13,17 11,17 11,8 "></polygon> <path data-color="color-2" fill="#0e0e0e" d="M22,21H2v-6H0v7c0,0.552,0.448,1,1,1h22c0.552,0,1-0.448,1-1v-7h-2V21z"></path></g></svg>Upload</asp:LinkButton>
        </div>
    </div>

       <div ng-app="MyApp" ng-controller="createoutbound" >
        <div class="divlineheight"></div>
        <div class="float end">
            <div>
                <input id="filetype" type="file" title="Import To Excel" name="upload" xlsx-model="excel" multiple style="width: 190px;"></div>
          
              <div>
                <%--<button Title="Import To Excel" id="btnimport" type="button" ng-click="ImportData(excel)"  class="btnGetTemplate btn btn-sm btn-primary" style="width:160px;" >Outward Import <i class="fa fa-folder-open" aria-hidden="true"></i></button>--%>
                <button title="Import To Excel" id="btnimport" type="button" ng-click="ImportData(excel)" class="btnGetTemplate btn btn-sm btn-primary" style="width: 160px;"><%= GetGlobalResourceObject("Resource", "OutwardImport")%> <i class="fa fa-folder-open" aria-hidden="true"></i></button>
            </div>
            
            
            
            <div>
                <asp:LinkButton ID="btnGetTemplate" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" OnClick="btnGetTemplate_Click" Style=""> <%= GetGlobalResourceObject("Resource", "GetTemplate")%> <i class="material-icons vl">file_download</i></asp:LinkButton>&nbsp;&nbsp;

            </div>
          
    </div>
    <div class="row">
            <%--<div style="width:1500px;overflow:auto;">--%>
        <div style="overflow:auto;">
              <%--  <div class="divmainwidth" id="div12" style="width: 80%;">
                </div>--%>
               <%-- <div id="div12">

                </div>--%>
              
                <table class="table-striped" style="width:100%;">
               <thead class="">
                <tr>
                    
                    
                 <%--   <th>Tenant Code</th>--%>
                       <th><%= GetGlobalResourceObject("Resource", "TenantCode")%></th>
                    <%--<th>Warehouse Code</th>--%>
                    <th><%= GetGlobalResourceObject("Resource", "WarehouseCode")%> </th>
                   <%-- <th>Customer Code</th>--%>
                     <th> <%= GetGlobalResourceObject("Resource", "CustomerName")%></th>
                    <th>CustomerPhoneno</th>
                    <th> <%= GetGlobalResourceObject("Resource", "CustomerAddress")%></th>
                      <th>Email</th>
                      <th>Zip</th>
                  <%--   <th>Inovice No.</th>--%>
                       <th> <%= GetGlobalResourceObject("Resource", "InoviceNo")%></th>
                   <%-- <th>SO No.</th>--%>
                     <th> <%= GetGlobalResourceObject("Resource", "SONo")%></th>
                    <%--<th>SO Date</th>--%>
                    <th> <%= GetGlobalResourceObject("Resource", "SODate")%></th>
                    <%--<th>SO Qty.</th>--%>
                    <th><%= GetGlobalResourceObject("Resource", "SOQty")%> </th>
                   <%-- <th>Item Code</th>--%>
                     <th> <%= GetGlobalResourceObject("Resource", "PartNo")%></th>
                    <%--<th>Line No.</th>--%>
                    <th> <%= GetGlobalResourceObject("Resource", "LineNo")%></th>
                   <%-- <th>UoM</th>--%>
                     <th> <%= GetGlobalResourceObject("Resource", "UoM")%></th>
                    <%-- <th>UoM Qty.</th>--%>
                     <th><%= GetGlobalResourceObject("Resource", "UoMQty")%> </th>
                  <%--  <th>Mfg. Date</th>  --%>
                      <th> <%= GetGlobalResourceObject("Resource", "MfgDate")%> </th>  
                    <%--<th>Exp. Date</th>--%>
                    <th> <%= GetGlobalResourceObject("Resource", "ExpDate")%> </th>
                    <%--<th>Batch No.</th>--%>
                    <th><%= GetGlobalResourceObject("Resource", "BatchNo")%> </th>
                  <%--  <th>Proj Ref. No.</th>--%>
                      <th><%= GetGlobalResourceObject("Resource", "ProjRefNo")%></th>
               
                     <th><%= GetGlobalResourceObject("Resource", "SerialNo")%></th>
                    <th>AWB No.</th>
                    <th>Courier</th>
                    <th>Priority</th>
                    <th>Due Date</th>
                    <th>Notes</th>
                </tr>
                   </thead>
                    <tbody class="">
                        <tr ng-repeat="obd in obdbdata">
                            
                            <td>{{obd.TenantCode}}</td>
                            <td>{{obd.WHcode}}</td>
                            <td>{{obd.CustomerName}}</td>
                            <td>{{obd.CustomerPhoneno}}</td>
                            <td>{{obd.CustomerAddress}}</td>
                            <td>{{obd.Email}}</td>
                            <td>{{obd.Zip}}</td>
                            <td>{{obd.InvoiceNo}}</td>
                            <td>{{obd.SONumber}}</td>
                            <td>{{obd.SODate}}</td>
                            <td>{{obd.SOQuantity}}</td>
                            <td>{{obd.PartNo}}</td>
                            <td>{{obd.LineNumber}}</td>
                            <td>{{obd.UoM}}</td>
                            <td>{{obd.UoMQuantity}}</td>
                            <td>{{obd.MfgDate}}</td>
                            <td>{{obd.ExpDate}}</td>
                            <td>{{obd.BatchNo}}</td>
                            <td>{{obd.ProjRefNo}}</td>
                            <td>{{obd.SerialNo}}</td>

                             <td>{{obd.AWBNo}}</td>
                             <td>{{obd.Courier}}</td>
                             <td>{{obd.Priority}}</td>
                             <td>{{obd.DueDate}}</td>
                             <td>{{obd.Notes}}</td>
                            
                        </tr>
                    </tbody>
                    </table>
                    </div>
         <div class="divlineheight"></div>
        </div>
    <div align="right" style="margin-right:1%;">
        <br />
         <img src="../Images/bx_loader.gif" id="imgLLoadingSAP" style="width:60px;display:none;" />
            <%--<button type="button" id="btnclick" ng-click="CreateOutbound();" class="btnGetTemplate btn btn-sm btn-primary" >Create Outbound <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
        <button type="button" id="btnclick" ng-click="CreateOutbound();" class="btnGetTemplate btn btn-sm btn-primary" > <%= GetGlobalResourceObject("Resource", "CreateOutbound")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
        <br />
    </div>
  </div>
    <div style="padding: 10px; color: red; font-size: 10pt;">
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
    </div>
        </div>
</asp:Content>
