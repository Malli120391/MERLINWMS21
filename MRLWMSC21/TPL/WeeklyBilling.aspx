<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WeeklyBilling.aspx.cs" Inherits="MRLWMSC21.TPL.WeeklyBilling" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <script src="Scripts/dist/ag-grid.js"></script>
    <link href="Scripts/dist/styles/ag-grid.css" rel="stylesheet" />
   <script src="Scripts/jquery-ui.min.js"></script>

   <%-- <script src="Scripts/bootstrap-datepicker.js"></script>
    <link href="Styles/bootstrap-datepicker.css" rel="stylesheet" />--%>



    <script>
        //$(function () {
        //    $(".CalDate").datepicker({
        //        autoclose: true,
        //        format: "MM-yyyy",
        //        viewMode: "months",
        //        minViewMode: "months"
        //    });
        //});
    </script>
    <style>
        body.loading {
            cursor: wait;
        }

        .divReportContainer {
            border: 0px solid red;
        }

        .divControls {
            text-align: right;
        }

        .form-control {
            display: block;
            width: 85%;
            /*height: 20px;*/
            padding: 0 5px 0 5px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out.15s,box-shadow ease-in-out .15s;
            border: 1px solid lightgrey;
            /*border-left: 5px solid #337ab7;*/
        }

        .ag-blue .ag-header {
            background-color: #2E6292 !Important;
            font-size: 14px;
        }

        .ag-header-group-cell-label {
            text-align: center;
        }

        .ag-blue .ag-root {
            font-size: 14px;
        }

        input[type="date"]::-webkit-inner-spin-button {
            display: none;
        }

        .divShowInfo {
            text-align: center;
            font-size: 11pt;
        }

            .divShowInfo .lblTitle {
                color: #336699;
            }

            .divShowInfo .lblValue {
                color: midnightblue;
                margin-left: 5px;
            }

        .spanHeads {
            display: inherit;
            font-size: 9pt;
            color: darkgrey;
        }

        .ExportExcelIcon {
            font-size: 15pt;
            color: green;
        }
       
        .ui-menu .ui-menu-item a:hover, .ui-widget-content .ui-state-hover, .ui-widget-header .ui-state-hover {
            font-size:inherit !important;
            /*font-weight:bold;*/
        }
   
.fa-file-excel-o {
margin-top:22px;
}
.ag-blue .ag-row-even {
    background-color:ghostwhite;
}
.divReportagGrid {
    /*height: 400px; width: 1060px;*/
       height: 400px; width: 100% !important;
}
.divLoading {
    background-color: white;
    border-radius: 25px;
    text-align: center;
    position: absolute;
    top: 50%;
    left: 47%;
    padding: 5px;
   box-shadow: 0px 0px 20px 12px white;
    background-image:url("../Images/agGridLoader.gif");
    width:50px;
    height:50px;   
    background-repeat: no-repeat;
    background-position: center;
}

        .datepicker {
            position: absolute;
            background: white;
            display: none;
            border: 1px solid rgba(0, 0, 0, .15);
            box-shadow: 0 6px 12px rgba(0, 0, 0, .175);
        }

         .btnClearURLFilters {
            display: none;
            cursor: pointer;
            font-size: 15pt;
            color: red;
        }

            .btnClearURLFilters:hover {
                color: grey;
            }

       
    </style>
   <%-- <script src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
    <script src="Scripts/jsPdf.js"></script>

    <script>
        $(function () {

            $('.ExportPdfIcon').click(function () {
               
                var printDoc = new jsPDF();
                printDoc.fromHTML($('#tdReports').get(0), 10, 10, { 'width': 180 });
                printDoc.autoPrint();
                printDoc.output("dataurlnewwindow"); // this opens a new popup,  after this the PDF opens the print window view but there ar
            });
        });
</script>--%>

      
    <script>
        $(document).ready(function () {
            $("#<%= this.CalDateFrom.ClientID %>").datepicker({ dateFormat: "dd-mm-yy" }); //, minDate: 0
            $("#<%= this.CalDateTo.ClientID %>").datepicker({ dateFormat: "dd-mm-yy" }); //, minDate: 0

            if (window.location.href.indexOf("?TenantID=") > -1) {
                $('.btnClearURLFilters').show();
            }
            else
                $('.btnClearURLFilters').hide();

            $('.btnClearURLFilters').click(function () {
                location.href = "WeeklyBilling.aspx";
            });





        });

        function MakeGridCall() {
            $('.btnGo').click(function () {
                $('.divLoading').fadeIn(0);
                setTimeout(function () { MakeGrid(); }, 500);
            });

            
        }
    </script>
    <script>


        document.addEventListener('DOMContentLoaded', MakeGrid)
        document.addEventListener('DOMContentLoaded', MakeGridCall)

        //RoutesAutoComplete();
       
        var colDefs = '';
        function MakeGrid() {
           // 
           

            $('#myGridWeek1, #myGridWeek2, #myGridWeek3, #myGridWeek4, #myGridTotals, #myGridTotalsCons').text('');

            var Date1 = $('.CalDateFrom').val();
            if (Date1 == "") {
                var now = new Date();
                var day = ("0" + now.getDate()).slice(-2);
                //var month = ("0" + (now.getMonth())).slice(-2);
                var month = ("0" + (now.getMonth() + 1)).slice(-2);
                Date1 = 21 + "-" + month + "-" + now.getFullYear();
                $('.CalDateFrom').datepicker('setDate', Date1);
                $('.CalDateFrom').val(Date1);
            }
            $('#<%=hdnCalDateFrom.ClientID%>').val(Date1);

            var Date2 = $('.CalDateTo').val();
            if (Date2 == "") {
                var now = new Date();
                var day = ("0" + now.getDate()).slice(-2);
                var month = ("0" + (now.getMonth() + 1)).slice(-2);
                Date2 = 20 + "-" + month + "-" + now.getFullYear();
                $('.CalDateTo').datepicker('setDate', Date2);
                $('.CalDateTo').val(Date2);
            }
            $('#<%=hdnCalDateTo.ClientID%>').val(Date2);
           
            var _sTenantID = $('.ddlTenant').val();
            $('#<%=hdnTenantID.ClientID%>').val(_sTenantID);

            if (window.location.href.indexOf("?TenantID") > -1) {
                var Items = window.location.href.split('?')[1].split('&');
                _sTenantID = Items[0].split('=')[1];                
                Date1 = Items[1].split('=')[1];
                Date2 = Items[2].split('=')[1];
                $('#<%=hdnTenantID.ClientID%>').val(_sTenantID);
                $('#<%=hdnCalDateFrom.ClientID%>').val(Date1);
                $('#<%=hdnCalDateTo.ClientID%>').val(Date2);

                $('.ddlTenant').val(_sTenantID);               
                $('.CalDateFrom').val(Date1);
                $('.CalDateTo').val(Date2);

                $('.CalDateFrom').datepicker("setDate", Date1);
                $(".CalDateTo").datepicker("setDate", Date2);

            }




            var Data = GetData(_sTenantID, Date1, Date2);
            
debugger
            var tablesCount = 0;
            var colWeek1 = "", colWeek2 = "", colWeek3 = "", colWeek4 = "", colTotalsCons = "";
            var dataWeek1 = "", dataWeek2 = "", dataWeek3 = "", dataWeek4 = "", dataTotalsCons="";
            $.each(JSON.parse(Data), function (idx, obj) {               
                tablesCount = tablesCount + 1;
            });
            tablesCount = tablesCount / 2;
            //alert(tablesCount);
 if (tablesCount == 6) {
                colWeek1 = JSON.parse(Data).Table6[0]["header"];
                dataWeek1 = JSON.parse(Data).Table;
                colWeek2 = JSON.parse(Data).Table7[0]["header"];
                dataWeek2 = JSON.parse(Data).Table1;
                colWeek3 = JSON.parse(Data).Table8[0]["header"];
                dataWeek3 = JSON.parse(Data).Table2;
                colWeek4 = JSON.parse(Data).Table9[0]["header"];
                dataWeek4 = JSON.parse(Data).Table3;
                colWeek5 = JSON.parse(Data).Table10[0]["header"];
                dataWeek5 = JSON.parse(Data).Table4;
                colTotalsCons = JSON.parse(Data).Table11[0]["header"];
                dataTotalsCons = JSON.parse(Data).Table5;
            }
            if (tablesCount == 5)
            {
                colWeek1 = JSON.parse(Data).Table5[0]["header"];
                dataWeek1 = JSON.parse(Data).Table;
                colWeek2 = JSON.parse(Data).Table6[0]["header"];
                dataWeek2 = JSON.parse(Data).Table1;
                colWeek3 = JSON.parse(Data).Table7[0]["header"];
                dataWeek3 = JSON.parse(Data).Table2;
                colWeek4 = JSON.parse(Data).Table8[0]["header"];
                dataWeek4 = JSON.parse(Data).Table3;
                colTotalsCons = JSON.parse(Data).Table9[0]["header"];
                dataTotalsCons = JSON.parse(Data).Table4;
            }
           

            if (tablesCount == 4) {
                colWeek1 = JSON.parse(Data).Table4[0]["header"];
                dataWeek1 = JSON.parse(Data).Table;
                colWeek2 = JSON.parse(Data).Table5[0]["header"];
                dataWeek2 = JSON.parse(Data).Table1;
                colWeek3 = JSON.parse(Data).Table6[0]["header"];
                dataWeek3 = JSON.parse(Data).Table2;
                //colWeek4 = JSON.parse(Data).Table8[0]["header"];
                //dataWeek4 = JSON.parse(Data).Table3;
                colTotalsCons = JSON.parse(Data).Table7[0]["header"];
                dataTotalsCons = JSON.parse(Data).Table3;
            }

            if (tablesCount == 3) {
                colWeek1 = JSON.parse(Data).Table3[0]["header"];
                dataWeek1 = JSON.parse(Data).Table;
                colWeek2 = JSON.parse(Data).Table4[0]["header"];
                dataWeek2 = JSON.parse(Data).Table1;
                //colWeek3 = JSON.parse(Data).Table6[0]["header"];
                //dataWeek3 = JSON.parse(Data).Table2;
                //colWeek4 = JSON.parse(Data).Table8[0]["header"];
                //dataWeek4 = JSON.parse(Data).Table3;
                colTotalsCons = JSON.parse(Data).Table5[0]["header"];
                dataTotalsCons = JSON.parse(Data).Table2;
            }

            if (tablesCount == 2) {
                colWeek1 = JSON.parse(Data).Table2[0]["header"];
                dataWeek1 = JSON.parse(Data).Table;
                //colWeek2 = JSON.parse(Data).Table3[0]["header"];
                //dataWeek2 = JSON.parse(Data).Table1;
                //colWeek3 = JSON.parse(Data).Table6[0]["header"];
                //dataWeek3 = JSON.parse(Data).Table2;
                //colWeek4 = JSON.parse(Data).Table8[0]["header"];
                //dataWeek4 = JSON.parse(Data).Table3;
                colTotalsCons = JSON.parse(Data).Table3[0]["header"];
                dataTotalsCons = JSON.parse(Data).Table1;
            }

/*----------------------------------Week1-------------------------------------------*/
            var gridOptionsWeek1 = {
                columnDefs: eval(colWeek1),// eval(JSON.parse(Data).Table6[0]["header"]),
                rowData: null,
                suppressMovableColumns: true,
                enableColResize: true
            };         

            var gridDivWeek1 = document.querySelector('#myGridWeek1');
            new agGrid.Grid(gridDivWeek1, gridOptionsWeek1);            
            gridOptionsWeek1.api.setRowData(dataWeek1); //JSON.parse(Data).Table
/*----------------------------------Week2-------------------------------------------*/
            var gridOptionsWeek2 = {
                columnDefs: eval(colWeek2),// eval(JSON.parse(Data).Table7[0]["header"]),
                rowData: null,
                suppressMovableColumns: true,
                enableColResize: true
            };

            var gridDivWeek2 = document.querySelector('#myGridWeek2');
            new agGrid.Grid(gridDivWeek2, gridOptionsWeek2);
            gridOptionsWeek2.api.setRowData(dataWeek2); //JSON.parse(Data).Table1
            /*----------------------------------Week3-------------------------------------------*/
            var gridOptionsWeek3 = {
                columnDefs:eval(colWeek3),// eval(JSON.parse(Data).Table8[0]["header"]),
                rowData: null,
                suppressMovableColumns: true,
                enableColResize: true
            };

            var gridDivWeek3 = document.querySelector('#myGridWeek3');
            new agGrid.Grid(gridDivWeek3, gridOptionsWeek3);
            gridOptionsWeek3.api.setRowData(dataWeek3);
            /*----------------------------------Week4-------------------------------------------*/
            var gridOptionsWeek4 = {
                columnDefs: eval(colWeek4),// eval(JSON.parse(Data).Table9[0]["header"]),
                rowData: null,
                suppressMovableColumns: true,
                enableColResize: true
            };

            var gridDivWeek4 = document.querySelector('#myGridWeek4');
            new agGrid.Grid(gridDivWeek4, gridOptionsWeek4);
            gridOptionsWeek4.api.setRowData(dataWeek4);
           

            /*----------------------------------Week5-------------------------------------------*/
            var gridOptionsWeek5 = {
                columnDefs: eval(colWeek5),// eval(JSON.parse(Data).Table9[0]["header"]),
                rowData: null,
                suppressMovableColumns: true,
                enableColResize: true
            };

            var gridDivWeek5 = document.querySelector('#myGridWeek5');
            new agGrid.Grid(gridDivWeek5, gridOptionsWeek5);
            gridOptionsWeek5.api.setRowData(dataWeek5);

            /*----------------------------------Totals Consolidated-------------------------------------------*/
            var gridOptionsTotalsCons = {
                columnDefs:eval(colTotalsCons),// eval(JSON.parse(Data).Table10[0]["header"]),
                rowData: null,
                suppressMovableColumns: true,
                enableColResize: true
            };

            var gridDivTotalsCons = document.querySelector('#myGridTotalsCons');
            new agGrid.Grid(gridDivTotalsCons, gridOptionsTotalsCons);
            gridOptionsTotalsCons.api.setRowData(dataTotalsCons);
        }

        function GetData(TenantID, DateFrom, DateTo) {
            //
            var Data;
            $.ajax({
                url: "WeeklyBilling.aspx/PrepareGrids",
               // dataType: "text",
                // data: JSON.stringify({ _sTenantID: TenantID, _sDateFrom:DateFrom, _sDateTo:DateTo}),
                data: "{ _sTenantID: '" + TenantID + "', _sDateFrom: '" + DateFrom + "', _sDateTo: '" + DateTo + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
               // method: 'GET',
                async: false,
                success: function (result) {
                    debugger
                    var filename = result.d;//JSON.parse(result);
                    Data = filename;//filename.d;

                    //alert('succes');

                },
                error: function (xhr, status, error) {
                    //
                    alert('error');
                },
                complete: function () {
                    $('.divLoading').fadeOut(1000);
                }
            });
            return Data;
        }
        function ModifyCellData(params) {
            // alert(params.value)
            if (params.value == null) {
                return "<span style='margin-right:47%;'>-</span>";
            }
            else {
                return params.value;
            }
        }
    </script>
      <script src="Scripts/jquery_1.11.1.min.js"></script>
    <div class="container">

    <table style="border: 0px solid red;">

        <tr>
            <td>&emsp;</td>
            <td>
                <div style="float:left;display:none;">
                        
                                    <table style="width:100%;text-align:left;font-size:10.5pt;float:right;">
                                        <tr>
                                            <td valign="top">Customer Name </td>
                                            <td>: <asp:Literal ID="ltCustomerName" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td valign="top">Address </td>
                                            <td>: <asp:Literal ID="ltAddress" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td valign="top">Date </td>
                                            <td>: <asp:Literal ID="Literal1" runat="server" Text='<%#DateTime.Now.ToString("dd-MM-yyy hh:mm tt") %>'></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td valign="top">Invoice No. </td>
                                            <td>: <asp:Literal ID="ltInvoiceNo" runat="server"></asp:Literal></td>
                                        </tr>
                                    </table>                                
                    </div>
                <div class="divReportContainer">
                    
                    <div class="divControls" style="display: inline-block; float: right;">
                        <table>
                            <tr>
                                <td style="width: 50px; text-align: left; padding-top: 20px;">
                                    <span class="fa fa-chain-broken btnClearURLFilters" aria-hidden="true" Title="Click this to clear URL filters to work with page filters"></span>
                                </td>
                                <td>
                                    <span class="spanHeads">Tenant</span>
                                    <select id="ddlTenant" class="ddlTenant form-control" runat="server" style="display: inline; width: 200px; height: 33px;">
                                        
                                    </select>&emsp;
                                    <asp:HiddenField ID="hdnTenantID" runat="server" Value="0"/>
                                </td>
                                <td>
                                    <span class="spanHeads">Start Date</span>
                                    <input type="text" id="CalDateFrom" runat="server" class="CalDateFrom form-control" style="display: inline; width: 100px; height: 33px;" />&emsp;
                                    <asp:HiddenField ID="hdnCalDateFrom" runat="server" />
                                </td>                                
                                <td>
                                    <span class="spanHeads">End Date</span>
                                    <input type="text" id="CalDateTo" runat="server" class="CalDateTo form-control" style="display: inline; width: 100px; height: 33px;" />&emsp;
                                    <asp:HiddenField ID="hdnCalDateTo" runat="server" />
                                </td>
                                <td>
                                    <input type="button" id="btnGo" class="btnGo ui-btn ui-button-small" value="Go" style="height: 30px; margin-top: 18px;" />&emsp;
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnExportExcel" runat="server" CssClass="ExportExcelIcon" Text="<i class='fa fa-file-excel-o' aria-hidden='true'></i>" ToolTip="Export Excel" OnClick="btnExportExcel_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="btnExportPdf" Visible="true" runat="server" CssClass="ExportExcelIcon" style="color:red;" Text="<i class='fa fa-file-pdf-o' aria-hidden='true'></i>" ToolTip="Export Pdf" OnClick="btnExportPdf_Click" ></asp:LinkButton> 
                                </td>
                            </tr>                           
                        </table>
                    </div>
                    
                </div>
            </td>
            <td>&emsp;</td>
        </tr>
        <tr>
            <td>&emsp;</td>
            <td >
                <div id="tdReports" >
                <div id="myGridWeek1" ag-grid="gridOptionsWeek1" class="ag-blue myGridWeek1 divReportagGrid" style="text-align: left;width:1000px;height:200px;"></div><br />
               <div id="myGridWeek2" ag-grid="gridOptionsWeek2" class="ag-blue myGridWeek2 divReportagGrid" style="text-align: left;width:1000px;height:200px;"></div><br />
               <div id="myGridWeek3" ag-grid="gridOptionsWeek3" class="ag-blue myGridWeek3 divReportagGrid" style="text-align: left;width:1000px;height:200px;"></div><br />
               <div id="myGridWeek4" ag-grid="gridOptionsWeek4" class="ag-blue myGridWeek4 divReportagGrid" style="text-align: left;width:1000px;height:300px;"></div><br />  
               <div id="myGridWeek5" ag-grid="gridOptionsWeek5" class="ag-blue myGridWeek5 divReportagGrid" style="text-align: left;width:1000px;height:200px;"></div><br />    
               <div id="myGridTotalsCons" ag-grid="gridOptionsTotalsCons" class="ag-blue myGridTotalsCons divReportagGrid" style="width:402px;height:300px;display:inline-block;"></div>
            <div style="display:inline-block; width:400px;float:right;margin-top:125px;">
                <asp:LinkButton ID="lnkSend" CssClass="ui-btn ui-button-small" OnClick="lnkSend_Click" runat="server">Send <%=MRLWMSC21Common.CommonLogic.btnfaRightArrow %></asp:LinkButton>
                </div>
                </div>
                <div id="editor"></div>
                    </td>
            <td>&emsp;</td>
        </tr>
    </table>
        </div>
     <div class="divLoading"></div>
</asp:Content>
