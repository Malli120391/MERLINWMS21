<%@ Page Title=" .: Bulk NC List :. " Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="BulkNCList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.BulkNCList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    
    <script type="text/javascript">

        function showStickyToast1(type, message, IsParmenent) {
            var val;
            var time;
            if (type == true)
                val = 'success';
            else
                val = 'error';
            $().toastmessage('showToast', {
                stayTime: 2600,
                text: message,
                sticky: IsParmenent,
                position: 'bottom-right',
                type: val,
                closeText: '',
                close: function () {
                },

            });

        }


        $(function () {
            $("[id*=tvBulkNCList] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).attr("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {
                    /*
                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    } else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }*/
                }
            });
        })



        function CheckGMDQty(TextBox, GMDValue) {

           
          

            if (TextBox.value == "")
            {
                return;
            }

            if (parseFloat(TextBox.value) > parseFloat(GMDValue)) {


                showStickyToast1("tue", "Quantity cannot exceed 'Captured' value", false);
                TextBox.value = "";
                TextBox.focus();
                return;

            }
            else {

                if (parseFloat(TextBox.value) == parseFloat("0"))
                {
                    showStickyToast1("tue", "Quantity cannot be 0 or empty", false);
                    TextBox.value = "";
                    TextBox.focus();
                    return;
                }
                
            }

            
            
        }


</script>


    
    <script>
        $(document).ready(function () {

            var textfieldname = $('#<%=txtKitCode.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=txtKitCode.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadOpenKitCodes") %>',
                        data: "{ 'Prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[0]
                                }
                            }))

                        }

                    });
                },
                select: function (e, i) {

                    $("#<%=this.hifKitCode.ClientID%>").val(i.item.val);

                },
                minLength: 0
            });

            var textfieldname = $('#<%=txtJobOrderRefNo.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtJobOrderRefNo.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadProhRefNoBasedonKitCode") %>',
                        data: "{ 'Prefix': '" + document.getElementById('<%=hifKitCode.ClientID%>').value + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('Please select kit code');
                            }
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

                    $("#<%=hifJobRefNoNumber.ClientID %>").val(i.item.val);

                },
                minLength: 0
            });

            var textfieldname = $('#<%=txtActivityNo.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtActivityNo.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadActivityNos") %>',
                        data: "{ 'prefix': '" + request.term + "','ProHID':'" + document.getElementById('<%=hifJobRefNoNumber.ClientID%>').value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('No activities are found');
                            }
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

                    $("#<%=hifActivityID.ClientID %>").val(i.item.val);

                },
                minLength: 0
            });


            var textfieldname = $('#<%=txtWorkStation.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=txtWorkStation.ClientID%>').autocomplete({
                     source: function (request, response) {

                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWorkCenterForProductionOrder") %>',
                             data: "{ 'prefix': '" + request.term + "','ProductionOrderHeaderID':'" + document.getElementById('<%=this.hifJobRefNoNumber.ClientID%>').value + "'}",
                             dataType: "json",
                             type: "POST",
                             async: true,
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 if (data.d == "") {
                                     alert('No work station for Job Order');
                                 }
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

                         $("#<%=this.hifWorkCenter.ClientID%>").val(i.item.val);

                      },
                      minLength: 0
                 });




        });
    </script> 

    <style type="text/css">

        .ss {
            font-family:Calibri;
        }

        .module_login {
            border: 0px solid #1a79cf;
        }
        a.ButMSG {

    background-image:url('../Images/new_go_icon_30x40.png');
	background-repeat:no-repeat;
    background-position:right;
    background-origin:padding-box;
    background-color:#ffb033;
    font-family:Calibri,Verdana,Geneva,sans-serif;
    text-decoration:none;
	font-size: 15px;
    color:#000;
    font-weight:bold;
    padding-left:10px;padding-right:45px;padding-top:9px;padding-bottom:9px;
    

}
        .txt_Yellow_Small {
       border-radius:3px;
       border:1px solid #848484;
       font-family:Calibri;
       position:relative;
       color: #000000; 
       font-size:13pt;
       width:200px;
    
}

    .txt_Yellow_Small:focus {
        outline: none;
    color: #000000; 
    box-shadow: 0px 0px 5px #ffb033;
    border:1px solid #ffb033;
    border-radius: 4px;
}

    .ui-menu .ui-menu-item a:hover , .ui-widget-content .ui-state-hover, .ui-widget-header .ui-state-hover{
    background-color: #ffb033;
    background-image:none;
    color:white;
    font-size:15px;
    border:none;    
}
    </style>

    






     <table border="0" cellpadding="3" cellspacing="1" align="right" >

               <tr>
                    <td align="left" >
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="getDetails" ControlToValidate="txtKitCode" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" /> 
                        <label style="font-size:15px"> 
                            Kit Code: </label> <br />
                         <asp:TextBox runat="server" ID="txtKitCode" Width="150" SkinID="txt_Hidden_Req_Auto" Font-Size="14px"></asp:TextBox>
                           <asp:HiddenField runat="server" ID="hifKitCode" />
                    </td>
                                 
                    <td align="left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="getDetails" ControlToValidate="txtJobOrderRefNo" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" /> 
                        <label style="font-size:15px"> Job Order Ref. No.: </label> <br />
                         <asp:TextBox runat="server" ID="txtJobOrderRefNo" Width="175" SkinID="txt_Hidden_Req_Auto" Font-Size="14px"></asp:TextBox>
                           <asp:HiddenField runat="server" ID="hifJobRefNoNumber" />
                    </td>
                     <td >
                         <asp:Literal ID="ltStatus" runat="server" ></asp:Literal>
                         <br />
                         <asp:LinkButton  ID="lnkGetDetails" ValidationGroup="getDetails" CssClass="ui-btn ui-button-large"  runat="server" OnClick="lnkGetDetails_Click" >Get Details<%=MRLWMSC21Common.CommonLogic.btnfaFilter %></asp:LinkButton></td>
                </tr>
                    
        </table>
    <br /><br /><br /><br />

    <table border="0" cellpadding="3" cellspacing="3" style="padding-left:15px;" width="100%"> 

        <tr>
            <td colspan="4">
                
    <asp:TreeView ShowLines="true" runat="server" ID="tvBulkNCList" ShowCheckBoxes="All"  NodeIndent="35" >

    </asp:TreeView>
                <br /><br /><br />
            </td>
        </tr>

        <tr >
            <td align="right">
                <table border="0" cellpadding="5" cellspacing="5">
                    <tr>
                        
                        <td class="FormLabels" > <br /> <asp:RequiredFieldValidator ID="rfvtxtRevRemarks" runat="server" ValidationGroup="UpdateRev" ControlToValidate="txtRemarks" Display="Dynamic" ErrorMessage=" * " />  Remarks:<br /> <asp:TextBox runat="server" TextMode="MultiLine"  ID="txtRemarks" Width="450" CssClass="txt_Yellow_Small" EnableTheming="false" Columns="100"></asp:TextBox >  </td>
                        <td class="FormLabels" > <asp:RequiredFieldValidator ID="rfvtxtActivityNo" runat="server" ValidationGroup="UpdateRev" ControlToValidate="txtActivityNo" Display="Dynamic" ErrorMessage=" * " /> Source Activity No.: <br /><asp:TextBox runat="server" ID="txtActivityNo" CssClass="ActivityPicker" SkinID="txt_Hidden_Req_Auto" Width="120"  />  </td>
                        <td class="FormLabels" > <asp:RequiredFieldValidator ID="rfvtxtWorkStation" runat="server" ValidationGroup="UpdateRev" ControlToValidate="txtWorkStation" Display="Dynamic" ErrorMessage=" * " />   Workstation: <br /><asp:TextBox runat="server" ID="txtWorkStation" CssClass="ActivityPicker" SkinID="txt_Hidden_Req_Auto"  />  </td>
                        <td class="FormLabels" >  <br /> <asp:LinkButton runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkTvSubmit_Click" ID="lnkTvSubmit" ValidationGroup="UpdateRev">Submit<%=MRLWMSC21Common.CommonLogic.btnfaRightArrow %></asp:LinkButton>  </td>
                    </tr>
                </table>

            </td>
            </tr>

    </table>

    <asp:Label runat="server" ID="lblCount"></asp:Label>

    <asp:HiddenField runat="server" ID="hifActivityID" />
    <asp:HiddenField runat="server" ID="hifWorkCenter" />

    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />

</asp:Content>
