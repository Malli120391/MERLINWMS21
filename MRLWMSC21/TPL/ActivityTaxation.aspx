<%@ Page Title="ActivityTaxation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActivityTaxation.aspx.cs" Inherits="FalconAdmin._3PLBilling.ActivityTaxation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadBasic();
            }
        }
        function Checkall(flag) {
            //var frm = document.getElementsByClassName('deleteRecord');
            $("[id=cbkTaxation]").prop('checked', flag);
        }
        fnLoadBasic();
        function fnLoadBasic() {
            $(document).ready(function () {

                $('#cbkCheckall').click(function () {
                    if ($('#cbkCheckall').is(':checked')) {
                        Checkall(true);
                    } else
                        Checkall(false);
                });

                var textfieldname = $("#txtTaxCode");
                DropdownFunction(textfieldname);
                
                $("#txtTaxCode").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTaxation") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {    
                                        label: item.split(',')[1],
                                        val: item.split(',')[0]
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
                        $('#hifTaxationID').val(i.item.val);
                        //alert($('#hifTaxationID').val());
                    },
                    minLength: 0
                });


                var textfieldname = $("#txtActivityRateType");
                DropdownFunction(textfieldname);

                $("#txtActivityRateType").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityRateTypeandTaxation") %>',
                            data: "{ 'prefix': '" + request.term + "','taxation':'" + $('#hifTaxationID').val() + "'}",
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
                        $('#hifActivityRateTypeID').val(i.item.val);
                        //alert($('#hifTaxationID').val());
                    },
                    minLength: 0
                });
            });
        }
        function Succes() {
            showStickyToast(true, "Successfully Updated", false);
        }
        function SuccesDel() {
            showStickyToast(true, "Successfully Deleted", false);
        }


        function validation() {
            debugger;
            var Taxcode = hifTaxationID.val();
            var Tariff = hifActivityRateTypeID.val();
        }
    </script>

    <div class="dashed"></div> 
      <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <a href="#">3PL</a> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Activity Taxation<asp:Literal ID="ltFormSubHeading" runat="server"/> </span></div>
                <%--<div class="mandatory"><b>Note:</b> <span style="color:red"> __ </span>Indicates mandatory fields</div>--%>
            </div>

        </div>
    <div class="container">
    <table class="" width="100%" align="center">
        <tr>
            <td colspan="2">
                <div class="FormSubHeading" style="font-size: larger"> </div>
            </td>
        </tr>
        <tr>
            <td>
                
                <asp:Label ID="lblStatus" runat="server" />
            </td>
            <td align="right">
               <%-- <asp:LinkButton runat="server" ID="lnkAddNew" CssClass="btn btn-primary right"  OnClick="lnkAddNew_Click" >Add New <%= MRLWMSC21Common.CommonLogic.btnfaNew%> </asp:LinkButton>--%>
                 <asp:LinkButton runat="server" ID="lnkAddNew" CssClass="btn btn-primary right"  OnClick="lnkAddNew_Click" > <%= GetGlobalResourceObject("Resource", "AddNew")%><%= MRLWMSC21Common.CommonLogic.btnfaNew%> </asp:LinkButton>
            </td>
        </tr>
<%--        <tr>
            <td colspan="2">
                &nbsp;&nbsp;
            </td>
        </tr>--%>
        <tr>
            <td colspan="2" align="center">

    
                <asp:GridView ID="gvActivityTaxaction" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" SkinID="gvLightSeaBlue" AllowPaging="true" AutoGenerateColumns="false" PageSize="25" Width="100%"
                    OnRowDataBound="gvActivityTaxaction_RowDataBound"
                    OnRowEditing="gvActivityTaxaction_RowEditing"
                    OnRowCancelingEdit="gvActivityTaxaction_RowCancelingEdit"
                    OnRowUpdating="gvActivityTaxaction_RowUpdating"
                    OnPageIndexChanging="gvActivityTaxaction_PageIndexChanging">
                    <Columns>
                       <%-- <asp:TemplateField HeaderText="Tax Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,TaxCode%>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                            <ItemTemplate>
                                <asp:Literal ID="ltTaxCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TaxCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="gridInput">
                                <asp:RequiredFieldValidator ID="rfvTaxCode" runat="server" ControlToValidate="txtTaxCode" ValidationGroup="vgTaxaction" ErrorMessage="*" />
                                <asp:TextBox ID="txtTaxCode" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TaxCode") %>'/>
                                <asp:HiddenField ID="hifTaxationID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"TaxationID") %>'/>
                                <asp:HiddenField ID="hifActivityTaxationID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityTaxationID") %>' />
                                <span class="errorMsg"></span>
                                    </div>
                            </EditItemTemplate>
                        </asp:TemplateField>


                    <%--    <asp:TemplateField HeaderText="HSN Code" ItemStyle-Width="180">--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,HSNCode%>"  ItemStyle-Width="180">
                            <ItemTemplate>
                                <asp:Literal ID="ltHSNCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HSNCode") %>' ></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <div class="gridInput">
                                <asp:TextBox ID="txtHSNCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HSNCode") %>'></asp:TextBox>
                                     </div>
                            </EditItemTemplate>
                        </asp:TemplateField>

                       <%-- <asp:TemplateField HeaderText="Tariff Sub-Group" ItemStyle-Width="180">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,TariffSubGroup%>"   ItemStyle-Width="180">
                            <ItemTemplate>
                                <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <div class="gridInput">
                                <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" ControlToValidate="txtActivityRateType" ValidationGroup="vgTaxaction" ErrorMessage="*" />
                                <asp:TextBox ID="txtActivityRateType" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>'/>
                                <asp:HiddenField ID="hifActivityRateTypeID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateTypeID") %>'/>
                           <span class="errorMsg"></span>
                                    </div>
                                     </EditItemTemplate>
                        </asp:TemplateField>

                      
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                            <HeaderTemplate>
                                 <div class="checkbox"><asp:CheckBox ID="cbkCheckall" runat="server" ClientIDMode="Static"  /><%--<label>Delete</label>--%>
                                     <label><%= GetGlobalResourceObject("Resource", "Delete")%></label>
                                 </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <label class="check-box"><asp:CheckBox ID="cbkTaxation" runat="server" ClientIDMode="Static" />  </label>
                                <asp:HiddenField ID="hifItemTenantActivityRateID" Value='<%#String.Format("{0}",DataBinder.Eval(Container.DataItem,"ActivityTaxationID")) %>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return window.confirm('Are you sure want to delete records');" Font-Underline="false"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                            </FooterTemplate>
                            <EditItemTemplate></EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:CommandField ValidationGroup="vgTariff"  ControlStyle-CssClass="ButEmpty" ButtonType="Link" ItemStyle-CssClass="ButEmpty"  ItemStyle-HorizontalAlign="Right" EditText="<i class='material-icons ss'>mode_edit</i>" ShowEditButton="True" ItemStyle-Width="20" />
                    </Columns>
                    <EmptyDataTemplate>
						<%--<div align="center">No Data Found</div>--%>
                        <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
					</EmptyDataTemplate>
                </asp:GridView>

                            </td>
        </tr>
    </table>
    </div>
</asp:Content>
