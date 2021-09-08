<%@ Page Title=" Search Outbound :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OutboundSearch.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <asp:ScriptManager runat="server" ID="spmngrOutboundSearch" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>

    <script type="text/javascript">
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }

        function ClearText(TextBox) {
            if (TextBox.value == "Search...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search...";

            TextBox.style.color = "#A4A4A4";
        }
         function check_uncheck(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement
                if (this != null) {

                    if (ValId.indexOf('CheckAll') != -1) {

                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes
                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsPrint') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkIsPrint') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }
                } // if
            } // for
        } // function
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadAutocompletes();
            }
        }
        function fnLoadAutocompletes() {
            $("#<%= this.txtFromDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%=this.txtToDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });
            $("#<%= this.txtToDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date()
            });
            $("#<%= this.txtDueDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                //maxDate: new Date()
            });
        }
        fnLoadAutocompletes();
        $(document).ready(function () {
          <%--   $('#<%= this.txtFromDate.ClientID%>').attr('readonly','readonly'); --%>
            $("#<%= this.txtFromDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%=this.txtToDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                    this.focus();

                    $('.hasDatepicker').datepicker({ minDate: -1, maxDate: -2 }).attr('readonly', 'readonly');
                },
                onClose: function () { this.focus(); $('.hasDatepicker').datepicker({ minDate: -1, maxDate: -2 }).attr('readonly', 'readonly'); }

            });
            $("#<%= this.txtToDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date()
            });

            $("#<%= this.txtDueDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy",
                //maxDate: new Date()
            });


            $("#<%= this.atcDepartment.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDepartment") %>',
                          data: "{ 'prefix': '" + request.term + "','TenentID': '" + '<%=  ViewState["TenantID"] %>' + "'}",
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {

                              response($.map(data.d, function (item) {
                                  return {
                                      label: item.split(',')[0],
                                      val: item.split(',')[1]
                                  }
                              }))
                          },
                          error: function (response) {

                          },
                          failure: function (response) {

                          }
                      });
                  },
                  select: function (e, i) {

                      $("#<%=hifDepartmentID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            $("#<%= this.atcDivision.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDivision") %>',
                          data: "{ 'prefix': '" + request.term + "' ,'TenantID': '" + '<%=  ViewState["TenantID"] %>' + "' ,'DeptID': '" + document.getElementById('<%= hifDepartmentID.ClientID%>').value + "'  }",
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {

                              if (data.d == "" || data.d == ",") {
                                  alert("No Divisions are available");
                                  document.getElementById('<%=atcDepartment.ClientID %>').focus();
                                  return;
                              }
                              response($.map(data.d, function (item) {
                                  return {
                                      label: item.split(',')[0],
                                      val: item.split(',')[1]
                                  }

                              }))
                          },
                          error: function (response) {

                          },
                          failure: function (response) {

                          }

                      });
                  },
                  select: function (e, i) {

                      $("#<%=hifDivisionID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            $("#<%= this.txtTenant.ClientID %>").focusout(function () {
                if ($("#<%= this.txtTenant.ClientID %>").val() == '')
                      $("#<%= this.hifTenant.ClientID %>").val('0');
            })
            var TextFieldName = $("#<%= this.txtTenant.ClientID %>");
            DropdownFunction(TextFieldName);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({

                source: function (request, response) {
                   // $("#<%= this.ddlStoreID.ClientID %>").val(0);
                    //  $("#<%= this.ddlStore.ClientID %>").val('');
                      $.ajax({
                          url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH") %>',
                          data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>

                        
                      
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {

                              response($.map(data.d, function (item) {
                                  return {
                                      label: item.split(',')[0],
                                      val: item.split(',')[1]
                                  }
                              }))
                          }
                      });
                  },
                  select: function (e, i) {

                      $("#<%=hifTenant.ClientID %>").val(i.item.val);
                   <%--  $("#<%=ddlStoreID.ClientID %>").val(i.item.val);--%>

                },
                minLength: 0
            });

            $("#<%= this.ddlStore.ClientID %>").val('');
            var textfieldname = $("#<%= this.ddlStore.ClientID %>");
            debugger;

            DropdownFunction(textfieldname);
            $("#<%=this.ddlStore.ClientID %>").autocomplete({
                source: function (request, response) {
                    //if (Tenantid == 0) {
                    //    showStickyToast(false, "Select Tenant");
                    //}
                    $.ajax({
                       // url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                       //   data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#<%=hifTenant.ClientID %>").val() + "'  }",
                          url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                          data: "{ 'prefix': '" + request.term + "'  }",
                        
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {
                              response($.map(data.d, function (item) {
                                  return {
                                      label: item.split(',')[0],
                                      val: item.split(',')[1]
                                  }
                              }))
                          }
                      });
                  },
                  select: function (e, i) {
                      debugger;
                      $("#<%=ddlStoreID.ClientID %>").val(i.item.val);
                     
                      $("#<%= this.txtTenant.ClientID %>").val('');

                },
                minLength: 0
            });
        });


        $(function () {
            $('.isvisibleNow').on('click', function () {
                $('.ishideNow').slideToggle();
            });
        });
    </script>
    <style>
        .ishideNow {
            display: none;
        }

        .pager {
            margin-top: 10px;
            margin-bottom: 10px;
        }

        /*#MainContent_OBContent_ddlDeliveryStatus {
            width: 100% !important;
        }*/

        #trt table {
            width: 100%;
        }

        .gvLightGrayNew_headerGrid th {
            white-space: nowrap;
        }

        .nonebrdr select {
            border-bottom: 1px solid #b3adad !important;
        }

        .nonebrdr .FormLabels {
            margin-right: 10px;
        }

        .flex-evenly .FormLabels {
            margin-bottom: 0px;
            width: 280px;
            margin-right: 10px;
        }

        .flex-evenly {
            justify-content: space-between;
        }

        #MainContent_IBContent_pnlSearchTextCat tr {
            display: flex;
            justify-content: space-between;
        }

        #MainContent_IBContent_pnlSearchTextCat td {
            margin-right: 5px;
            display: block;
            width: 350px;
        }


        .scroll {
            width: 81vw;
        }

        .scrollDexced {
            width: 91vw !important;
        }

        .fixayy input, select {
            width: 100% !important;
        }

        .flex______ {
            display: flex;
            justify-content: space-between;
            padding-right: 30px;
        }

        table {
            overflow: hidden;
        }

        .page-numbers {
            border: 0px !important;
        }

        .page-numbers {
            margin: 1px;
        }

        .flex__ #lnkPageNo.aspNetDisabled {
            text-decoration: none;
            padding: 3px;
            line-height: 21px;
        }

        input[type="text"], textarea {
            width: 97%;
        }

        .fixed_drop__ select {
            width: 80px !important;
        }

        .superAlie__ {
            width: 280px !important;
        }

            .superAlie__ select {
                width: 92% !important;
            }

        .hideNoww {
            display: none;
        }
    </style>


    <div class="container">
        <asp:UpdateProgress ID="uprgSearchOutbound" runat="server" AssociatedUpdatePanelID="upnlSearchOutbound">
            <ProgressTemplate>
                <div style="width: 100%; height: 100%; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: #e0ddd8ba;">

                    <div style="align-self: center;">
                        <div class="spinner">
                            <div class="bounce1"></div>
                            <div class="bounce2"></div>
                            <div class="bounce3"></div>
                        </div>

                    </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upnlSearchOutbound" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
            <%-- <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtnoutboundsearch" />
                            </Triggers>--%>
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" align="center" width="1250px" style="display: none;">

                    <tr>
                        <td>
                            <asp:Label ID="lblStatus" CssClass="ErrorMsg" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlDateStoreShipmentSearch" DefaultButton="lnkSearchtext" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" align="center">
                                    <tr>
                                        <td class="FormLabels" id="trt" valign="top">
                                            <table border="0" cellpadding="3" cellspacing="3">


                                                <tr class="flex-evenly ">
                                                    <td class="FormLabels hideNoww" valign="top"></td>
                                                    <td class="FormLabels hideNoww" valign="top"></td>

                                                    <!-- Globalization Tag is added for multilingual  -->

                                                    <%-- <td class="FormLabels" style="display: none;">Department--%>
                                                    <td class="FormLabels" style="display: none;"><%= GetGlobalResourceObject("Resource", "Department")%>
                                                        <asp:TextBox ID="atcDepartment" Width="200" CssClass="txt_slim" runat="server" />
                                                        <asp:HiddenField ID="hifDepartmentID" runat="server" />
                                                    </td>
                                                    <%-- <td class="FormLabels" style="display: none;">Division:--%>
                                                    <td class="FormLabels" style="display: none;"><%= GetGlobalResourceObject("Resource", "Division")%>
                                                        <br />
                                                        <asp:TextBox ID="atcDivision" Width="200" CssClass="txt_slim" runat="server" />
                                                        <asp:HiddenField ID="hifDivisionID" runat="server" />
                                                    </td>
                                                    <td class="FormLabels hideNoww superAlie__" valign="top"></td>
                                                    <td class="FormLabels superAlie__ hideNoww" valign="top"></td>
                                                    <td class="FormLabels superAlie__" valign="top" style="display: none;">
                                                        <div class="flex">
                                                            <input type="text" style="visibility: hidden" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="flex-evenly">
                                                    <td colspan="18" class="" style="display: block; width: -webkit-fill-available;" valign="top"></td>

                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="FormLabels" valign="top">
                                            <table border="0" id="MainContent_IBContent_pnlSearchTextCat">
                                                <tr>
                                                    <%--          <td class="FormLabels superAlie__" valign="top">
                                                                        <div class="flex">
                                                                            <div><asp:DropDownList ID="ddlCategory" CssClass="txt_slim" runat="server" required=""/></div>
                                                                        </div>
                                                                    </td>
                                                                    <td class="FormLabels superAlie__" valign="top">
                                                                        <div class="flex">
                                                                            <div></div>

                                                                            <div><asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                                                <label style="width: 60px;">Tenant</label>
                                                                            <asp:HiddenField runat="server" ID="hifTenant" Value="0"/></div>
                                                                        </div>
                                                                    </td>--%>

                                                    <%--      <td class="FormLabels" valign="top">
                                                                        <div class="flex__ alignrd">
                                                                        <div><asp:LinkButton ID="lnkSearchtext" CausesValidation="True" runat="server" OnClick="lnkSearchtext_Click" CssClass="btn btn-primary">
                                                                            Search <%= MRLWMSC21Common.CommonLogic.btnfaSearch %>
                                                                        </asp:LinkButton></div>

                                                                        <div>&nbsp;&nbsp;<asp:ImageButton ID="imgbtnoutboundsearch" runat="server" ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtnoutboundsearch_Click" ToolTip="Export To Excel" />
                                                                    </div></div></td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label CssClass="ErrorAlert" ID="ltError" runat="server" Text="" />

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Label ID="lblStatusMessage" runat="server" CssClass="ErrorMsg" />
                        </td>
                    </tr>


                    <tr>
                        <td>
                            <table>
                                <tr class="flex______">
                                    <td class="flex__ nonebrdr"></td>
                                    <td align="right">
                                        <div class="flex__">
                                            <div>
                                            </div>
                                        </div>
                                    </td>

                                </tr>


                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>

                    <tr>
                        <td align="right"></td>
                    </tr>


                </table>
                </div>


                                 <div class="row">
                                     <div class="col m3 s3">
                                         <div class="flex">
                                         <asp:TextBox ID="ddlStore" CssClass="txt_slim" runat="server" required="" />
                                         <asp:HiddenField ID="ddlStoreID" runat="server" Value="0" />
                                              <span class="errorMsg">*</span>  
                                        <label>Warehouse</label>
                                    </div>
                                </div>
                                     <div class="col m3 s3">
                                         <div class="flex">
                                             <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                             <%--<label style="width: 60px;">Tenant</label>--%>
                                             <label style="width: 60px;"><%= GetGlobalResourceObject("Resource", "Tenant")%></label>

                                             <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                         </div>
                                     </div>
                                     <div class="col m3 s3">
                                         <div class="flex">
                                             <div>
                                                 <asp:TextBox ID="txtFromDate" CssClass="txt_slim" runat="server" required="" /><%--<label>From Date</label>--%>
                                                 <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                                             </div>
                                         </div>
                                     </div>
                                     <div class="col m3 s3">
                                         <div class="flex">
                                             <asp:TextBox ID="txtToDate" CssClass="txt_slim" runat="server" required="" />
                                             <%--<label style="width: 60px;">To Date</label>--%>
                                             <label style="width: 60px;"><%= GetGlobalResourceObject("Resource", "ToDate")%></label>
                                         </div>
                                     </div>
                                     <div class="col m3 s3">
                                         <div class="flex">
                                             <asp:DropDownList ID="ddlDeliveryStatus" CssClass="txt_slim" runat="server" required="" /><%--<label>Delivery Status</label>--%>
                                             <label><%= GetGlobalResourceObject("Resource", "DeliveryStatus")%> </label>
                                         </div>
                                     </div>
                                     <%--   <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:DropDownList ID="ddlStore" CssClass="txt_slim" runat="server" required="" />
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:DropDownList ID="ddlDocumentType" CssClass="txt_slim" runat="server" required="" />
                                        </div>
                                    </div>--%>
                                 </div>

                <div class="row ishideNow">
                    <%-- <div class="col m3 s3">
                        <div class="flex">
                            <asp:DropDownList ID="ddlDeliveryStatus" CssClass="txt_slim" runat="server" required="" /><%--<label>Delivery Status</label>--%>
                    <%-- <label><%= GetGlobalResourceObject("Resource", "DeliveryStatus")%> </label>--%>
                    <%--    </div>
                    </div>--%>
                    <div class="col m3 s3">
                        <div class="flex">
                            <asp:DropDownList ID="ddlCategory" CssClass="txt_slim" runat="server" required="" />
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <asp:TextBox ID="txtsearchText" placeholder="Search" onfocus="ClearText(this)" SkinID="txt_Hidden_Req" onblur="javascript:focuslost1(this)" runat="server" />
                        </div>
                    </div>

                    <%--<div class="col m3 s3">
                        <div class="flex">
                            <asp:TextBox ID="ddlStore" CssClass="txt_slim" runat="server" required="" />
                            <asp:HiddenField ID="ddlStoreID" runat="server" Value="0" />
                            <label>Warehouse</label>
                        </div>
                    </div>--%>
                    <div class="col m3 s3">
                        <div class="flex">
                            <asp:DropDownList ID="ddlDocumentType" CssClass="txt_slim" runat="server" required="" />
                            <label>Outbound Type</label>
                        </div>
                    </div>

                    <div class="col m3 s3">
                        <div class="flex">
                            <asp:TextBox ID="txtAWBNo" CssClass="txt_slim" runat="server" required="" />
                            <label>AWB No.</label>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <asp:TextBox ID="txtDueDate" CssClass="txt_slim" runat="server" required="" />
                            <label>DueDate</label>
                        </div>
                    </div>
                    <%--<div class="col m3 s3" style="display:none">
                        <div class="flex">
                            <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                            <%--<label style="width: 60px;">Tenant</label>--%>
                    <%-- <label style="width: 60px;"><%= GetGlobalResourceObject("Resource", "Tenant")%></label>--%>

                    <%-- <asp:HiddenField runat="server" ID="hifTenant" Value="0" />--%>
                    <%-- </div>
                    </div>--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row">
            <div class="col m12">
                <div class="flex__ right">
                    <%--<button type="button" class="isvisibleNow btn btn-primary">Advanced Search<i class="material-icons vl">youtube_searched_for</i></button>--%>
                    <button type="button" class="isvisibleNow btn btn-primary"><%= GetGlobalResourceObject("Resource", "AdvancedSearch")%> <i class="material-icons vl">youtube_searched_for</i></button>
                    <div>
                        <%--<asp:LinkButton ID="lnkSearchtext" CausesValidation="True" runat="server" OnClick="lnkSearchtext_Click" CssClass="btn btn-primary">
                                                 Search <%= MRLWMSC21Common.CommonLogic.btnfaSearch %>
                                            </asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkSearchtext" CausesValidation="True" runat="server" OnClick="lnkSearchtext_Click" CssClass="btn btn-primary">
                                                  <%= GetGlobalResourceObject("Resource", "Search")%> <%= MRLWMSC21Common.CommonLogic.btnfaSearch %>
                        </asp:LinkButton>
                            <%--<asp:LinkButton ID="lnkTest" CssClass="btn btn-primary" runat="server" OnClick="lnkTest_Click">Test</asp:LinkButton>--%>
                      
                    </div>
                    <div>

                        <%-- <asp:LinkButton ID="imgbtnoutboundsearch" runat="server" CssClass="btn btn-primary" OnClick="imgbtnoutboundsearch_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                        <asp:LinkButton ID="imgbtnoutboundsearch" runat="server" CssClass="btn btn-primary" OnClick="imgbtnoutboundsearch_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%>  <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                          <asp:LinkButton ID="lnkGeneratePckingSlipPDF" ClientIDMode="Static" runat="server" CssClass="btn btn-primary" CausesValidation="false" OnClick="lnkGeneratePckingSlipPDF_Click">Generate Slip <%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" hidden>
            <div class="col m5">
                <div class="flex__">
                    <div class="FormLabels">
                        <asp:Label runat="server" ID="gvStatusMsg" CssClass="SubHeading3"></asp:Label>
                    </div>
                    &nbsp;
                                                        <div>
                                                            <asp:Literal runat="server" ID="lblstringperpage" Text="Display " />
                                                        </div>
                    &nbsp;
                          
                                                        <div class="fixed_drop__">
                                                            <asp:DropDownList runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="drppagesize_SelectedIndexChanged" ID="drppagesize">
                                                                <asp:ListItem Text="25" Value="25" Selected="True" />
                                                                <asp:ListItem Text="50" Value="50" />
                                                                <asp:ListItem Text="80" Value="80" />
                                                                <asp:ListItem Text="100" Value="100" />
                                                            </asp:DropDownList>
                                                        </div>
                </div>
            </div>
            <div class="col m7">
                <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ClientIDMode="Static" ID="dlPagerupper" OnItemCommand="dlPagerupper_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" class="page-numbers" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>


        <div class="row">
            <div class="col m12 p0">
                <div class="divdown">

                    <asp:GridView ShowFooter="false" ShowHeader="true" ShowHeaderWhenEmpty="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="deliverydata" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="false" PageSize="25" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnSorting="gvDeliveryResults_Sorting" OnPageIndexChanging="gvDeliveryResults_PageIndexChanging" OnRowDataBound="gvDeliveryResults_RowDataBound">
                        <Columns>

                            <asp:TemplateField ItemStyle-Width="" ItemStyle-CssClass="gvOBDNumber" ItemStyle-HorizontalAlign="Right" Visible="false">
                                <ItemTemplate>
                                    <asp:Literal ID="ltPriorityLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityLevel") %>' />
                                    <asp:Literal ID="ltPriorityTime" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityDateTime") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField ItemStyle-Width="" HeaderText="Delv. Doc. #" ItemStyle-CssClass="gvOBDNumber">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,DelvDoc%>" ItemStyle-CssClass="gvOBDNumber">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltOBDNumber" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") %>' />
                                    <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField ItemStyle-Width="" HeaderText="Delv. Doc. Type" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,DelvDocType%>" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <%--<asp:Literal runat="server" ID="ltDelvType" Text='<%# MRLWMSC21Common.CommonLogic.GetDocumentType(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString()) %>' />--%>
                                    <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType") %>' />
                                    <asp:Literal runat="server" ID="ltDocTypeID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>' />
                                    <asp:Literal runat="server" ID="ltIsReservationCheck" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsReservationDelivery") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,Account%>" Visible="false" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="ItAccName" ToolTip='<%# Bind("Account") %>' Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode") %>' />
                                    <%--  <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName").ToString() %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,Tenant%>">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField ItemStyle-Width="" HeaderText="Customer" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,Customer%>" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField ItemStyle-Width="" HeaderText="Warehouse(s)" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,Warehousess%>" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNames(DataBinder.Eval(Container.DataItem, "ReferedStoreIDs").ToString(),"<br/>") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>


                            <%-- <asp:TemplateField ItemStyle-Width="" HeaderText="Delv. Doc. Date" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,DelvDocDate%>" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltOBDDate" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd-MMM-yy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField ItemStyle-Width="" HeaderText="PNC Date" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,PNCDate%>" ItemStyle-CssClass="home" Visible="false">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltSentForPGI_DT" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNamesWithPNCStatus(DataBinder.Eval(Container.DataItem, "ReferedStoreIDs").ToString(),"<br>", DataBinder.Eval(Container.DataItem,"OBDNumber").ToString() , DataBinder.Eval(Container.DataItem,"OutboundID").ToString(),DataBinder.Eval(Container.DataItem,"TenantID").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- <asp:TemplateField ItemStyle-Width="" HeaderText="PGI Date / Done By" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,PGIDateDoneBy%>" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltPGIDate" Text='<%# String.Format("{0} <br> {1}",DataBinder.Eval(Container.DataItem, "PGIDoneOn","{0: dd-MMM-yy}").ToString(), MRLWMSC21Common.CommonLogic.GetUserName(DataBinder.Eval(Container.DataItem, "PGIDoneBy").ToString()) )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- <asp:TemplateField ItemStyle-Width="" HeaderText="Delivery Date" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,DeliveryDate%>" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltDeliveryDate" Text='<%# MRLWMSC21Common.CommonLogic.GetOBSearchStoreNamesWithDeliveryStatus(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryStatusID").ToString(), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsReservationDelivery")), DataBinder.Eval(Container.DataItem, "ReferedStoreIDs").ToString(),DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(), DataBinder.Eval(Container.DataItem,"OutboundID").ToString(),DataBinder.Eval(Container.DataItem,"TenantID").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- <asp:TemplateField ItemStyle-Width="" HeaderText="SO Number" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,SONumber%>" ItemStyle-CssClass="home">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltSONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="" HeaderText="Customer PO. No." ItemStyle-CssClass="home" Visible="false">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltCustPONumber1" Text='<%# DataBinder.Eval(Container.DataItem, "CustPONumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--   <asp:TemplateField ItemStyle-Width="" HeaderText="Group OBD No." ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,GroupOBDNo%>" ItemStyle-CssClass="home" Visible="false">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltCustPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "vlpdnumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- <asp:TemplateField ItemStyle-Width="" HeaderText="Invoice Number" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,InvoiceNumber%>" ItemStyle-CssClass="home" Visible="false">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>



                            <%-- <asp:TemplateField ItemStyle-Width="" HeaderText="Status" ItemStyle-CssClass="home">--%>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="<%$Resources:Resource,Status%>" ItemStyle-CssClass="home">
                                <ItemTemplate>

                                    <%--<asp:Literal runat="server" ID="ltDeliveryStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetDeliveryStatus((DataBinder.Eval(Container.DataItem, "DeliveryStatusID").ToString())) %>' />--%>
                                    <asp:Literal runat="server" ID="ltDeliveryStatus" Text='<%# DataBinder.Eval(Container.DataItem, "DeliveryStatus") %>' />
                                    <asp:Literal runat="server" ID="ltHidDeliveryStatusID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DeliveryStatusID") %>' />


                                </ItemTemplate>
                            </asp:TemplateField>



                             <asp:TemplateField ItemStyle-Width="" HeaderText="AWB No." ItemStyle-CssClass="home">
                                <ItemTemplate>                                   
                                    <asp:Literal runat="server" ID="ltAWBNo" Text='<%# DataBinder.Eval(Container.DataItem, "AWBNo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="" HeaderText="Courier" ItemStyle-CssClass="home">
                                <ItemTemplate>                                   
                                    <asp:Literal runat="server" ID="ltCourier" Text='<%# DataBinder.Eval(Container.DataItem, "Courier") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="" HeaderText="Due Date" ItemStyle-CssClass="home">
                                <ItemTemplate>                                   
                                    <asp:Literal runat="server" ID="ltDueDate" Text='<%# DataBinder.Eval(Container.DataItem, "DueDate","{0: dd-MMM-yy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField ItemStyle-CssClass="home" ItemStyle-Width="" ItemStyle-ForeColor="Red">
                                <HeaderTemplate>
                                    <%--  <nobr> &nbsp; Pick Note &nbsp; </nobr>--%>
                                    <nobr> &nbsp;Pick List &nbsp; </nobr>
                                </HeaderTemplate>
                                <ItemTemplate>

                                    <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." Text="P.Note" Font-Underline="false" ForeColor="Red">

                                    </asp:HyperLink>

                                    <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                    <img src='../Images/redarrowright.gif' border='0' />
                                    </a>    
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField ItemStyle-CssClass="home" ItemStyle-Width="" ItemStyle-ForeColor="Red">
                                <HeaderTemplate>
                                    <%--  <nobr> &nbsp; Pick Note &nbsp; </nobr>--%>
                                    <nobr> &nbsp;Del. Pick Note  &nbsp; </nobr>
                                </HeaderTemplate>
                                <ItemTemplate>

                                    <asp:HyperLink runat="server" ID="lnkHyperLinkDel" ToolTip="Del. Pick Note" Text="Del. Pick Note" Font-Underline="false" ForeColor="Red">

                                    </asp:HyperLink>

                                    <asp:Literal runat="server" ID="ltLineCount1" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                    <img src='../Images/redarrowright.gif' border='0' />
                                    </a>    
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pick Path" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="lnkPickPath" Text="Pick.Path" Font-Underline="false" ForeColor="Red" NavigateUrl='<%# String.Format("OptimalPickPathLibrary.aspx?obdid={0}&lineitemcount={1}",DataBinder.Eval(Container.DataItem, "OutboundID"),DataBinder.Eval(Container.DataItem, "LineCount").ToString())%>'></asp:HyperLink>
                                    <asp:Literal runat="server" ID="ltpickpathLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                    <img src='../Images/redarrowright.gif' border='0' />
                                </ItemTemplate>
                            </asp:TemplateField>


                            <%-- <asp:TemplateField ItemStyle-CssClass="home centered" HeaderText="Modify">--%>
                            <asp:TemplateField ItemStyle-CssClass="home centered" HeaderText="<%$Resources:Resource,Modify%>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditInbound" runat="server" CssClass="GvLink" Visible="false" PostBackUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}") %>' Text="<nobr> Change <img src='../Images/redarrowright.gif' border='0' /></nobr>" ToolTip="Change Outbound" />

                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" NavigateUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}") %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="" ItemStyle-CssClass="NoPrint" HeaderStyle-HorizontalAlign="Center"  HeaderText="Print Labels" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:Label ID="lblPrintLabel" Text="Select" runat="server" CssClass="NoPrint" /><br />
                                    <span class="checkbox hide-next-check">
                                        <asp:CheckBox ID="CheckAll" onclick="return check_uncheck(this);" runat="server" /><label for=""></label></span>
                                    <br>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="checkbox hide-next-check">
                                        <asp:CheckBox ID="chkIsPrint" runat="server" CssClass="NoPrint" /><label for=""></label></span>
                                </ItemTemplate>
                               <%-- <FooterTemplate>
                                    <div style="text-align: center;">
                                        <asp:LinkButton ID="lnkGeneratePckingSlipPDF" ClientIDMode="Static" runat="server" Text="<nobar><i class='material-icons vl'>print</i></nobar>" CssClass="ButEmpty" CausesValidation="false" OnClick="lnkGeneratePckingSlipPDF_Click" /></div>

                                </FooterTemplate>--%>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            <%--<div align="center">No Data Found</div>--%>
                            <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col m12">
                <asp:DataList CellPadding="10" ClientIDMode="Static" RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPagerupper_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CssClass="page-numbers" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hfpageId" />


    </div>


</asp:Content>
