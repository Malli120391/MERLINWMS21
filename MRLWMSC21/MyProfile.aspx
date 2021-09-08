<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="MRLWMSC21.MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <style>
                .iconinc {
                        font-size: 70px;
                }
            </style>                  

     <div class="container">
         <fieldset style="border: 1px solid #0B4C5F; background-color: #FFFFFF; text-align: center; width: 360px; border: 0; ">
            <table cellpadding="4" cellspacing="4" border="0" width="100%" >
												     <span class="material-icons iconinc">person_pin</span>
												    <tr><td style="    text-align: center;">
												    
												    <b><asp:Label ID="lblInfo"  CssClass="smlText" runat="server" /></b>
												    </td></tr>
												    
												    <tr>
                                                        <td>
                                                            <div class="flex">
												                <asp:TextBox ID="txtMobile" runat="server" ValidationGroup="profileEdit" CssClass="txt_slim_req" Width="95%" required="" /> 
												                <label>Mobile</label>
                                                                 <span class="errorMsg"></span>
												                <asp:RequiredFieldValidator id="rfvtxtMobile"
                                                                                  runat="server" 
                                                                                  ControlToValidate="txtMobile"
                                                                                  ErrorMessage=" (Mobile No. is required!)"
                                                                                  SetFocusOnError="True" 
                                                                                  Display="Dynamic"
                                                                                  ValidationGroup="profileEdit"
                                                                                  CssClass="errorMsg"
                                                                                   />
                                                            </div>
                                                        </td>
												    </tr>
												    
												    <tr><td>
                                                        <div class="flex">

												    	
                                                                             
												    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" ValidationGroup="profileEdit" CssClass="txt_slim_req" Width="95%" required="" /> 
												     <label>Current Password</label>
                                                    <span class="errorMsg"></span>
												    <asp:RequiredFieldValidator id="rfvtxtOldPassword"
                                                                                  runat="server" 
                                                                                  ControlToValidate="txtOldPassword"
                                                                                  ErrorMessage=" (Current password is required!)"
                                                                                  SetFocusOnError="True" 
                                                                                  Display="Dynamic" 
                                                                                  ValidationGroup="profileEdit"
                                                                                  CssClass="errorMsg"
                                                                                  />
                                                        </div>
                                                            </td></tr>
												    
												    <tr><td><div class="flex">
												       
												    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" ValidationGroup="profileEdit" CssClass="txt_slim_req" Width="95%" required="" /> 
												    <label>New Password </label></div></td></tr>
												    
												    <tr><td>
                                                        <div class="flex">
												    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" ValidationGroup="profileEdit" CssClass="txt_slim_req" Width="95%" required="" /> 
												   <label>Re-enter New-Password</label></div> </td></tr>
												    
												                              <asp:CompareValidator id="comparePasswords" Visible="false" 
                                                                                  runat="server"
                                                                                  ControlToCompare="txtNewPassword"
                                                                                  ControlToValidate="txtConfirmPassword"
                                                                                  ErrorMessage=" (Your passwords do not match up!)"
                                                                                  Display="Dynamic"
                                                                                  ValidationGroup="profileEdit"
                                                                                   CssClass="errorMsg"
                                                                                   />
                                                                                 
												    
												    <tr><td> 
											
                                                                             

												     </td></tr>
												    
												        <tr><td style="display: flex; justify-content: center;">
                                                            <%--<button id="btnChangePassword" type="button" runat="server">Change Password <%= MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
												        <asp:LinkButton  ID="lnkCancel" runat="server" CssClass="btn btn-primary"    OnClick="lnkCancel_Click"  > Cancel <%= MRLWMSC21Common.CommonLogic.btnfaClear %> </asp:LinkButton>  &nbsp;&nbsp;
												    <asp:LinkButton ValidationGroup="profileEdit"  ID="lnkChangePassword" runat="server"  CssClass="btn btn-sm btn-primary" OnClick="lnkChangePassword_Click"  > Update Profile <%= MRLWMSC21Common.CommonLogic.btnfaSave %> </asp:LinkButton> 
                                                            <br /><br />
												    </td></tr>
												    
												</table>
    
           <asp:Literal ID="ltStatus"   runat="server" />

       </fieldset>
</asp:Content>
