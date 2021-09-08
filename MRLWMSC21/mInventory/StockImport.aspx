<%@ Page Title="Stock Import " Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="StockImport.aspx.cs" Inherits="MRLWMSC21.mInventory.StockImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
   
   <script src="../Scripts/angular.min.js"></script>
      <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
  
  
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
   
    <script src="StockImport.js"></script>
    <%--<script>
        $(document).ready(function () {
            $('#<%=fuExcel.ClientID %>').change(function () {
                var fileExtension = ['xls', 'xlsx'];
                if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                    showStickyToast(false, "Only '.xls or .xlsx' format is allowed");
                    $('#<%=fuExcel.ClientID %>').val('');
                    return false;
                }
            });
            $('.btnUpload').click(function () {
                if ($('#<%=fuExcel.ClientID %>').val() == "") {
                    showStickyToast(false, "Please upload excel template");
                    $('#<%=fuExcel.ClientID %>').val('');
                    return false;
                }
                else {
                    var fileExtension = ['xls', 'xlsx'];
                    if ($.inArray($('#<%=fuExcel.ClientID %>').val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                        $('#<%=fuExcel.ClientID %>').val('');
                        showStickyToast(false, "Only '.xls or .xlsx' formats are allowed");
                        return false;
                    }
                }
            });

            $('.cbCheckAll').change(function () {
                if ($(this).is(':checked')) {
                    $('.cbListColumns input[type=checkbox]').each(function () {
                        //alert($(this).prop('disabled'));
                        if ($(this).prop('disabled') == false)
                        { 
                            $(this).prop("checked", true);
                        }
                    });
                    //$('.cbListColumns input[type=checkbox]').prop("checked", true);
                } else {
                    $('.cbListColumns input[type=checkbox]').each(function () {
                        if ($(this).prop('disabled') == false) {
                            $(this).prop("checked", false);
                        }
                    });
                    //$('.cbListColumns input[type=checkbox]').prop("checked", false);
                }
            });
        });
    </script>--%>
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
        .sssss {    width: 22px;
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





    <div ng-app="MyApp" ng-controller="Stock">
        <div class="divlineheight"></div>
        <div class="float end">
            
 <!-- Globalization Tag is added for multilingual  -->
                   <div><input id="filetype" type="file"  Title="Import To Excel" name="upload"   xlsx-model="excel" multiple style="width:190px;"></div>
          
                   <div>
                     
                      
                       <button Title="Import To Excel" id="btnimportstock" type="button" ng-click="ImportStockData(excel)"  class="btnGetTemplate btn btn-sm btn-primary" style="width:140px;" >  <%= GetGlobalResourceObject("Resource", "ImportFile")%> <i class="fa fa-folder-open" aria-hidden="true"></i></button>
               
                       </div>
   
        <div class="float end">
        <div id="divColumnContainer" runat="server">
          <%--<input type="checkbox" id="cbCheckAll" class="cbCheckAll" checked="checked" />&nbsp;Check All<br /><br />--%>
            <asp:CheckBoxList ID="cbListColumns" runat="server" Visible="false" RepeatColumns="8" CssClass="cbListColumns" style="width:100%;"></asp:CheckBoxList>
        </div>&nbsp;
        <div style="text-align:center;">
            <%--<asp:LinkButton ID="btnGetTemplate" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" OnClick="btnGetTemplate_Click" style="">Get Template <i class="material-icons vl">file_download</i></asp:LinkButton>--%>
           <a class="btn btn-primary" href="../Template/Template_StockTake.xls"="../Template/Template_StockTake.xls">  <%= GetGlobalResourceObject("Resource", "GetTemplate")%>  <i class="material-icons vl">file_download</i></a>
        <%--    <asp:LinkButton ID="btnGetTemplate" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" OnClick="btnGetTemplate_Click" style=""> <%= GetGlobalResourceObject("Resource", "GetTemplate")%>  <i class="material-icons vl">file_download</i></asp:LinkButton>
    --%>    </div>
        <div style="text-align:center;">
            <asp:FileUpload ID="fuExcel" Visible="false" CssClass="fuExcel" runat="server" AllowMultiple="false" accept="excel/*" />
            <input type="hidden" id="hdnExcelName" runat="server" class="hdnExcelName" value=""/>
           <asp:LinkButton ID="btnUpload" runat="server" Visible="false" CssClass="btnUpload btn btn-sm btn-primary" OnClick="btnUpload_Click" style="margin:20px;"><svg class="upld" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 24 24" xml:space="preserve" width="24" height="24"><g class="nc-icon-wrapper" fill="#0e0e0e"><polygon fill="#0e0e0e" points="6,8 12,1 18,8 13,8 13,17 11,17 11,8 "></polygon> <path data-color="color-2" fill="#0e0e0e" d="M22,21H2v-6H0v7c0,0.552,0.448,1,1,1h22c0.552,0,1-0.448,1-1v-7h-2V21z"></path></g></svg> <%= GetGlobalResourceObject("Resource", "Upload")%></asp:LinkButton>
        </div></div>    
        </div>
    <div class="row" style="margin: 0;" style="width: 80%;" >
            <div class="col-sm-12 col-lg-12" style=" overflow: auto; margin: 15px; padding: 0;">
              <%--  <div class="divmainwidth" id="div12" style="width: 80%;">
                </div>--%>
               <%-- <div id="div12">

                </div>--%>
              
                <table class="table-striped">
               <thead class="mytableOutboundHeaderTR">
                <tr style="height:30px;">
                    
                    <th> <%= GetGlobalResourceObject("Resource", "TenantCode")%></th>
                  
                    <th><%= GetGlobalResourceObject("Resource", "ItemCode")%></th>
                     <th><%= GetGlobalResourceObject("Resource", "Location")%></th>
                    <th><%= GetGlobalResourceObject("Resource", "UoM")%></th>
                    <th><%= GetGlobalResourceObject("Resource", "Qty")%></th>
                    <th><%= GetGlobalResourceObject("Resource", "MfgDate")%></th>   
                    <th><%= GetGlobalResourceObject("Resource", "ExpDate")%></th>
                    <th><%= GetGlobalResourceObject("Resource", "BatchNo")%></th>
                  
                </tr>
                   </thead>
                    <tbody class="mytableOutboundBodyTR">
                        <tr ng-repeat="inb in stockdata">
                            <td>{{inb.TenantCode}}</td>  
                            <td>{{inb.MaterialCode}}</td>
                            <td>{{inb.Bin}}</td>
                            <td>{{inb.UoM}}</td>
                            <td>{{inb.Quantity}}</td>
                            <td>{{inb.MfgDate}}</td>
                            <td>{{inb.ExpDate}}</td>
                            <td>{{inb.BatchNo}}</td>
                        
                        </tr>
                    </tbody>
                    </table>
                    </div>
         <div class="divlineheight"></div>
        </div>
    <div align="right" style="margin-right:1%;">
       <%--  <img src="../Images/bx_loader.gif" id="imgLLoadingSAP" style="width:60px;display:none;" />--%>
            <%--<button type="button" id="btnclick" ng-click="CreateInbound();" class="btnGetTemplate btn btn-sm btn-primary" >Create Inbound <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
           <button type="button" id="btnclick1" ng-click="CreateStock();" class="btnGetTemplate btn btn-sm btn-primary" ><%= GetGlobalResourceObject("Resource", "ImportStock")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
      
        <br />
    </div>
  </div>
    </div>

     <div style="padding: 10px; color: red; font-size: 10pt;">
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
    </div>
    

</asp:Content>
