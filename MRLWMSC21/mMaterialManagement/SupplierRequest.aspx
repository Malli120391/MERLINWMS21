<%@ Page Title="Supplier Request :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="SupplierRequest.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.SupplierRequest" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">

    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="supplierRequest" SupportsPartialRendering="true"></asp:ScriptManager>
    

    

     
     <script type="text/javascript">
       
        $(document).ready(function () {
              var prm = Sys.WebForms.PageRequestManager.getInstance();    
       prm.add_initializeRequest(InitializeRequest);
       prm.add_endRequest(EndRequest);

       // Place here the first init of the autocomplete
       InitAutoCompl();
    });        

    function InitializeRequest(sender, args) {
    }

    function EndRequest(sender, args) {
       // after update occur on UpdatePanel re-init the Autocomplete
       InitAutoCompl();
    }

         function InitAutoCompl() {
             var textfieldname = $("#<%= this.atcBankCurrency.ClientID %>");
             DropdownFunction(textfieldname);
             $("#<%= this.atcBankCurrency.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCurrencyData") %>',
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
                         }
                     });
                 },
                 select: function (e, i) {
                     $("#<%=hifBankCurrency.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
             });

             textfieldname = $("#<%= this.atcCountry.ClientID %>");
             DropdownFunction(textfieldname);
             $("#<%= this.atcCountry.ClientID %>").autocomplete({
                 source: function (request, response) {

                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountries") %>',
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
                         }
                     });
                 },
                 select: function (e, i) {
                     $("#<%=hifCountry.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
             });

             textfieldname = $("#<%= this.atcBankCountry.ClientID %>");
             DropdownFunction(textfieldname);
             $("#<%= this.atcBankCountry.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountries") %>',
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
                         }
                     });
                 },
                 select: function (e, i) {
                     $("#<%=hifBankCountry.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
             });

             var textfieldname = $("#<%= this.txtTenant.ClientID %>");
             DropdownFunction(textfieldname);
             $("#<%= this.txtTenant.ClientID %>").autocomplete({
                 source: function (request, response) {
                     debugger;
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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
                     $("#<%=hifTenant.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
             });

             var TextFieldName = $('#txtCompanyName');
             DropdownFunction(TextFieldName);
             $("#txtCompanyName").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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

                     $("#hifTenantID").val(i.item.val);
                 },
                 minLength: 0
             });

             var TextFieldName = $("#<%= this.txtAccount.ClientID %>");
             DropdownFunction(TextFieldName);
             $("#<%= this.txtAccount.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountDataFor3PL") %>',
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

                     $("#<%=hifAccount.ClientID %>").val(i.item.val);
                    <%--alert($("#<%=hifAccount.ClientID %>").val())--%>
                 },
                 minLength: 0
             });

         }
          $(function () {
            $("#<%=txtIBANNo.ClientID %>, #<%=txtSortCode.ClientID %>, #<%=txtSwiftCode.ClientID %>").keypress(function (e) {
                 var regex = new RegExp("^[a-zA-Z0-9]+$");
                 var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
                 if (regex.test(str)) {
                     return true;
                 }

                 e.preventDefault();
                 return false;
             });
         });
       

         function checkAlpha(e) {
             var regex = new RegExp("^[a-zA-Z0-9]+$");
             var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
             if (regex.test(str)) {
                 return true;
             }

             e.preventDefault();
             return false;
         }

         function validate() {
             debugger;
             if ($("#<%=hifAccount.ClientID %>").val() == 0 || $("#<%=hifAccount.ClientID %>").val() == null || $("#<%=hifAccount.ClientID %>").val() == undefined || $("#<%=txtAccount.ClientID %>").val() == "" || $("#<%=txtAccount.ClientID %>").val() == null || $("#<%=txtAccount.ClientID %>").val() == undefined) {
                 showStickyToast(false, "Please select Account");
                 return false;
             }
             else if ($("#<%=hifTenant.ClientID %>").val() == 0 || $("#<%=hifTenant.ClientID %>").val() == null || $("#<%=hifTenant.ClientID %>").val() == undefined || $("#<%=txtTenant.ClientID %>").val() == "" || $("#<%=txtTenant.ClientID %>").val() == null || $("#<%=txtTenant.ClientID %>").val() == undefined) {
                 showStickyToast(false, "Please select Tenant");
                 return false;
             }
             else {
                 return true;
             }

         }
       
        </script>
    <style>
        .errorMsg {
            display:none;
        }
   

    </style>
    <style>
        .ui-autocomplete-input {
            --md-arrow-width: 1em;
            background: url(../Images/magnifier.svg) calc(100% - var(--md-arrow-offset) - var(--md-select-side-padding)) center no-repeat !important;
            background-size: var(--md-arrow-width) !important;
        }

    </style>
   <%-- <style>
        .txt_Blue_Small {
                margin-bottom: 15px;
        }
    </style>--%>
    <div class="dashed"></div>

      <asp:UpdateProgress ID="uprgSearchOutbound" runat="server" AssociatedUpdatePanelID="upnlSearchOutbound">
            <ProgressTemplate>
                <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                <%--<div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>--%>
                <div style="align-self:center;" >
                        <div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>

                </div>
                                  
                </div>
                                
                                
            </ProgressTemplate>
            </asp:UpdateProgress>
    <div class="container">
        <asp:UpdatePanel ID="upnlSearchOutbound" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
            <ContentTemplate>

                 <div class="">
        
                    <div class="row">
                        <%--<td colspan ="2">Note: <span style="color:red"> __ </span>Indicates mandatory fields

                        </td>--%>
                          <div align="right">
                              <div align="" flex end>
                                  <!-- Globalization Tag is added for multilingual  -->
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-sm btn-primary"  PostBackUrl="~/mMaterialManagement/SupplierList.aspx"><i class="material-icons vl">arrow_back</i>Back to List</asp:LinkButton>
                                  </div>
                        </div>

                    </div>
    
                    <div class="row">
                        <div class="col m12"><asp:Label ID="lblStatus" runat="server" CssClass ="errorMsg" />

                        </div>

                    </div>


                      <div class="row">
                        <div class="">

                            <%--<div class="ui-SubHeading ui-SubHeadingBar" style=""> Supplier Details--%>
                           <div class="ui-SubHeading ui-SubHeadingBar" style=""> <%= GetGlobalResourceObject("Resource", "SupplierDetails")%>

</div>
                
                            <div class="ui-Customaccordion">
                                <gap></gap>

                                <div>
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class=" flex ">
                                                    <asp:TextBox ID="txtAccount" runat="server" CssClass="txt_slim" SkinID="txt_Hidden_Req" required="" />
                                                    <%-- <span class="errorMsg"> * </span> <label>Account  </label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label><%= GetGlobalResourceObject("Resource", "Account")%> </label>
                                                    <asp:HiddenField ID="hifAccount" runat="server" Value="0" />
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                  <div>
                                                 <%--   <asp:RequiredFieldValidator ID="rfvTenant" runat="server" ControlToValidate="txtTenant" Display="Dynamic" ErrorMessage=" * " />--%>

                                                </div>
                                                
                                                <asp:TextBox ID="txtTenant" runat="server" CssClass="txt_slim" SkinID="txt_Hidden_Req" required="" />
                                                <span class="errorMsg"></span>
                                                <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                                <asp:HiddenField ID="hifTenant" runat="server" value="0"/>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvtxtSupplierName" runat="server" Display="Dynamic" ControlToValidate="txtSupplierName" ErrorMessage=" * " />

                                                </div>

                                                <asp:TextBox ID="txtSupplierName" runat="server" onKeyPress="return checkSpecialChar(event)" CssClass="txt_slim" MaxLength="250" required="" />
                                                <span class="errorMsg"></span>
                                                <label><%= GetGlobalResourceObject("Resource", "SupplierName")%></label>

                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvSupplierCode" runat="server" Display="Dynamic" ControlToValidate="txtSupplierCode" ErrorMessage=" * " />


                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtSupplierCode" runat="server" CssClass="txt_slim" onKeyPress="return checkSpecialChar(event)" MaxLength="30" required="" />
                                                    
                                                    <span class="errorMsg"></span>
                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "SupplierCode")%>
                                                        <asp:Literal ID="ltSupplierCode" runat="server" />
                                                    </label>
                                                </div>
                                            </div>

                                        </div>

                                    </div>


                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvtxtTelephoneNo1" runat="server" Display="Dynamic" ControlToValidate="txtTelephoneNo1" ErrorMessage=" * " />

                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtTelephoneNo1" runat="server" onKeyPress="return Phoneset(event)" CssClass="txt_slim" MaxLength="14" required="" />
                                                  
                                                    <span class="errorMsg">* </span>
                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "TelephoneNo")%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <asp:TextBox ID="txtTelephoneNo2" runat="server" onKeyPress="return Phoneset(event)" CssClass="txt_slim" MaxLength="14" required="" />
                                                    <label><%= GetGlobalResourceObject("Resource", "TelephoneNos")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                    
                                        <div class="col m3 s3">
                                            <div class="flex">
                                          
                                                <div>
                                                    <asp:TextBox ID="txtMobileNo" runat="server" onKeyPress="return Phoneset(event)" CssClass="txt_slim" MaxLength="10" required="" />
                                                  
                                                    <label><%= GetGlobalResourceObject("Resource", "MobileNo")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3" colspan="1">
                                            <div class="flex">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvtxtSupplierAddress1" runat="server" Display="Dynamic" ControlToValidate="txtSupplierAddress1" ErrorMessage=" * " />

                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtSupplierAddress1" runat="server" onKeyPress="return Addressset(event)" CssClass="txt_slim" MaxLength="250" required="" />
                                                    <%-- <span class="errorMsg"> * </span><label>  Address 1</label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label><%= GetGlobalResourceObject("Resource", "Address1")%> </label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>


                                    <div class="row">
                                        <div class="col m3 s3" colspan="1">
                                            <div class=" flex ">
                                             
                                                <div>
                                                    <asp:TextBox ID="txtSupplierAddress2" runat="server" onKeyPress="return Addressset(event)" CssClass="txt_slim" MaxLength="250" required="" />
                                                 
                                                    <label><%= GetGlobalResourceObject("Resource", "Address2")%></label>

                                                </div>

                                            </div>
                                        </div>
                                        <div class="col m3 s3" colspan="1">
                                            <div class=" flex ">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvddlCountry" runat="server" Display="Dynamic" ControlToValidate="atcCountry" ErrorMessage=" * " />

                                                </div>
                                                <div>
                                                    <asp:TextBox ID="atcCountry" runat="server" onKeyPress="return checkSpecialChar(event)" SkinID="txt_Req" required="" />
                                                    <%--<span class="errorMsg"> * </span><label>  Country </label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label><%= GetGlobalResourceObject("Resource", "Country")%></label>
                                                    <asp:HiddenField ID="hifCountry" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3" colspan="1">
                                            <div class="flex">
                                               
                                                <div>
                                                    <asp:TextBox ID="txtFaxNo" runat="server" onKeyPress="return checkSpecialChar(event)" CssClass="txt_slim" MaxLength="50" required="" />
                                                  
                                                    <label><%= GetGlobalResourceObject("Resource", "FaxNo")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3" colspan="1" valign="top">
                                            <div class="flex">
                                               
                                                <div class="">
                                                    <asp:TextBox ID="txtEmailAddress" runat="server" onKeyPress="return Emailset(event)" CssClass="txt_slim" MaxLength="50" required="" />
                                                    <%--<label>  Email Address</label><asp:RequiredFieldValidator ID="rfvtxtEmailAddress" runat ="server" Display ="Dynamic" ControlToValidate ="txtEmailAddress" ErrorMessage =" * " />--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "EmailAddress")%> </label>
                                                    <asp:RequiredFieldValidator ID="rfvtxtEmailAddress" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" ErrorMessage=" * " />
                                                    <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" ErrorMessage="Invalid E-Mail Address" CssClass="errorMsg" ValidationExpression="\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z" />
                                                </div>
                                            </div>
                                        </div>
                                      
                                    </div>
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <asp:TextBox ID="txtSearchTerm" runat="server" onKeyPress="return checkSpecialChar(event)" CssClass="txt_slim" MaxLength="100" required="" />
                                                    <%--<label id="lblsearchterm" runat="server" style="display:none !important;"> Search Term <asp:Literal runat="server" ID="ltSearchTerm"   /></label>--%>
                                                    <label id="lblsearchterm" runat="server" style="display: none !important;">
                                                        <%= GetGlobalResourceObject("Resource", "SearchTerm")%>
                                                        <asp:Literal runat="server" ID="ltSearchTerm" /></label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>



                       <div class="row">
                        <div colspan="3">

<%--                            <div class="ui-SubHeading ui-SubHeadingBar" style="">Contact Person Details</div>--%>
                             <div class="ui-SubHeading ui-SubHeadingBar" style=""><%= GetGlobalResourceObject("Resource", "ContactPersonDetails")%>
</div>
                
                            <div class="ui-Customaccordion">


                                <div>
                                    <gap></gap>
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContactPerson" Display="Dynamic" ErrorMessage=" * " />

                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtContactPerson" onKeyPress="return checkSpecialChar(event)" runat="server" CssClass="txt_slim" MaxLength="50" required="" />
                                                    <%-- <span class="errorMsg"> * </span><label>  Name </label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "Name")%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class=" flex ">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtContactPersonTitle" Display="Dynamic" ErrorMessage=" * " />


                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtContactPersonTitle" onKeyPress="return checkSpecialChar(event)" runat="server" CssClass="txt_slim" MaxLength="50" required="" />
                                                    <%--<span class="errorMsg"> * </span><label>  Title</label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "Title")%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class=" flex ">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtContactPersonContactNo" Display="Dynamic" ErrorMessage=" * " />

                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtContactPersonContactNo" runat="server" onKeyPress="return Phoneset(event)" CssClass="txt_slim" MaxLength="14" required="" />
                                                    <%--<span class="errorMsg"> * </span><label>  Contact No.</label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "ContactNo")%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                   
                                        <div class="col m3 s3" colspan="1">
                                            <div class=" flex ">
                                                <div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmailAddressContactPerson" Display="Dynamic" ErrorMessage=" * " />

                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtEmailAddressContactPerson" runat="server" onKeyPress="return Emailset(event)" CssClass="txt_slim" MaxLength="50" required="" />
                                                    <%--<span class="errorMsg" > * </span><label>  Email Address</label>--%>
                                                    <span class="errorMsg">* </span>
                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "EmailAddress")%>
                                                    </label>
                                                    <%-- <asp:RegularExpressionValidator ID="revEmailContactPerson" runat="server" ControlToValidate ="txtEmailAddressContactPerson" ErrorMessage="Invalid E-Mail Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                </div>


                            </div>
                        </div>
                    </div>


                      <div class="row" style="display:none;">
                        <div colspan="3">

                           <%-- <div class="ui-SubHeading ui-SubHeadingBar" style="">Supplier Bank Details</div>--%>
                            <div class="ui-SubHeading ui-SubHeadingBar" style=""> <%= GetGlobalResourceObject("Resource", "SupplierBankDetails")%>
</div>
                
                            <div class="ui-Customaccordion">
               
        
                                  <div  width="100%">
                                      <gap></gap>
                                      <div class="row">
                                          <div class="col m3 s3" colspan="1">
                                              <div class=" flex ">
                                                
                                                  <div>
                                                      <asp:TextBox ID="txtBankName" runat="server" onKeyPress="return checkSpecialChar(event)" CssClass="txt_slim" MaxLength="100" required="" />
                                                      <%--<label> Bank Name</label>--%>
                                                      <label>
                                                          <%= GetGlobalResourceObject("Resource", "BankName")%>
                                                      </label>
                                                  </div>
                                              </div>
                                          </div>
                                          <div class="col m3 s3">
                                              <div class=" flex ">
                                                
                                                  <div>
                                                      <asp:TextBox ID="txtAccountNo" runat="server" onkeypress="return checkDec(this,event)" CssClass="txt_slim" MaxLength="50" required="" />
                                                      <%-- <label> Account No.</label>--%>
                                                      <label>
                                                          <%= GetGlobalResourceObject("Resource", "AccountNo")%>
                                                      </label>
                                                  </div>

                                              </div>
                                          </div>
                                          <div class="col m3 s3">
                                              <div class=" flex ">
                                                 
                                                      <asp:TextBox ID="txtIBANNo" runat="server" CssClass="txt_slim" MaxLength="50" required="" />
                                                      <%-- <label> IBAN No.</label>--%>
                                                      <label>
                                                          <%= GetGlobalResourceObject("Resource", "IBANNo")%>
                                                      </label>
                                                  </div>
                                              </div>
                                          </div>
                                      </div>


                                <div class="row">
                                    <div class="col m3 s3" colspan="1">
                                        <div class="flex">
                                            <div>
                                                <asp:TextBox ID="txtSortCode" runat="server" CssClass="txt_slim" MaxLength="15" required="" />
                                                <%-- <label> Sort Code or BLZ Code</label>--%>
                                                <label>
                                                    <%= GetGlobalResourceObject("Resource", "SortCodeorBLZCode")%>
                                                </label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3" colspan="1">
                                        <div class="FormLabels flex baseline">
                                           
                                            <div>
                                                <asp:TextBox ID="txtSwiftCode" runat="server" CssClass="txt_slim" MaxLength="15" required="" />
                                                <%--<label> Swift Code</label>--%>
                                                <label>
                                                    <%= GetGlobalResourceObject("Resource", "SwiftCode")%>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m3 s3" colspan="1">
                                        <div class="flex">
                                           
                                            <div>
                                                <asp:TextBox ID="atcBankCurrency" runat="server" onKeyPress="return checkSpecialChar(event)" SkinID="txt_Req" required="" />
                                                <%-- <label> Currency</label>--%>
                                                <label><%= GetGlobalResourceObject("Resource", "Currency")%></label>
                                                <asp:HiddenField ID="hifBankCurrency" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col m3 s3" colspan="1">
                                        <div class="flex">
                                           
                                            <div>
                                                <asp:TextBox ID="atcBankCountry" runat="server" onKeyPress="return checkSpecialChar(event)" SkinID="txt_Req" required="" />
                                                <%--    <label> Country</label>--%>
                                                <label>
                                                    <%= GetGlobalResourceObject("Resource", "Country")%>
                                                </label>
                                                <asp:HiddenField ID="hifBankCountry" runat="server" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3" colspan="1">
                                        <div class=" flex ">
                                          
                                            <div>
                                                <asp:TextBox ID="txtBankAddress" runat="server" onKeyPress="return Addressset(event)" TextMode="MultiLine" Rows="1" MaxLength="250" required="" Style="resize: vertical;" />
                                                <%--<label> Bank Address</label>--%>
                                                <label><%= GetGlobalResourceObject("Resource", "BankAddress")%></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>





                                <div class="row">
                                    <div class="col m3 s3" colspan="1">
                                        <asp:Literal ID="ltIsFirstEdit" runat="server" Visible="false" />

                                    </div>
                                    <div class="col m3 s3" colspan="2">
                                        <asp:Literal ID="ltSupplierCodeAprEditCount" runat="server" Visible="false" />

                                    </div>
                                </div>



                            </div>


                            </div>
                        </div>
                    </div>
                       
                  <div class="row">
                      <div colspan="3">

                          <%--<div class="ui-SubHeading ui-SubHeadingBar" style=""> Tenant Details</div>
                
                            <div class="ui-Customaccordion">--%>

                        <div width="100%"  visible="false">
                             <div class="row">

                                <div align="right" style="vertical-align: top;">

                                    <asp:LinkButton ID="lnkAddTenant" OnClick="lnkAddTenant_Click" Visible="false" runat="server" CssClass="ui-button-small">Add Tenant<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                                </div>

                              </div>
                                <div class="row">
                                          
                                    <div class="cpl m3">
                                        <asp:Label runat="server" ID="lblSupplierStatus" CssClass="errorMsg" ForeColor="DarkGreen"></asp:Label>
                                    </div>
                                </div>

                                <div class="row">
                                    <div>

                          <asp:Panel ID="pnlSupplierList" runat="server" Visible="false" >
              
                                      <asp:GridView SkinID="gvLightOrangeNew"    ID="gvSupplierList" runat="server"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="10" AllowSorting="True"  
                                          HorizontalAlign="Left"   
                                          OnSorting="gvSupplierList_Sorting" 
                                          OnPageIndexChanging="gvSupplierList_PageIndexChanging" 
                                          OnRowDataBound="gvSupplierList_RowDataBound"
                                          OnRowEditing="gvSupplierList_RowEditing"
                                          OnRowCancelingEdit="gvSupplierList_RowCancelingEdit"
                                          OnRowUpdating="gvSupplierList_RowUpdating" >
                                                        <Columns>

                                                          <%--  <asp:TemplateField ItemStyle-Width="350" HeaderText="Tenant"   >--%>
                                                                 <asp:TemplateField ItemStyle-Width="350" HeaderText= "<%$Resources:Resource,Tenant%>"  >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>'/>
                                                                    <asp:Literal ID="ltTenantID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />
                                                                    <asp:Literal ID="ltTenantSupplierID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Tenant_SupplierID") %>' />
                                                                </ItemTemplate>

                                                                <EditItemTemplate>
                                                                   <asp:Literal ID="ltTenantSupplierID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Tenant_SupplierID") %>' />
                                                                    <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" Display="Dynamic" ControlToValidate="txtCompanyName" ValidationGroup="vRequiredCompanyName" ErrorMessage="*" />
                                                                    <asp:TextBox ID="txtCompanyName" ClientIDMode="Static" SkinID="txt_Req" Width="250" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CompanyName") %>' />
                                                                    <asp:HiddenField ID="hifTenantID" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "TenantID").ToString() %>' />
                                    
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <%--<asp:TemplateField ItemStyle-Width="300" HeaderText="Company Registration No"   >--%>
                                                                  <asp:TemplateField ItemStyle-Width="300" HeaderText=  "<%$Resources:Resource,CompanyRegistrationNo%>"  >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltTenantRegistrationNo" Text='<%# DataBinder.Eval(Container.DataItem, "TenantRegistrationNo") %>'/>
                                                        
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                             <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Company DBA"  >--%>
                                                                 <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,CompanyDBA%>"  >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyDBA").ToString() %>'/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                           <asp:TemplateField ItemStyle-Width="50" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">

                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate></EditItemTemplate>

                                                                <FooterTemplate>
                                                                    <asp:LinkButton ID="lnkSupplierDelete" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkSupplierDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                        <asp:CommandField ValidationGroup="vRequiredCompanyName" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="50" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />

                                               
                                                   
                                                  
                                                
                                                        </Columns>
                                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                                                            Mode="NumericFirstLast" PageButtonCount="15" />
                                           
                                                </asp:GridView>
                     
                              </asp:Panel>
                                    </div>

                                    </div>
                            </div>

                                <%--</div>--%>
                      </div>
                  </div>
                    <div class="row">
                        <div class="col m3 s3" colspan="4" align="left">
                            <div class="checkbox">
                                    <asp:CheckBox ID="chkIsActive" Text="Active" runat="server"   />  
                                   <%--<label> Active</label>--%>
                                    <asp:CheckBox ID="chkIsApproved" runat ="server" Text ="Approved" Visible="false" />
                                </div>
                  </div>
                  </tr>

      
                    <div>
                      <div colspan="3" align="right" >
                          <div align="right" >
                             <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="lnkButCancel" OnClick="lnkButCancel_Click">

                             <%--Cancel <%= MRLWMSC21Common.CommonLogic.btnfaClear %>--%>
                                  <%= GetGlobalResourceObject("Resource", "Cancel")%> <%= MRLWMSC21Common.CommonLogic.btnfaClear %>
                                 </asp:LinkButton>

                           

                        <asp:LinkButton  ID="lnkSendRequest"  CssClass="btn btn-primary" OnClientClick="showAsynchronus();" runat="server"  OnClick="lnkSendRequest_Click" >
                
                            </asp:LinkButton>
                         </div>
                  </div>

                  </div>
             

        
        
            </table>

        </ContentTemplate>
    </asp:UpdatePanel></div>
 
    <style>
        .oneafter label::after {
            display:none;
        }
          .oneafter label {
           width:50%;
           word-wrap:break-word;
        }
    </style>
<%--<style>
    #MainContent_MMContent_LinkButton3 {    background: #e7852b;
    }
</style>--%>
</asp:Content>
