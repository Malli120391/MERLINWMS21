<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="MRLWMSC21.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <script type="text/javascript">


		function CallServerMethod() {
		    alert("<%= LinkButton1_Click1() %>");
		}


        $(document).ready(function () {
            
            $("#<%= this.txtFreightCompany.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/FreightCompanys") %>',
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
                           alert(response.responseText);
                       },
                       failure: function (response) {
                           alert(response.responseText);
                       }
                   });
               },
                 select: function (e, i) {
                     $("#<%=hifFreightCompany.ClientID %>").val(i.item.val);
                },
                minLength: 0
             });

        });

        
    </script>


     <script type="text/javascript">
        
         $(document).ready(function () {
             $("#<%= this.txtTestSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCode") %>',
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
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hftxtSearch.ClientID %>").val(i.item.val);
                },
                minLength: 1
            });
        });
    </script>



    <div>

          <br />

        test area <br />
    <asp:RequiredFieldValidator runat="server" ID="rfvSearchMCode" ControlToValidate="txtTestSearch" Display="Dynamic" InitialValue="" ValidationGroup="sss" ErrorMessage="*" ></asp:RequiredFieldValidator>
         <asp:TextBox ID="txtTestSearch" runat="server" ValidateRequestMode="Enabled" ValidationGroup="sss" ></asp:TextBox>
         
        <asp:HiddenField ID="hftxtSearch" runat="server" />

         <asp:TextBox ID="txtTestSearch1" runat="server" ></asp:TextBox>



    <br />
        <asp:Label ID="lblTest" runat="server" ></asp:Label>
    <br />
        <asp:LinkButton runat="server" ID="lnkLoginClick" OnClick="LinkButton1_Click" Text="click here"></asp:LinkButton>

    </div>


    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>Your app description page.</h2>
    </hgroup>

    <article>

        <asp:TextBox runat="server" ID="txtFreightCompany"></asp:TextBox>
        <asp:HiddenField runat="server" ID="hifFreightCompany" />

        <input type="button" id="btnTest" value="Click me" />
        <p>   
            <asp:Label runat="server" ID="lblTestService"></asp:Label>     
            Use this area to provide additional information.
        </p>

        <p>        
            Use this area to provide additional information.
        </p>

        <p>        
            Use this area to provide additional information.
        </p>
        
        <br />

        <input type="text" class="txt_Blue_Req"  iD="txtDate" />
    </article>

    <aside>
        <h3>Aside Title</h3>
        <p>        
            Use this area to provide additional information.
        </p>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About">About</a></li>
            <li><a runat="server" href="~/Contact">Contact</a></li>
        </ul>
    </aside>
    <br />
    <div id="popUp" style="width:500px;height:30px;background-color:#ff6a00"> 
        Naresh Inventrax............
    </div>

    <asp:TextBox ID="txtEmail" runat="server" OnTextChanged="LinkButton1_Click"></asp:TextBox>
    <br />
    <asp:Button ID="lnkTestBtn" runat="server" Text="Click Me"  />

    <button id="toggle4">Modal dialog #4</button>

    <div id="dialog4" Title="Dialog #4 (modal)">

            <p>Another sample of modal dialogs - login forms. The dialog using 'highlight/scale' methods to 'show/hide'. Can be moved and closed with the 'x' icon.</p>


                <label for="name">Name</label>

                <input type="text" name="name" id="name" /><br />

                <label for="password">Password</label>

                <input type="password" name="password" id="password" value="" />
        </div>
    <br />
    <asp:DropDownList runat="server" SkinID="ddlBlueSmall">
        <asp:ListItem  Text="SSt" Value="0"> </asp:ListItem>
    </asp:DropDownList>

    <asp:TextBox  ID="txtTestOnClick" runat="server" ></asp:TextBox>
    <br />
    <br />
   
 
    <br />

    <br />

    <asp:GridView ID="gvReturnsForm" runat="server" CssClass="gvBlue" 
                   
                     Width="100%"
                     GridLines="None"
                     ShowFooter="true"
                     AutoGenerateColumns="False"
                     PagerStyle-HorizontalAlign="Right"
                     CellPadding="5"
                     CellSpacing="2">
       
                 <Columns>
                    <asp:TemplateField HeaderText="Sr.No." >
                
                    <ItemTemplate>
                               <asp:Literal ID="ltLineNumber"  runat="server" Text="Line Number" />
                               
                    </ItemTemplate>
       
                   
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Material Code"   >
                    <ItemTemplate>
                               <asp:Literal ID="ltLineNumber"  runat="server" Text="Line Number" />
                               
                    </ItemTemplate>
               </asp:TemplateField>
               
               <asp:TemplateField HeaderText="Description"   >
                     <ItemTemplate>
                               <asp:Literal ID="ltLineNumber"  runat="server" Text="Line Number" />
                               
                    </ItemTemplate>
               </asp:TemplateField>
                                        
                 </Columns>
       
                  <FooterStyle CssClass="gvBlue_footerGrid" />
                  <RowStyle CssClass="gvBlue_DataCellGrid" />
                  <EditRowStyle CssClass="gvBlue_DataCellGridEdit" />
                  <PagerStyle CssClass="gvBlue_pager" />
                  <HeaderStyle CssClass="gvBlue_headerGrid" />
                  <AlternatingRowStyle CssClass="gvBlue_DataCellGridAlt" />
                                            
       </asp:GridView>
              
    <br />

    <h1><asp:Label runat="server" ID="lblServiceDetails"></asp:Label></h1>





</asp:Content>