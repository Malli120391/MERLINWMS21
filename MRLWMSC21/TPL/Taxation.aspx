<%@ Page Title="Taxation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Taxation.aspx.cs" Inherits="FalconAdmin._3PLBilling.Taxation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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

                $("#txtFromDate").datepicker({
                    dateFormat: "dd-M-yy",
                    onSelect: function (selected) {
                        $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" });
                    }
                });
                $("#txtToDate").datepicker({
                    dateFormat: "dd-M-yy",
                });
            });
        }
        function Succes() {
            showStickyToast(true, "Successfully Updated", false);
        }
        function SuccesDel() {
            showStickyToast(true, "Successfully Deleted", false);
        }
        function Exists() {
            showStickyToast(false, "Tax Code already exists", false);
        }
    </script>
    <link href="tpl.css" rel="stylesheet" />
        <style>
        .gvLightSeaBlue_headerGrid th {
            padding: 12px;
            font-size: 11.5px;
        }

        .gvLightSeaBlue_DataCellGrid td {
            font-size: 11.5px;
        }

        .gvLightSeaBlue_DataCellGridAlt td {font-size:14px;
        }
       
        .gvCell
        {
            text-align: center !important;
        }
        .gridInput input, .gridInput inputselect{
               width: 100px !important;
    font-size: 13px;
        }
    </style>
      <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <a href="#">3PL</a> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Taxation<asp:Literal ID="ltFormSubHeading" runat="server"/> </span></div>
                <%--<div class="mandatory"><b>Note:</b> <span style="color:red"> __ </span>Indicates mandatory fields</div>--%>
            </div>

        </div>
    <div class="container">

        <div>


            <div class="row">
                <div class="" flex end>
                    <div>
                        <asp:Label ID="lblStatus" runat="server" />
                    </div>
                    <div>
                        <asp:LinkButton runat="server" ID="lnkAddNew" CssClass="btn btn-primary right" OnClick="lnkAddNew_Click"> <%= GetGlobalResourceObject("Resource", "AddNew")%> <%= MRLWMSC21Common.CommonLogic.btnfaNew%> </asp:LinkButton></div>
                </div>
            </div>

            <div class="row">
                <div colspan="2" align="center">

                    <div style="">
                        <asp:GridView ID="gvActivityTaxaction" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" SkinID="gvLightSeaBlue" AllowPaging="true" AutoGenerateColumns="false" PageSize="25" Width="70%"
                            OnRowDataBound="gvActivityTaxaction_RowDataBound"
                            OnRowEditing="gvActivityTaxaction_RowEditing"
                            OnRowCancelingEdit="gvActivityTaxaction_RowCancelingEdit"
                            OnRowUpdating="gvActivityTaxaction_RowUpdating"
                            OnPageIndexChanging="gvActivityTaxaction_PageIndexChanging">
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="Tax Code" ItemStyle-Width="120">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,TaxCode%>" ItemStyle-Width="120">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltTaxCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TaxCode") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                            <asp:RequiredFieldValidator ID="rfvTaxCode" runat="server" ControlToValidate="txtTaxCode" ValidationGroup="vgTaxaction" />
                                            <asp:TextBox ID="txtTaxCode" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TaxCode") %>' />
                                            <asp:HiddenField ID="hifTaxationID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"TaxationID") %>' />
                                            <span class="errorMsg"></span>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- <asp:TemplateField HeaderText="Description" ItemStyle-Width="80">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,Description%>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                            <asp:TextBox ID="txtDescription" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- <asp:TemplateField HeaderText="From Date" ItemStyle-Width="80">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,FromDate%>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltFromDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FromDate","{0:dd/MM/yyyy}") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate" ValidationGroup="vgTaxaction" />
                                            <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FromDate","{0:dd/MM/yyyy}") %>' />
                                            <span class="errorMsg"></span>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <%--  <asp:TemplateField HeaderText="To Date" ItemStyle-Width="80">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,ToDate%>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltToDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ToDate","{0:dd/MM/yyyy}") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate" ValidationGroup="vgTaxaction" ErrorMessage="*" />
                                            <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ToDate","{0:dd/MM/yyyy}") %>' />
                                            <span class="errorMsg"></span>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <%--    <asp:TemplateField HeaderText="Percentage (%)" ItemStyle-Width="80" ItemStyle-CssClass="gvCell">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,Percentage%>" ItemStyle-Width="80" ItemStyle-CssClass="gvCell">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltPercentage" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Percentage") %>' />

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                            <asp:TextBox ID="txtPercentage" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Percentage") %>' />
                                             <span class="errorMsg"></span>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="State Code" ItemStyle-Width="80">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,StateCode%>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltStateCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StateCode") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                            <asp:TextBox ID="txtStateCode" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StateCode") %>' />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30">
                                    <HeaderTemplate>
                                        <div class="checkbox">
                                            <asp:CheckBox ID="cbkCheckall" runat="server" ClientIDMode="Static" /><%--<lable>Delete</lable>--%>
                                            <lable> <%= GetGlobalResourceObject("Resource", "Delete")%></lable>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <label class="check-box">
                                            <asp:CheckBox ID="cbkTaxation" runat="server" ClientIDMode="Static" /></label>
                                        <asp:HiddenField ID="hifTaxationID" Value='<%#String.Format("{0}",DataBinder.Eval(Container.DataItem,"TaxationID")) %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return window.confirm('Are you sure want to delete records');" Font-Underline="false"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                    </FooterTemplate>
                                    <EditItemTemplate></EditItemTemplate>
                                </asp:TemplateField>

                                <asp:CommandField ValidationGroup="vgTariff" ControlStyle-CssClass="ButEmpty" ButtonType="Link" ItemStyle-CssClass="ButEmpty" ItemStyle-HorizontalAlign="Right" EditText="<i class='material-icons ss'>mode_edit</i>" ShowEditButton="True" ItemStyle-Width="20" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%--<div align="center">No Data Found</div>--%>
                                <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%> </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
