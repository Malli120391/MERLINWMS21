<%@ Page Title="Tariff Allocation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TariffAllocation.aspx.cs" Inherits="FalconAdmin._3PLBilling.TariffAllocation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />

    <script type="text/javascript" src="../Scripts/CommonScripts.js"></script>

    <script>
        var BillTypeID;
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadBasic();
            }
        }
        fnLoadBasic();
        //function validRate()
        //{
        //    //alert($('#txtDiscount').val() > 0);
        //    if (!($('#rbOnRate').is(':checked') || $('#rbOnRateGroup').is(':checked')) && $('#txtDiscount').val()>0) {
        //        alert("Please select discount type");
        //        $('#rbOnRate').focus();
        //    }
        //    //if ($('#rbOnRate').is(':checked') || $('#rbOnRateGroup').is(':checked'))
        //    //{
                
        //    //    if (!($('#txtDiscount').val() > 0))
        //    //    {
        //    //        alert('Discount greater than zero');
        //    //        $('#txtDiscount').focus();
        //    //    }
        //    //}
        //}
        function Checkall(flag)
        {
            //var frm = document.getElementsByClassName('deleteRecord');
            $("[id=cbkhifTenantActivityRateID]").prop('checked', flag);
        }

        function Succes() {
            showStickyToast(true, "Successfully Updated", false);
        }

        function SuccesDel() {
            showStickyToast(true, "Successfully Deleted", false);
        }

        function fnLoadBasic()
        {
            $(document).ready(function ()
            {
                
                $('#cbkCheckall').click(function () {
                    if ($('#cbkCheckall').is(':checked'))
                    {
                        Checkall(true);
                    }else
                        Checkall(false);
                });

                //$('#txtDiscount').focusout(function () {
                   
                //        validRate();
                   
                //})


                //$('#rbOnRate').focusout(function () {
                //    validRate();
                //})
                
                var textfieldname = $("#txtTenantName");
                DropdownFunction(textfieldname);
                
                $("#txtTenantName").autocomplete({            
                    source: function (request, response) {
                        
                        $.ajax({
                        
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantandBilltype") %>',--%>
                           // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantList") %>',
                          //  data: "{ 'prefix': '" + request.term + "'}",
                            url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',  // added by Ganesh @Sep 30 2020 -- Tenant drop down data should be displyed by UserWH
                            data: "{ 'prefix': '" + request.term + "','whid':'"+$("#hifWareHouseId").val()+"' }",
                       

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

                        $("#hifTenantID").val(i.item.val.split('`')[0]);
                        $("#hidTenant").val(i.item.val.split('`')[0]);
                        BillTypeID = i.item.val.split('`')[1];
                        $('#txtEffectiveFrom').val('');
                        //alert(BillTypeID); hidTenant
                    },
                     minLength: 0
                });


                var textfieldname = $("#txtActivityRateGroup");
                DropdownFunction(textfieldname);
                
                $("#txtActivityRateGroup").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityRateGroupAdmin") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
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
                        $("#hifActivityRateGroupID").val(i.item.val);
                        $("#txtActivityRateType").val('');
                    },
                    minLength: 0
                });



                var textfieldname = $("#txtActivityRateType");
                DropdownFunction(textfieldname);

                $("#txtActivityRateType").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityRateTypeAdmin") %>',
                            data: "{ 'prefix': '" + request.term + "','ActivityRateGroupID':'" + $("#hifActivityRateGroupID").val() + "'}",
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
                        $("#hifActivityRateTypeID").val(i.item.val);
                        $("#txtActivityRateName").val('');
                    },
                    minLength: 0
                });


                var textfieldname = $("#txtWareHouse");
                DropdownFunction(textfieldname);

                $("#txtWareHouse").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                           // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWareHouse_TariffAllocation") %>',
                           // data: "{ 'prefix': '" + request.term + "','Tenantid':'" + $("#hifTenantID").val() + "'}",  // comment by Ganesh @Sep 30 2020
                            url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',    // added by Ganesh @Sep 30 2020 -- Wh drop down data should be displyed by User
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
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    select: function (e, i) {
                        $("#hifWareHouseId").val(i.item.val);
                        $("#txtActivityRateName").val('');
                    },
                    minLength: 0
                });



                var textfieldname = $("#txtActivityRateName");
                DropdownFunction(textfieldname);

                $("#txtActivityRateName").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityRateAdmin_Tarif") %>',
                            data: "{ 'prefix': '" + request.term + "','ActivityRateTypeID':'" + $("#hifActivityRateTypeID").val() + "', 'WareHouseId':'"+ $("#hifWareHouseId").val()+"'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == "/") {
                                    alert('No Activity Tariffs are configured to this Activity Tariff Type');
                                    document.getElementById("txtActivityRateName").value = "";
                                    return;
                                }
                                else {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item.split(',')[0],
                                            val: item.split(',')[1]
                                        }
                                    }))
                                }

                            }

                        });
                    },
                    select: function (e, i) {
                        $("#hifActivityRateID").val(i.item.val);
                    },
                    minLength: 0
                });
                var _minDate = new Date(1990, 1, 1, 0, 0, 0);
                $("#txtEffectiveFrom").datepicker({
                    dateFormat: "dd-M-yy",
                    onSelect: function (selected) {
                        $("#txtEffectiveTo").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" });
                       // setMaxdate(selected, BillTypeID);
                    }
                });

               
                
                $('#txtEffectiveTo').datepicker({
                    dateFormat: "dd-M-yy",
                    minDate: _minDate,
                    onSelect: function (selected) {
                        $("#txtEffectiveFrom").datepicker("option", "maxDate", selected, { dateFormat: "dd-M-yy" })
                    }
                });
            });
            
        }

        function setBilltype(BillType)
        {
            BillTypeID = BillType;
            //$('#txtEffectiveFrom').val('');
            //alert(BillTypeID);
        }
        function setMaxdate(fromdate, BillTypeID)
        {
            var datepart = fromdate.split('/');
            //var testdate=new Date(2015,12,03,0,0,0,0);
            //alert('new :'+testdate.setMonth);
            var todate = new Date(datepart[2], (datepart[1]-1), datepart[0]);
            
            switch (BillTypeID)
            {
                case '1': todate.setDate(todate.getDate() + 15);
                    break;
                case '2': todate.setDate(todate.getDate() + 30);
                    break;
                case '3':
                        //alert(todate + ' start' + BillTypeID);
                        todate.setMonth(todate.getMonth() + 3);
                        //alert(todate + ' end  ' + BillTypeID);
                    break;
                case '4': todate.setMonth(todate.getMonth() + 6);
                    break;
                case '5': todate.setYear(todate.getYear() + 1);
                    break;
                case '6': todate.setDate(todate.getDate() + 1);
                    break;
                case '7': todate.setDate(todate.getDate() + 7);
                    break;
                default: todate.setDate(todate.getDate() + 10);

            }
            //alert(todate + 'abc' + BillTypeID);
            //tomorrow.setDate(tomorrow.getDate() + dif);
            //console.log($("#lbEffectiveTo").text());
            //$("#txtEffectiveTo").datepicker("option", "maxDate", todate);
            $("#lbEffectiveTo").text(todate.format("dd/MM/yyyy"));
            $("#hifEffectiveTo").val(todate.format("dd/MM/yyyy"));
            //console.log($("#lbEffectiveTo").text());
            //$("#txtEffectiveTo").datepicker({
            //    //minDate: tomorrow,
            //    //maxDate: new Date(),
            //    maxDate: tomorrow
            //});
            //setMindate('2015/06/17', 10);
        }
        
    </script>



    <script type="text/javascript">

        function ClearText(TextBox) {
            if (TextBox.value == "Tenant...");
            TextBox.value = "";
        }

        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Tenant...";
        }

     </script>

    <script type="text/javascript">

            $(document).ready(function () {
                var textfieldname = $("#<%= this.txtTenant.ClientID %>");
               DropdownFunction(textfieldname);
               $("#<%= this.txtTenant.ClientID %>").autocomplete({
                   source: function (request, response) {
                       $.ajax({
                           //url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantList") %>',
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',   // added by Ganesh @Sep 30 and Tenant Drop down data should be displyed by UserWh
                         data: "{ 'prefix': '" + request.term + "' }",
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

                    $("#<%=hidTenant.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
            });

            });      
    </script>
    <link href="tpl.css" rel="stylesheet" />
    <style>
        .gvLightSeaBlue_headerGrid th {
    padding: 12px;
    font-size: 11.5px;
}

        .gvLightSeaBlue_DataCellGrid td {
            font-size:11.5px;
        }

        .gvLightSeaBlue_DataCellGridAlt td {font-size:14px;
        }
         .gvCell
        {
            text-align: center !important;
        }
         .gridInput .ui-autocomplete-input{
             width:100px !important;
         }

         .gridInput
         {
             display:flex;align-items:center;
         }

         .gridInput  input{
               width:100px !important;
                   padding: 5px 5px 5px 0px !important;
         }

         .inscrolling ~ div{
             width:100%;
             overflow:auto;
         }
    </style>
     <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <a href="#">3PL</a> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Tariff Allocation<asp:Literal ID="ltFormSubHeading" runat="server"/> </span></div>
                <%--<div class="mandatory"><b>Note:</b> <span style="color:red"> __ </span>Indicates mandatory fields</div>--%>
            </div>

        </div>
    <div class="container">

    <div>
     
        <div class="row">
            <div>
                <div class=" ">
                    <div class="">
                        <div class="col m3 offset-m7">
                            <div class="flex">
                                <asp:TextBox runat="server" ID="txtTenant" SkinID="txt_Hidden_Req_Auto" required="" />
                                <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            </div>
                        </div>
                        <div class="col m2">
                            <gap5></gap5>
                            <asp:LinkButton runat="server" ID="lnkSearchTenant" CssClass="btn btn-primary" OnClick="lnkSearchTenant_Click"> <%= GetGlobalResourceObject("Resource", "Search")%> <%= MRLWMSC21Common.CommonLogic.btnfaSearch %> </asp:LinkButton>
                            <asp:HiddenField runat="server" ID="hidTenant" Value="0" />
                       
                            <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" CssClass="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Add")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
         </div>
       
        
        <div class="row">
            <div class="minHeight" >
                <asp:Label ID="lblStatus" runat="server" />
                <div class="">
                    <div class="inscrolling"></div>
                <asp:GridView ID="gvTariffAllocation" runat="server" SkinID="gvLightSeaBlue" ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" AutoGenerateColumns="false" PageSize="25"
                    OnRowDataBound="gvTariffAllocation_RowDataBound"
                    OnRowEditing="gvTariffAllocation_RowEditing"
                    OnRowCancelingEdit="gvTariffAllocation_RowCancelingEdit"
                    OnRowUpdating="gvTariffAllocation_RowUpdating"
                    OnPageIndexChanging="gvTariffAllocation_PageIndexChanging" OnPreRender="gvTariffAllocation_PreRender">
                    <Columns>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,   WareHouse%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltWareHouse" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"WHCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                  <div class="gridInput">
                               <%-- <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" ControlToValidate="txtActivityRateType" ValidationGroup="vgTariff" />--%>
                                <asp:TextBox ID="txtWareHouse" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"WHCode") %>'/>
                                <asp:HiddenField ID="hifWareHouseId" ClientIDMode="Static" runat="server" />
                             <span class="errorMsg"></span>
                            </div>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="Tenant" ItemStyle-Width="120">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,Tenant%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltTenantName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="gridInput">
<%--                                <asp:RequiredFieldValidator ID="rfvTenantName" runat="server" ControlToValidate="txtTenantName" ValidationGroup="vgTariff"  />--%>
                                <asp:TextBox ID="txtTenantName" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>'/>
                                <asp:HiddenField ID="hifTenantActivityRateID" runat="server" />
                                <asp:HiddenField ID="hifTenantID" ClientIDMode="Static" runat="server" />
                                    <span class="errorMsg"></span>
                            </div>
                                    </EditItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="Tariff Group" ItemStyle-Width="80">--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,TariffGroup%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltActivityRateGroup" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateGroup") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <div class="gridInput">
<%--                                <asp:RequiredFieldValidator ID="rfvActivityRateGroup" runat="server" ControlToValidate="txtActivityRateGroup" ValidationGroup="vgTariff" />--%>
                                <asp:TextBox ID="txtActivityRateGroup" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateGroup") %>'/>
                                <asp:HiddenField ID="hifActivityRateGroupID" ClientIDMode="Static" runat="server" />
                             <span class="errorMsg"></span>
                            </div>
                                     </EditItemTemplate>
                        </asp:TemplateField>

                     <%--   <asp:TemplateField HeaderText="Tariff Sub-Group" ItemStyle-Width="90">--%>
                           <asp:TemplateField HeaderText="<%$Resources:Resource,TariffSubGroup%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                  <div class="gridInput">
                               <%-- <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" ControlToValidate="txtActivityRateType" ValidationGroup="vgTariff" />--%>
                                <asp:TextBox ID="txtActivityRateType" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>'/>
                                <asp:HiddenField ID="hifActivityRateTypeID" ClientIDMode="Static" runat="server" />
                             <span class="errorMsg"></span>
                            </div>
                            </EditItemTemplate>
                        </asp:TemplateField>

                      


                       <%-- <asp:TemplateField HeaderText="Tariff" ItemStyle-Width="150">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,Tariff%>" >
                            <ItemTemplate>
                                <asp:Literal ID="ltActivityRateName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                  <div class="gridInput">
                              <%--  <asp:RequiredFieldValidator ID="rfvActivityRateName" runat="server" ControlToValidate="txtActivityRateName" ValidationGroup="vgTariff"  />--%>
                                <div class="flex"> <asp:TextBox ID="txtActivityRateName" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>'/>
                               <span class="errorMsg"></span></div>
                                      <asp:HiddenField ID="hifActivityRateID" ClientIDMode="Static" runat="server" />
                                     
                                <div class="checkbox" ><asp:CheckBox ID="cbxIsOntimeRate"  runat="server" /><label for="" style="padding: 0 20px;">Is One Time Rate</label></div>
                             
                            </div>
                                </EditItemTemplate>
                        </asp:TemplateField>
                   <%--     <asp:TemplateField HeaderText="Unit Cost" ItemStyle-Width="50">--%>
                             <asp:TemplateField HeaderText="<%$Resources:Resource,UnitCost%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>

                            </EditItemTemplate>
                        </asp:TemplateField>
                       <%--<asp:TemplateField HeaderText="Discount On " ItemStyle-Width="30" Visible="false" >
                            <ItemTemplate>
                                <asp:Literal ID="ltdiscoynt" runat="server" Text='<%#setDiscountOn(DataBinder.Eval(Container.DataItem,"OnRate").ToString(),DataBinder.Eval(Container.DataItem,"OnRateGroup").ToString()) %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RadioButton ID="rbOnRate" Checked="true" ClientIDMode="Static" runat="server" Text="On Rate" GroupName="rbRate" /><br />
                                
                                <asp:RadioButton ID="rbOnRateGroup" ClientIDMode="Static" runat="server" Text="On Rate Group" GroupName="rbRate" />
                                <asp:HiddenField ID="hifTenantDiscountRateGroupID" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>--%>
                       <%-- <asp:TemplateField HeaderText="Discount (%)" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,Discounts%>" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="">
                            <ItemTemplate >
                                <%--<asp:Literal ID="ltDiscount" runat="server" Text='<%#setDiscount(DataBinder.Eval(Container.DataItem,"OnRate").ToString(),DataBinder.Eval(Container.DataItem,"OnRateGroup").ToString()) %>' />--%>
                                <asp:Literal ID="ltDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OnRate") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <div class="gridInput">
                                <asp:TextBox ID="txtDiscount" ClientIDMode="Static" runat="server" onKeyPress="return checkDec(this,event)"/>
                                <asp:HiddenField ID="hifTenantDiscountRateID" runat="server" />
                            </div>
                                     </EditItemTemplate>
                        </asp:TemplateField>
                        
                        
                      <%--  <asp:TemplateField HeaderText="Effective From">--%>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,EffectiveFrom%>">
                            <ItemTemplate>
                                <asp:Literal ID="ltEffectiveFrom" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:dd/MM/yyyy}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <div class="gridInput">
<%--                                <asp:RequiredFieldValidator ID="rfvEffectiveFrom" ControlToValidate="txtEffectiveFrom"  ErrorMessage="*" ValidationGroup="vgTariff" runat="server"/>--%>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server" ClientIDMode="Static"  Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:dd/MM/yyyy}") %>' />
                            <span class="errorMsg"></span>
                            </div>
                                     </EditItemTemplate>
                        </asp:TemplateField>
                        
                      <%--  <asp:TemplateField HeaderText="Effective To">--%>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,EffectiveTo%>"
>
                            <ItemTemplate>
                                <asp:Literal ID="ltEffectiveTo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd/MM/yyyy}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
 <div class="gridInput">
<%--                                <asp:RequiredFieldValidator ID="rfvEffectiveTo" Enabled="false"  ControlToValidate="txtEffectiveTo"  ErrorMessage="*" ValidationGroup="vgTariff" runat="server"/>--%>
                                <asp:TextBox ID="txtEffectiveTo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd/MM/yyyy}") %>'  ClientIDMode="Static" /><!---ClientIDMode="Static"  -->
                               <%-- <asp:Label ID="lbEffectiveTo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd/MM/yyyy}") %>'  ClientIDMode="Static" />--%>
                                <asp:HiddenField ID="hifEffectiveTo" runat="server" ClientIDMode="Static" />
                          <span class="errorMsg"></span>
                            </div>
                                </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                
                                <div class="checkbox"><asp:CheckBox ID="cbkCheckall" runat="server" ClientIDMode="Static"  /><%--<label>Delete</label>--%>
                                    <label> <%= GetGlobalResourceObject("Resource", "Delete")%>
</label>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <label class="check-box"><asp:CheckBox ID="cbkhifTenantActivityRateID" runat="server" ClientIDMode="Static" /></label>
                                <asp:HiddenField ID="hifItemTenantActivityRateID" Value='<%#DataBinder.Eval(Container.DataItem,"TenantActivityRateID") %>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return window.confirm('Are you sure want to delete records');" Font-Underline="false"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                            </FooterTemplate>
                            <EditItemTemplate></EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:CommandField  ControlStyle-CssClass="ButEmpty"  ButtonType="Link" ItemStyle-CssClass="ButEmpty"  ItemStyle-HorizontalAlign="Right" EditText="<i class='material-icons ss'>mode_edit</i>" ShowEditButton="True" ItemStyle-Width="20" />
                    </Columns>
                    <EmptyDataTemplate>
						<%--<div align="center">No Data Found</div>--%>
                        <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%>
</div>
					</EmptyDataTemplate>
                </asp:GridView>
                    </div>
            </div>
        </div>
    </div>
        </div>
    <asp:HiddenField ID="hifDiscountChangePrevious" runat="server" />
    <asp:HiddenField ID="hifDiscountChangeAfter" runat="server" />
</asp:Content>

