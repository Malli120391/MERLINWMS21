<%@ Page Title="Vehicle Master List" Language="C#" MasterPageFile="~/mYardManagement/YardManagement.master" AutoEventWireup="true" CodeBehind="VehicleMasterList.aspx.cs" Inherits="MRLWMSC21.mYardManagement.VehicleMasterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script src="../Scripts/jquery-ui-1.8.24.js"></script>
    <%--<script src="../Scripts/CommonScripts.js"></script>--%>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../Content/app.css" rel="stylesheet" />
    <%--<script src="../Scripts/CommonWMS.js"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            if (<%=this.cp.AccountID%> == 0 || <%=this.cp.AccountID%> == null) {
              <%--  $('#<%= this.txtAccount.ClientID %>').attr("disabled", false);--%>
            }
            else {
                //debugger;
              <%--  $('#<%= this.txtAccount.ClientID %>').attr("disabled", true);
                $("#<%= this.txtAccount.ClientID %>").css("background-color", "#ebebe4");
                $("#<%= this.txtAccount.ClientID %>").val("<%=this.cp.Account%>");
                $("#<%= this.Account.ClientID %>").val(<%=this.cp.AccountID%>);--%>
            }
        });
        $(function () {
            //Load Account Dropdown
            var accountfield = $('#<%= this.txtAccount.ClientID %>');
            DropdownFunction(accountfield);
            $("#<%= this.txtAccount.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountForCyccleCount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" +  <%=this.cp.AccountID%> + "'}",
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
                    //debugger
                    $("#<%= this.Account.ClientID %>").val(i.item.val);
                    $("#<%= this.txtAccount.ClientID %>").val(i.item.label);
                    var AccountID = $("#<%= this.Account.ClientID %>").val();
                    $("#<%= this.Freight.ClientID %>").val("");
                     $("#<%= this.VehicleType.ClientID %>").val("");
                    LoadFreightBasedAcc(AccountID);
                    LoadVehicleTypeBasedAcc(AccountID);
                },
                minLength: 0
            });            
            
        });

        //Load Freight Company Based on Account
        function LoadFreightBasedAcc(id) {
            var freightfield = $('#<%= this.txtFreight.ClientID %>');
            DropdownFunction(freightfield);
            $("#<%= this.txtFreight.ClientID %>").autocomplete({
                source: function (request, response) {
                    debugger;
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadFreightCompanyBasedonAccount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" + id + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger;
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
                    //debugger;
                    $("#<%= this.Freight.ClientID %>").val(i.item.val);
                    $("#<%= this.txtFreight.ClientID %>").val(i.item.label);
                     $("#<%= this.VehicleType.ClientID %>").val("");
                },
                minLength: 0
            });
        }

        //Load Vehicle Type Dropdown
        function LoadVehicleTypeBasedAcc(id) {
            var vehiclefield = $('#<%= this.txtVehicleType.ClientID %>');
            DropdownFunction(vehiclefield);
            $("#<%= this.txtVehicleType.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadVehicleTypeBasedonAccount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" +  id + "'}",
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
                    //debugger
                    $("#<%= this.VehicleType.ClientID %>").val(i.item.val);
                    $("#<%= this.txtVehicleType.ClientID %>").val(i.item.label);
                },
                minLength: 0
            });
        }

        function validation() {
            debugger;
            var account = $("#<%= this.Account.ClientID %>").val();
            var Acc = $("#<%= this.txtAccount.ClientID %>").val();
            var Freight = $("#<%= this.Freight.ClientID %>").val();
            var Freightname = $("#<%= this.txtFreight.ClientID %>").val();
            var VehicleType = $("#<%= this.VehicleType.ClientID %>").val();
            var Vehicle = $("#<%= this.txtVehicleType.ClientID %>").val();
            if (account == "" || account == 0 || account == null || account == undefined || Acc == "" || Acc == undefined || Acc == null) {
                showStickyToast(false, 'Please select Account ');
                return false;
            }
            else if (Freight == "" || Freight == 0 || Freight == null || Freight == undefined || Freightname == "" || Freightname == undefined || Freightname == null) {
                showStickyToast(false, "Please select Freight Company");
                return false;
            }
            else if (VehicleType == "" || VehicleType == 0 || VehicleType == null || VehicleType == undefined || Vehicle == "" || Vehicle == null || Vehicle == null) {
                showStickyToast(false, "Please Select Vehicle Type");
                return false;
            }

            else {
                return true;
            }
        }
    </script>


    <div class="container">
        <div class="">
            <div class="row m0">
                <div>
                    <div>
                       
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>
                                    <input type="text" id="txtAccount" runat="server" class="ui-autocomplete-input" required="">
                                   <%-- <label>Account</label>--%>
                                     <label> <%= GetGlobalResourceObject("Resource", "Account")%></label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="Account" runat="server" />
                                    <%--<select id="Account" required=""></select>--%>
                                </div>
                            </div>
                        </div>
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>
                                    <input type="text" id="txtFreight" runat="server" class="ui-autocomplete-input" required="">
                                    <label> <%= GetGlobalResourceObject("Resource", "FreightCompany")%> </label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="Freight" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>
                                    <input type="text" id="txtVehicleType" runat="server" class="ui-autocomplete-input" required="">
                                   <%-- <label>Vehicle Type</label>--%>
                                     <label><%= GetGlobalResourceObject("Resource", "VehicleType")%></label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="VehicleType" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col m3 s3">
                            <gap5></gap5>
                            <div flex btnBottom>
                                <div class="">
                               <%--     <asp:LinkButton ID="lnkVehicleListSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkVehicleListSearch_Click"><i class="material-icons vl">search</i>Search</asp:LinkButton>--%>
                                         <asp:LinkButton ID="lnkVehicleListSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkVehicleListSearch_Click"><%= GetGlobalResourceObject("Resource", "Search")%><i class="material-icons vl">search</i></asp:LinkButton>
                               
                                   <%-- <asp:LinkButton PostBackUrl="~/mYardManagement/VehicleMasterRequest.aspx" CssClass="btn btn-primary" ID="lnkAddVehicle" runat="server">Add Vehicle <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>--%>
                                     <asp:LinkButton PostBackUrl="~/mYardManagement/VehicleMasterRequest.aspx" CssClass="btn btn-primary" ID="lnkAddVehicle" runat="server"><%= GetGlobalResourceObject("Resource", "AddVehicle")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView Width="100%" ID="gvYardList" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" Font-Underline="false" PagerSettings-Position="TopAndBottom" OnPageIndexChanging="gvYardList_PageIndexChanging" AllowPaging="true" PageSize="10" AllowSorting="True" SkinID="gvLightSkyBlueNew" HorizontalAlign="Left">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="170" HeaderText="<%$Resources:Resource,SNo%>" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSNo" Text='<%# DataBinder.Eval(Container.DataItem, "SNo") %>' />
                                <asp:Literal runat="server" ID="ltYVehicleID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "YM_MST_Vehicle_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="170" HeaderText="<%$Resources:Resource,VehicleRegd%>"  HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSNo" Text='<%# DataBinder.Eval(Container.DataItem, "RegistrationNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="170" HeaderText="<%$Resources:Resource,VehicleType%>"  HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSNo" Text='<%# DataBinder.Eval(Container.DataItem, "VehicleType") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField ItemStyle-Width="170" HeaderText="<%$Resources:Resource,FreightCompany%>" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSNo" Text='<%# DataBinder.Eval(Container.DataItem, "FreightCompany") %>' />
                                <asp:Literal runat="server" ID="ltYVehicleID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "FreightCompanyID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="170" HeaderText="<%$Resources:Resource,OwnerName%>" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSNo" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="170" HeaderText="<%$Resources:Resource,OwnerContact%>"  HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltSNo" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerContact") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="55" ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:Resource,Edit%>" >
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink11" Text="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" NavigateUrl='<%#  String.Format("VehicleMasterRequest.aspx?vid={0}",DataBinder.Eval(Container.DataItem, "YM_MST_Vehicle_ID").ToString())  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                    </EmptyDataTemplate>
                    <PagerSettings FirstPageText="&amp;lt;&amp;lt;" LastPageText="&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
