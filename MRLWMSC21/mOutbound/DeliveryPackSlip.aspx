<%@ Page Title=" Delivery Pack Slip:." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="DeliveryPackSlip.aspx.cs" Inherits="MRLWMSC21.mOutbound.DeliveryPackSlip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
    <script type="text/javascript" src="../mManufacturingProcess/Scripts/ben_Print.js"></script>
    <script src="../Scripts/angular.min.js"></script>
      <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="DeliveryPackSlip.js"></script>
    <style>
    #BrowserPrintDefaults{display:none} 
    </style>

    <script type="text/javascript">
        function printDiv() {

            // Print the DIV.
            $(".PrintListcontainer").print();
        }
        jQuery(document).bind("keyup keydown", function (e) {
            if (e.ctrlKey && e.keyCode == 80) {
                $('#linkprint').click();
                return false;
            }
        });
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <div>
        <table width="80%" align="center" >
             <tr> 
                    
                      <%--  <td colspan="3" align="right" valign="bottom" class="NoPrint">
                            <br />
                            <asp:LinkButton runat="server" ID="lnkbackToList" SkinID="lnkButEmpty" Text="Back to List" />
                            &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" id="linkprint" style="text-decoration:none;" onclick="javascript:printDiv();" class="ButPrint">Print</a> &nbsp; <img src="../Images/redarrowright.gif"  border="0" />
                        </td>--%>
                </tr>
        </table>
    </div>
    <div id="printArea" class="PrintListcontainer" align="center">
        <LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">
        <table width="80%" align="center" >
       <%-- <tr> 
                    
                        <td colspan="3" align="right" valign="bottom" class="NoPrint">
                            <br />
                            <asp:LinkButton runat="server" ID="lnkbackToList" SkinID="lnkButEmpty" Text="Back to List" />
                            &nbsp;&nbsp; &nbsp;&nbsp;<a href="#" id="linkprint" style="text-decoration:none;" onclick="javascript:printDiv();" class="ButPrint">Print</a> &nbsp; <img src="../Images/redarrowright.gif"  border="0" />
                        </td>
                </tr>--%>
                <tr>
                    <td  colspan="3">
                        <table  cellpadding="3" cellspacing="3" border="0" width="95%">
                            <tr>
                                <td> <img id="Img1"  runat="server"   src="~/Images/RT_Logo_icon.png" style="visibility:hidden;" width="80"   border="0" alt=""></td>
                                <td align="center" style="text-align:center;"><font  size="6"> Delivery Pack Slip </font></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
        <tr>
                    <td colspan="3">
                        <hr style="height:0.5px;color:#000;border-color:#000;background-color:#000;"/>
                    </td>
                </tr>
        <tr>
            <td>
                <asp:Literal ID="ltOBDNumber" runat="server" ></asp:Literal>
            </td>
            <td align="right">
                <table style=" width: fit-content;float:right;">
                    <tr>
                        <td>
                            <b>Customer</b> 
                        </td>
                        <td>
                            <asp:Literal ID="ltCustomer" runat="server" ></asp:Literal>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <b>Requested By </b>
                        </td>
                        <td>
                            <asp:Literal ID="ltRequestedBy" runat="server" ></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <b>Delv. Doc. Date</b> 
                        </td>
                        <td>
                            <asp:Literal ID="ltDelvDocDate" runat="server" ></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Literal ID="ltTotalCount" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="gvDeliveryPackSlip" runat="server" OnRowDataBound="gvDeliveryPackSlip_DataBound" SkinID="gvIBGray" Visible="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Line #">
                            <ItemTemplate>
                                <asp:Literal ID="ltLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>'></asp:Literal>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Number">
                            <ItemTemplate>
                                <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>'></asp:Literal>
                                <br />
                                <span style="color:#1287a1;font-size:14px"> <nobr> <%# DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString() %>  </nobr> </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IUoM/Qty">
                            <ItemTemplate>
                                <asp:Literal ID="ltuomqty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UOM/Qty").ToString() %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KitCode">
                            <ItemTemplate>
                                <asp:Literal ID="ltKitCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Literal ID="ltSplitMaterialStorage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SplitLocation").ToString() %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </td>
        </tr>
    </table>

    </div>

  

       <div ng-app="MyApp" ng-controller="PackSlip">
          
         <div class="divlineheight"></div>
          <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;" >
                <div class="divmainwidth" >
               <%-- <table class="mytableOutbound" id="tbldatas" >
                    <thead class="mytableOutboundHeaderTR">
                        <tr class="mytableReportItemsHeaderTR">
                            <th colspan="9" class="thalign">
                                <table class="Headertablewidth">
                                     <tr>
                                        <td>
                                            <div class="buttonrowstyle">
                                                <button type="button" id="btnPdf" class="button button3" ng-click="PrintBoxLabels(PL.obdid)">Print &nbsp;<i class="fa fa-print" aria-hidden="true"></i></button>
                                               
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                        </tr>
                        
                        
                    </thead>
                    
                   
                    
                    

                </table>--%>
                    
                    <table >
                       <thead class="table">
                            
                        <tr class="">
                            <th>Part No.</th>
                             <th>Description</th>
                            <th>Picked Qty.</th>
                            <th>Vehicle No.</th>
                             <th>Vehicle Type</th>
                             <th>Driver Name</th>
                             <th>Dock Name</th>
                             <th>Driver Mobile No.</th>
                        </tr>
                    </thead>
                    <tbody class="">
                        <tr dir-paginate="PL in PackList|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                            <td>{{PL.MCode}}</td>
                             <td>{{PL.MDescription}}</td>
                                <td>{{PL.PickedQty}}</td>
                                <td>{{PL.VehicleNo}}</td>
                                <td>{{PL.VehicleType}}</td>
                             <td>{{PL.DriverName}}</td>  
                             <td>{{PL.DockName}}</td>  
                                <td>{{PL.DriverMobileNo}}</td>
                                 
                                
                        </tr>
                    </tbody>
                        <tfoot>
                            <tr class="">
                                <td colspan="9">
                                    <div style="text-align:right;">
                                        <button type="button" id="btnPrints" class=" btht btn btn-small right" ng-click="PrintBoxLabels(PL.obdid)" ><i class="material-icons">&#xE8AD;</i></button>  <p></p>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>

                <table id="tbldata"></table>
            </div>
        <div class="divlineheight"></div>
              
          </div>     
     </div>
        </div>
    
</asp:Content>
