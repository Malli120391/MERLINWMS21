<%@ Page Language="C#" Title="Login" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MRLWMSC21.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MRLWMSC21™ - receive .. manage .. deliver.. </title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script>
        window.onload = function () {
            if ($.browser.webkit) {
                $('input[name="txtEmailID"]').attr('autocomplete', 'off');
                $('input[name="txtPassword"]').attr('autocomplete', 'off');
            }
        };

    </script>

    <style>
        

/* fallback */

@font-face {
    font-family: 'Material Icons';
    font-style: normal;
    font-weight: 400;
    src: url('Fonts/flUhRq6tzZclQEJ-Vdg-IuiaDsNc.woff2') format('woff2');
}

.material-icons {
    font-family: 'Material Icons';
    font-weight: normal;
    font-style: normal;
    font-size: 24px;
    line-height: 1;
    letter-spacing: normal;
    text-transform: none;
    display: inline-block;
    white-space: nowrap;
    word-wrap: normal;
    direction: ltr;
    -webkit-font-feature-settings: 'liga';
    -webkit-font-smoothing: antialiased;
}




@font-face {
    font-family: "Roboto";
    src: local(Roboto Thin), url("Fonts/Roboto-Thin.woff2") format("woff2"), url("Fonts/Roboto-Thin.woff") format("woff");
    font-weight: 100;
}

@font-face {
    font-family: "Roboto";
    src: local(Roboto Light), url("Fonts/Roboto-Light.woff2") format("woff2"), url("Fonts/Roboto-Light.woff") format("woff");
    font-weight: 300;
}

@font-face {
    font-family: "Roboto";
    src: local(Roboto Regular), url("Fonts/Roboto-Regular.woff2") format("woff2"), url("Fonts/Roboto-Regular.woff") format("woff");
    font-weight: 400;
}

@font-face {
    font-family: "Roboto";
    src: local(Roboto Medium), url("Fonts/Roboto-Medium.woff2") format("woff2"), url("Fonts/Roboto-Medium.woff") format("woff");
    font-weight: 500;
}

@font-face {
    font-family: "Roboto";
    src: local(Roboto Bold), url("Fonts/Roboto-Bold.woff2") format("woff2"), url("Fonts/Roboto-Bold.woff") format("woff");
    font-weight: 700;
}


        body {
            background:#fff !important;
           font-family: "Roboto";

        }

        .MenuBG,.Header,.MobileMenu {
            display:none;
            background-color:white;
        }
        .log-in {
            color: #6c6763;
            padding-right: 2px;
        }
        .cssinv {
            position:absolute;
            top: 16px;
            right: 26px;
        }
        .sign-up-inv {
            position:absolute;
            color: #e85b13;
            padding-left: 14px;
            padding-top:12px;
            font-weight:bold;
            font-size: 19px;

        }
        #Capa_1 {
    top: 65%;
}

        .sign-up {
            color: #e85b13;
            padding-left: 14px;
            font-weight:bold;
            font-size: 19px;
        }
        .txt_Blue_Small_login {
            font-size: 15px;
            font-weight: 400;
            display: block;
            width: 85%;
            padding: 5px;
            margin-bottom: 5px;
            border-radius: 5px;
            -webkit-transition: all 0.3s ease-out;
            -moz-transition: all 0.3s ease-out;
            -ms-transition: all 0.3s ease-out;
            -o-transition: all 0.3s ease-out;
            transition: all 0.3s ease-out;
            height:25px;
            border: 3px solid #ebe6e2;
            background-color:white;
        }
        .txt_Blue_Small_login:hover,txt_Blue_Small_login::selection {
            border-color: #CCC;
            outline:none;
            box-shadow: 0px 0px 0px #ccc;

            /*border: 3px solid #ebe6e2;*/
        }
            
        .main {
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0px 0px 40px rgba(0, 0, 0, 0.1);
            width: 370px;
            max-width: 450px;
            margin: 0px auto;
            align-self:center;
        }

        .m_div {
                    padding: 15px;
        }

        table[width="1054"] {
            box-shadow: unset !important;
            width: 80vw;
            height: 90vh;
        }

        .footer {
            display: none !important;
        }

        .btn-primary {
    color: #fff !important;
    background-color: #2a328a;
    border-color: transparent;
    background: #2a328a;
    border: 0px;
        }

.btn {
    display: inline-block;
    font-weight: 400;
    text-align: center;
    white-space: nowrap;
    vertical-align: middle;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    border: 1px solid transparent;
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    line-height: 1.5;
    border-radius: 0.25rem;
    transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12), 0 3px 1px -2px rgba(0, 0, 0, 0.2);
    transition: all 0.8s;
    font-size: 14px !important;
    font-family: sans-serif;
    width: 115px;
    text-transform: uppercase;
    border-radius: 2px !important;
}

        .errorMsg {
            color: #FF0000;
            font-size: 12px;
            float: left;
        }        
        
        .styling {
        box-shadow: none;
            border-radius: 0px;
            /* background-image: url(Images/bg-pattern2.png) !important; */
            background-image: url(Images/banner-bg3e7818e040f1fb3244fc67d5d4b29ab6.png) !important;
            background-size: contain;
            background-repeat: repeat;
            background-repeat: no-repeat;
            background-position: 0 0;
            width: 100vw;
            height: 100vh;
            margin: auto;
            display: flex;
            justify-content: center;
            background-size: cover;
        }
        .styling:before {
content: "";
    position: absolute;
    z-index: -1;
    background: -webkit-linear-gradient(top left, #D7BBEA, #65A8F1);
    background: linear-gradient(to bottom right, #D7BBEA, #65A8F1);
    background: linear-gradient(to bottom right, #03A9F4, #20909e);
    left: 0;
    top: 0;
    bottom: 0;
    right: 0;
        background: #29328b;
        }
        .module_login {
            background:transparent;
        }

        .fixedd {
            position: fixed;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
            display: flex;
        }

        .c-form {
            display: block;
            width: 90%;
            padding: 0.5rem 0.7rem;
            font-size: 0.9rem;
            line-height: 1.25;
            color: #464a4c;
            background-color: #fff;
            background-image: none;
            background-clip: padding-box;
            border: 1px solid #cecece;
            border-radius: 0.25rem;
            -webkit-transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
            transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
            margin-bottom: 20px;
        }

        .FormLabels {
            color: #000 !important;
    font-size: 14px !important;
    font-weight: normal !important;
        }
        input[type="text"] {
            height:initial;
        }
        .flex-center {
            display: flex;
            justify-content: space-between;
        }

        .flex-center div{
            align-self: center;
        }

        #MainContent_btnLogin {
    background-image: linear-gradient(80deg, #00aeff, #3369e7);
    box-shadow: 0 2px 6px 0 rgba(5,15,44,0.5) !important;
    color: #fff !important;
        }
        .svgsize {
            position:relative;
        }
        .svgsize svg{
            width: 22px;
            height: 22px;
            position: absolute;
            /* top: calc(50% - 11px); */
            top: 54%;
            right: 15px;
        }


    .flex input[type="text"], input[type="number"], textarea {
        font-size: 14px;
        padding: 10px 5px 10px 0px !important;
        display: block;
        border: none !important;
        border-bottom: 1.3px solid #b3adad !important;
        outline: none !important;
        margin: 5px 0px;
        color: #0e0e0e;
        font-size: 14px;
            border-radius: 0px;
        background-color: #fff !important;
            width: 97%;
    }

        .flex input[type="password"], input[type="password"], textarea {
        font-size: 14px;
        padding: 10px 5px 10px 0px !important;
        display: block;
        border: none !important;
        border-bottom: 1.3px solid #b3adad !important;
        outline: none !important;
        margin: 5px 0px;
        color: #0e0e0e;
            border-radius: 0px;
        font-size: 14px;
        background-color: #fff !important;
            width: 97%;
    }
        .flex {
            position: relative;
        }

    .flex label {
        color: #6b6868;
        font-size: 14px;
        font-weight: normal;
        position: absolute;
        pointer-events: none;
        left: 1px;
        top: 10px;
        transition: 0.2s ease all;
        -moz-transition: 0.2s ease all;
        -webkit-transition: 0.2s ease all;
        background:transparent;
    }

    .flex input[type="text"]:focus ~ label, input[type="text"]:valid ~ label {
    top: -17px;
    font-size: 13px;
   
    color: #2a328a;
    }

            .flex input:focus ~ .cmnn, input:valid ~ .cmnn{
                
    color: #2a328a !important;
            }

.flex input[type="password"]:focus ~ label, input[type="password"]:valid ~ label {
    top: -17px;
    font-size: 13px;
    color: var(--sideNav-bg);
    color: #2a328a;
}

        input:-webkit-autofill ~ label{
                top: -17px;
    font-size: 13px;
    color: var(--sideNav-bg);
    color: #2a328a;
        }

        input:-webkit-autofill ~ .cmnn {
            color: #2a328a;
        }

.checkbox {
    position: relative;
    display: inline;
    user-select:none;
}

    .checkbox label {
        cursor: pointer;
    font-size: 14px;
    font-family: "Roboto";
    }

        .checkbox label:before, .checkbox label:after {
            content: "";
            position: absolute;
            left: 0;
            top: 0;
        }

        .checkbox label:before {
            width: 16px;
            height: 16px;
            background: #fff;
            border: 2px solid rgba(0, 0, 0, 0.54);
            border-radius: 3px;
            cursor: pointer;
            transition: background 0.3s;
            left: 0px;
        }

    .checkbox input[type="checkbox"] {
        outline: 0;
        margin-right: 10px;
        position: absolute;
    }

        .checkbox input[type="checkbox"]:checked + label:before {
    background: #2a328a;
    border-color: #2a328a;
        }

        .checkbox input[type="checkbox"]:checked + label:after {
            transform: rotate(-45deg) !important;
            top: 4px;
            left: 4px;
            width: 10px;
            height: 5px;
            border: 2px solid #fff;
            border-top-style: none;
            border-right-style: none;
        }

        .checkbox input[type="checkbox"] {
            visibility: hidden;
        }

        .cmnn {
            position: absolute;
            top: 10px;
            right: 0;
        }

        #lblError {
                font-weight: normal;
    font-size: 12px;
        }

        .right-fotter {
    width: 28%;
    float: right;
    text-align: right;
    height: 100%;
}

        #footer12 {
    width: 20px;
    position: relative;
    top: 5px;
}

        @media (max-width:500px) {
            .main {
                width:300px;
            }
        }

        .forgetPassword {
                border: 0;
                cursor:pointer;
    background: transparent;
    outline:0px;
        }

        .after-shadow {
        position:relative;
        }

        .after-shadow:after{
            content: "";
            position: absolute;
            left: 0;
            right: 0;
            bottom: -10px;
            margin: auto;
            width: 90px;
            height: 6px;
            background-color: rgba(0, 0, 0, 0.1);
            -webkit-filter: blur(2px);
            filter: blur(2px);
            -moz-border-radius: 100%;
            -webkit-border-radius: 100%;
            border-radius: 100%;
            z-index: 1;
            -moz-animation: shadow 0.5s ease infinite alternate;
            -webkit-animation: shadow 0.5s ease infinite alternate;
            animation: shadow 0.5s ease infinite alternate;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sManagerLogin" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>


        <div>
            <div class="fixedd">
    <div class="styling">
                
            <div class="main">
                        <div style="text-align:center; padding-bottom" class="after-shadow">
                            <br /><br />
                           <%--  <img style="  width: 190px; margin-bottom: 15px; "  src="data:image/png;base64,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" alt="Alternate Text" />--%>
                           <img style="width: 210px; margin-bottom: 15px;" src="data:image/png;base64,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" alt="Alternate Text" /> 
                            
                               
                            <h3 style=" margin-top: 0; font-size: 18px;  margin-bottom: 0px;">LOGIN</h3> </div>
                <div>
                    <br />
                   
            <asp:Panel runat="server" ID="pnlLogin" DefaultButton="btnLogin">
                <div class="m_div"> 
                   
                    <div>
                
                  

                        <div class="flex">
                            <asp:TextBox runat="server" ClientIDMode="Static" autocomplete="off" EnableTheming="false" CssClass="c-form" ID="txtEmailID" required=""  />
                            <i class="material-icons cmnn">perm_identity</i>
                           <%-- <label><asp:Label ID="Label1" runat="server" CssClass="" >User Name or Email</asp:Label></label>--%>
                             <label><asp:Label ID="Label1" runat="server" CssClass="" >Email</asp:Label></label>
                            <asp:RequiredFieldValidator ID="rfvtxtEmailID" runat="server"  ControlToValidate="txtEmailID" CssClass="errorMsg" Display="Dynamic" BorderColor="Red" ErrorMessage="Required" ValidationGroup="valLogin" />                            
                        </div>
                        <br />

                        <div class="flex">
                            <asp:TextBox runat="server" ID="txtPassword" ClientIDMode="Static" autocomplete="off" EnableTheming="false" CssClass="c-form inptn" TextMode="Password" required="" />
                            <i class="material-icons cmnn">fingerprint</i>
                            <label><asp:Label ID="Label2" runat="server" CssClass="">Password </asp:Label></label>
                            <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ControlToValidate="txtPassword" CssClass="errorMsg" Display="Dynamic" BorderColor="Red" ErrorMessage="Required" ValidationGroup="valLogin" />
                        </div>
                        
                        <div class="flex__ " style="display:flex; justify-content:flex-end; margin:10px 0;">
                           <%-- <input type="submit" class="forgetPassword" value="Forgot Password ?" onclick="alert()"/>--%>
<%--                            <a href="./ForgotPassword.aspx" style="text-decoration: none; font-size: 14px;color: #080808cc;">Forgot Password ?</a>--%>
                           <%-- <asp:LinkButton ID="lnkForgotPassword" runat="server"   OnClientClick="window.location.href='~/ForgotPassword.aspx'; return false;">Forgot Password </asp:LinkButton>--%>
                            <asp:HyperLink runat="server" ID="lnkForgotPassword"  NavigateUrl="~/ForgotPassword.aspx"> Forgot Password </asp:HyperLink>
                            
                        </div>
                    </div>

                    <div class="flex-center">
                        <div class="checkbox" style="display:none"><asp:CheckBox runat="server" ID="chkRemember"/><label for="chkRemember">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Remember Me</label> </div>
                        <br /> <br />  <br />
                        <div><asp:LinkButton CssClass="btn btn-primary" runat="server" ID="btnLogin"  OnClick="btnLogin_Click"  Font-Underline="false"  ValidationGroup="valLogin" >Login</asp:LinkButton></div>
                    </div>
                </div>    

               
                        <div class="svgsize" style="text-align: center;padding-bottom:5px;">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="false" ></asp:Label>
                        </div>
                       
            </asp:Panel>
                </div>
                </div>
    </div>

 </div>
                               
        </div>


    </form>

    <script>

</script>
</body>
</html>
