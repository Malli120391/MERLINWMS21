<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="MRLWMSC21.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            padding: 15px;
            width: 340px;
            max-width: 450px;
            margin: 0px auto;
            align-self:center;
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
            float: right;
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
        padding: 10px 5px 10px 5px !important;
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
        padding: 10px 5px 10px 5px !important;
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

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sManagerLogin" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>


        <div>
            <div class="fixedd">
    <div class="styling">
                
            <div class="main">
            <asp:Panel runat="server" ID="pnlLogin" >
                <div > 
                   
                    <div>
                        <div class="svgsize" style="    text-align: center;">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="false" ></asp:Label>
                        </div>
                        <div style="text-align:center"><img src="data:image/png;base64,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">
                            <h3 style="    margin-top: 0;">Forgot Password</h3> </div>
                        <br />

                        <div class="flex">
                            <asp:TextBox runat="server" ClientIDMode="Static" autocomplete="off" EnableTheming="false" CssClass="c-form" ID="txtEmailID" required=""  />
                            <i class="material-icons cmnn">perm_identity</i>
                            <label><asp:Label ID="Label1" runat="server" CssClass="" >Email</asp:Label></label>
                            <asp:RequiredFieldValidator ID="rfvtxtEmailID" runat="server"  ControlToValidate="txtEmailID" CssClass="errorMsg" Display="Dynamic" BorderColor="Red" ErrorMessage="Required" ValidationGroup="valLogin" />                            
                        </div>
                        <br />

                        <div class="flex">
                            <asp:TextBox ID="txtCaptcha" runat="server" required="" Visible="false"></asp:TextBox>
                            <label style="display:none;">Type Code Shown</label>
                             <asp:Image ID="Image2" runat="server" Height="55px" ImageUrl="~/Captcha.aspx" Width="186px" Visible="false" />  
                            <asp:LinkButton ID="lnkRefresh" runat="server" PostBackUrl="~/ForgotPassword.aspx" Visible="false">Refresh</asp:LinkButton>
                        </div>
                        <br />

                       
                    </div>

                    <div class="flex" style="float:right !important;">
                        <%--div class="checkbox"><asp:CheckBox runat="server" ID="chkRemember"/><label for="chkRemember">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Remember Me</label> </div>
                        <div><asp:LinkButton CssClass="btn btn-primary" runat="server" ID="btnLogin"    Font-Underline="false"  ValidationGroup="valLogin" >Login</asp:LinkButton></div>--%>
                        <asp:LinkButton ID="lnkSubmit" runat="server" OnClick="lnkSubmit_Click" CssClass="btn btn-primary" Font-Underline="false">Submit</asp:LinkButton>
                        <br />
                        <asp:Label runat="server" ID="lblCaptchaMessage"></asp:Label>  
                    </div>
                </div>    


            </asp:Panel>
                </div>
    </div>

 </div>
                               
        </div>


    </form>

    <script>

    </script>
</body>
</html>
