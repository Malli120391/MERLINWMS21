<%@ Page Title=" Workstation :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="WorkCenters.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.WorkCenters" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

    <asp:ScriptManager ID="spmngrWorkCenter" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    
    <script type="text/javascript" src="../Scripts/CommonScripts.js"></script>
   

    <table border="0" cellpadding="2" cellspacing="2" width="80%"  align="center">

        <tr>
            <td colspan="3">
                <br />
                <asp:Label CssClass="errorMsg" runat="server" ID="lblWCStatus" ></asp:Label>
            </td>
        </tr>

        <tr>
            <td colspan="3" align="right">
                <asp:HyperLink ID="hlNewWorkstation"  NavigateUrl="~/mManufacturingProcess/WorkCenters.aspx" runat="server" CssClass="ui-button-small">New Workstation<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:HyperLink>
                &nbsp;&nbsp;&nbsp;
                <asp:HyperLink ID="hlWorkstationList"  NavigateUrl="~/mManufacturingProcess/WorkCenterList.aspx" runat="server" CssClass="ui-button-small" >Workstation List<%=MRLWMSC21Common.CommonLogic.btnfaList %></asp:HyperLink>
                <br /><br />
            </td>
        </tr>
        <tr>    
             <td class="FormLabels"> 
                 <asp:RequiredFieldValidator InitialValue="0" ID="rfvddlWorkGroup" SetFocusOnError="true" ControlToValidate="ddlWorkGroup" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateWC"/>
                Workstation Type: <br />
                <asp:DropDownList runat="server" ID="ddlWorkGroup" Width="200"></asp:DropDownList>
            </td>

            <td class="FormLabels">
                    <asp:RequiredFieldValidator ID="rfvtxtWCName" SetFocusOnError="true" ControlToValidate="txtWCName" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateWC"/>
                Workstation Name: <br />
                <asp:TextBox ID="txtWCName" runat="server" Width="200" ></asp:TextBox>
            </td>
            <td class="FormLabels">
                   <asp:RequiredFieldValidator ID="rfvtxtWCCode" SetFocusOnError="true" ControlToValidate="txtWCCode" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateWC"/>
                Code: <br />
                <asp:TextBox ID="txtWCCode" runat="server" Width="200"></asp:TextBox>
            </td>
            
        </tr>

        <tr>
           
            <td class="FormLabels">
                Working Period (Hours/Week): <br />
                <asp:TextBox runat="server" ID="txtWRPeriod" Width="200" onKeyPress="return checkDec(event)"></asp:TextBox>
            </td>
           
            <td class="FormLabels">
                Active: <br />
                <asp:CheckBox runat="server" ID="chkActive" />
            </td>

             <td class="FormLabels" style="visibility:hidden;">
                 <asp:RequiredFieldValidator InitialValue="0" ID="rfvddlResourceType" SetFocusOnError="true" ControlToValidate="ddlResourceType" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server"/>
                Resource Type: <br />
                <asp:DropDownList runat="server" ID="ddlResourceType" Width="200"></asp:DropDownList>
            </td>

            

        </tr>

        <tr>
            <td colspan="3">

                 <h3><asp:Label ID="Label1" runat="server" Text="Users" /></h3>

                  <asp:ListBox SelectionMode="Multiple" runat="server" ID="listUsers" Width="200" Height="250" > </asp:ListBox>

                
                <table border="0" width="50%" cellpadding="3" cellspacing="4" align="left" style="display:none;">

                    <tr>
                        <td>
                            <h3><asp:Label runat="server" Text="Capacity Information" ></asp:Label></h3>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <h3><asp:Label  runat="server" Text="Costing Information" ></asp:Label></h3>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <h3><asp:Label runat="server" Text="Users" /></h3>
                        </td>
                    </tr>

                   

                   <tr>
                        <td class="FormLabels">
                            Capacity Per Cycle: <br />
                            <asp:TextBox runat="server" ID="txtCapacityPerCycle" Width="150" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                       <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                           &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="FormLabels">
                            Cost per hour:<br />
                            <asp:TextBox runat="server" ID="txtCostPerHour" Width="150" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                       <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                           &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                       <td rowspan="5" valign="top">
                          <!-- List users -->


                       </td>
                    </tr>

                    <tr>
                        <td class="FormLabels">
                            Time for 1 Cycle(hour):<br />
                            <asp:TextBox runat="server" ID="txtTimefor1Cycle" Width="150" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="FormLabels">
                             Cost per Cycle:<br />
                            <asp:TextBox runat="server" ID="txtCostPerCycle" Width="150" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        
                    </tr>

                    <tr>
                        <td class="FormLabels">
                            Time before prod.:<br />
                             <asp:TextBox runat="server" ID="txtTimeBeforeProd" Width="150" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="FormLabels">
                           <br />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="FormLabels">
                             Time after prod.:<br />
                             <asp:TextBox runat="server" ID="txtTimeAfterProd" Width="150" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="FormLabels">
                            <br />
                            
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td class="FormLabels">
                          <br />
                            <asp:TextBox runat="server" ID="txtEFFactor" Width="150" Visible="false" onKeyPress="return checkDec(event)"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="FormLabels">
                           <br />
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>

                    

                </table>

                
                <br />

            </td>
        </tr>

        <tr>
            <td class="FormLabels" colspan="3">

                Description: <br />

                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5"  Width="90%"></asp:TextBox>

            </td>
        </tr>

      

        <tr>
            <td colspan="3" align="right">
                <br />
                <asp:LinkButton runat="server"  ID="lnkCancel" OnClick="lnkCancel_Click" CssClass="ui-btn ui-button-large">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %> </asp:LinkButton>
                &nbsp;&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkButSave" OnClick="lnkButSave_Click" CssClass="ui-btn ui-button-large" ValidationGroup="InitiateWC"></asp:LinkButton>
            </td>
        </tr>

    </table>


    <br />

    

</asp:Content>
