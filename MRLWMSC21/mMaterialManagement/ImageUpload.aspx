<%@ Page Title="" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="ImageUpload.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.ImageUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
      <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
       <script type="text/javascript">
           function checkFileExtension(elem) {
               var filePath = elem.value;

               if (filePath.indexOf('.') == -1)
                   return false;

               var validExtensions = new Array();
               var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

               validExtensions[0] = 'png';
               //validExtensions[1] = 'xlsx';


               for (var i = 0; i < validExtensions.length; i++) {
                   if (ext == validExtensions[i])
                       return true;
               }

               elem.value = "";
               alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
               return false;
           }
    </script>
    <div class="dashed"></div>
    
    <div style="height: 100%;" class="pagewidth">
        <br />
        <table border="0" style="width: 38%; height: 100%;">
            <tr>
                <td> 
                    <b>Logo</b> <br />
                    <asp:FileUpload runat="server" ID="flupldImage" onchange="return checkFileExtension(this);" />
                </td>
                <td><br />
                    <asp:LinkButton runat="server" ID="lnkflupldImage" Text="Import Logo" OnClick="lnkflupldImage_Click" CssClass="ui-btn ui-button-large"></asp:LinkButton>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
