<%@ Page Title="Material Group" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="MGroupMaster.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.MGroupMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
<%--<link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" /> 
    <script src="../mInventory/CycleCountScripts/jquery.dataTables.min.js"></script>  
    <script src="../mInventory/CycleCountScripts/dataTables.bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
   

    <style>
        .row {
            margin-left:0;
            margin-right:0;
        }
        table {
            border-collapse:inherit !important;
        }
        a {
            box-sizing: initial !important;
        }

        .pagination {
            margin:0px !important;
        }

         .fa-edit, fa-trash {
         cursor:pointer;
         }
          .dataTables_filter, .dataTables_length, .dataTables_info {
         display:inline-block;
         }
        .dataTables_info {
            margin-left:20px;
        }
        .dataTables_filter {
        float:right;
            -webkit-margin-end: 75px;
        }
        .pagewidth {
            margin-bottom:0px !important;
        }
         .stripedhed {
                 background-color: #99c87d;
    text-align: justify;
    color: #333333;
         }

         .even {    background-color: #99c87d36 !important;
         }
         /*.pagination > li > a:hover, .pagination > li > span:hover, .pagination > li > a:focus, .pagination > li > span:focus {

    border-color: #fff !important;
    background-color: var(--sideNav-bg) !important;
    color: #fff;
}*/

        .pagination {
            padding:0;
            float: right;
        }

         .previous:hover{
            background-color: #fff !important;
            color: #0e0e0e !important;
            border-color: #fff !important;
         }

         .trRow {
         cursor:pointer;}
         .tblChild {
         display:none;}

         select {
            font-size:14px !important;
                width: 99% !important;
         }

         .table-striped .text-right {
                padding: 0 !important;
                border: 0;
                box-shadow: none;
         }

         .table-striped tr td {
             font-size:13px !important;
         }

         .sorting{
             width:unset !important;
         }
    </style>
     
    <style>
        .divContainer {
            position:fixed;
            background-color:rgba(0,0,0,0.5);
            top:0;
            bottom:0;
            left:0;
            right:0;
            text-align:center;
            z-index:999;
        }
        .divBody {
           background-color: white;
    width: 50%;
    padding-bottom: 25px;
    padding: 20px;
    border-radius: 5px;
    position: absolute;
    left: calc(50% - 25%);
    top: calc(50% - 82px );
        }
        #spanClose {
           cursor:pointer;
           float:right;
        }


        textarea{
            height: 42px !important;
    width: -webkit-fill-available !important;
        }

        .gap10 {
            height:10px;
        }

    </style>
    <!--preloader block for ajax call-->
        <div class="loader-block" id="preloader" style="display:none">
        <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">
            <div style="align-self: center;">
                <div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="module_yellow">
            <div class="ModuleHeader">
               <div> <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Administration</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Material Group </span></div>
                
           </div>
            
        </div>
   <div class="container">
        <div id="InsertPanel1" class="modal" >
             <div class="modal-dialog" role="document">
             <div class="modal-content" style=" width: 800px; height: 150px;">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
            <h4 class="modal-title" style="display: inline !important;text-align:left"> <%= GetGlobalResourceObject("Resource", "MaterialGroupDetails")%></h4> 

                            </div>
             <br />
            <div>
                <div class="row">
                   <div class=" col m4">
                            <div class="FormLabels flex baseline">
                         
                        </div>
                    </div>
                     <div class=" col m4">
                         <div class=" flex ">

                             </div>
                    </div>
                     <div class=" col m4">
                         <div class="flex">
                            
                             </div>
                    </div>
                   <%-- <div class="col-md-3"><asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click"  />&nbsp;&nbsp;<button id="cancel" type="button" class="btn btn-primary btn-sm"  onclick="cancelUpdate()" >Cancel</button></div>--%>
                    
                </div>
                <div class="row">
                     <div class="col m12" flex end>
                        
                         </div>
                </div>
                
            </div>
             <div><span><asp:HiddenField ID="hifid" Value="0" runat="server" /></span></div>
        </div>
             </div>
    </div>
  
         <div class="modal inmodal" id="InsertPanel" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 50% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                      <h4 class="modal-title" style="display: inline !important;text-align:left"> <%= GetGlobalResourceObject("Resource", "MaterialGroupDetails")%></h4> 
                </div>

                <div class="modal-body">
                 <div class="row">
                    <div class="col m4">
                        <div class="flex">
                               <asp:TextBox ID="txtTenant" runat="server" CssClass="txt_slim" SkinID="txt_Hidden_Req"  />
                        <%--    <label>Tenant</label>--%>
                                    <label> <%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                  <span class="errorMsg"></span>
                            <asp:HiddenField ID="hifTenant" runat="server" Value="0" />
                        </div>
                    </div>
                    <div class="col m4">
                        <div class="flex">
                   <asp:TextBox ID="txtGroupCode"  runat="server" CssClass="txt_slim" SkinID="txt_Hidden_Req" MaxLength="10"  ></asp:TextBox>
                             <%-- <label>  Group Code </label>--%>
                              <label> <%= GetGlobalResourceObject("Resource", "GroupCode")%> </label>
                             <span class="errorMsg"> </span>
                        </div>
                    </div>
                    <div class="col m4">
                        <div class="flex">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="txt_slim"  SkinID="txt_Hidden_Req"  TextMode="MultiLine" ></asp:TextBox>                               
                            <%--<label>Description </label>--%>
                             <label> <%= GetGlobalResourceObject("Resource", "Description")%></label>
                              <span class="errorMsg"></span>
                        </div>
                    </div>

                </div>
                </div>
                <div class="modal-footer">
                   <button id="cancel" type="button" data-dismiss="modal" class="btn btn-primary btn-sm"  onclick="cancelUpdate()" ><%= GetGlobalResourceObject("Resource", "Cancel")%></button>&nbsp;&nbsp;
                         <asp:Button ID="btnSave" type="button" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                   <%-- <button type="button" ID="btnSave" OnClick="btnSave_Click()" class="btn btn-primary" runat="server">Save</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div>
        <div class="row">
            <div class="col m2 s3 offset-m9 offset-s8">
                <div class="flex">
                    <input type="text" ng-model="search"  id="myInput" >
                  <%--  <label class="lblFormItem">Search</label>--%>
                      <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "Search")%></label>
                </div>
            </div>
            <div class="col m1 s1">
                <gap5></gap5>
                <asp:Label ID="lblMsg" Text="" runat="server"></asp:Label>
                <button id="btnAddMaterial" type="button" class=" btn btn-primary btn-sm" onclick="editMaterial(0);" style="float: right">Add <i class="material-icons vl">add</i></button>
                <!-- <button  type="button" id="btnevent" onclick="editMaterial(0);" class="btn btn-primary" >Add <i class="material-icons">add</i></button>-->
            </div>

        </div>
       </div>
          
        
    <div id="divContent">

       <%-- <table id="tbMaterials" class="table table-striped">
            
        </table>--%>
    </div></div>
       </div> 
    <br />
    <br />
    <br />

    <script >
        $('input').keypress(function( e ) {
    if(e.which === 32) 
        return false;
});
    </script>

    <script>




        //======================= Added By M.D.Prasad For View Only Condition ======================//
        var UserRoleDataID = "";
        var role = '';
        debugger;
        role = '<%=UserRoledat%>';
        role = role.substring(0, role.length - 1);
        role = role.split(',');
        for (var i = 0; i < role.length; i++) {
            if ('<%=UserTypeID%>' == '3' && role[i] == '4') {
                 UserRoleDataID = role[i];
                 $("#btnAddMaterial").css("display", "none");
             }
         }
         //======================= Added By M.D.Prasad For View Only Condition ======================//

        function openModal() {
            debugger;
            $('#InsertPanel').modal('show');
        }

         $(document).ready(function () {
             $("#myInput").on("keyup", function () {
                 var value = $(this).val().toLowerCase();
                 $("#tbMaterials tbody tr").filter(function () {
                     $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                 });
             });
             displayMaterial();
             var textfieldname = $("#<%= this.txtTenant.ClientID %>");
                        DropdownFunction(textfieldname);
                        $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                       // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH', // Added By Ganesh @Sep 28-2020 --Tenant Drop down data should be displyed by UserWh                             
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
            //$("#btnShow").click(function () {
            //    displayMaterial();
            //});

            $('#spanClose').click(function () {
                cancelUpdate();
            });
         });
        function displayMaterial() {
            //$("#preloader").show();
             $.ajax({
                 type: "POST",
                 url: "MGroupMaster.aspx/GetMaterials",
                 contentType: "application/json",
                 dataType: "JSON",
                 success: function (response) {
                     //alert("Success method");
                     debugger;
                   // $("#preloader").hide();
                     var table = $("#divContent");
                     table.empty();
                     var data = "<table id='tbMaterials' class=' table-striped'><thead><tr><th>SNO:</th><th>Material Group</th><th>Description</th><th>Tenant Name</th>";
                     data += "<th>";
                     if (UserRoleDataID != "4") {
                         data += "Action";
                     }
                     data += "</th>";
                     data += "</tr ></thead><tbody id='tdbody'>";
                      if (response.d.length == 0) {
                         data += " <tr><td colspan='9'><div align='center' style='font-size:13px'>No data Found. </div> </td></tr></tbody> </table>"
                          $("#divContent").html(data);
                          return false;
                     }
                     //table.html(tbheader);
                     for (i = 0; i < response.d.length; i++) {
                         data += "<tr><td>" + (i + 1) + "</td>";
                         data += "<td>" + response.d[i].MaterialGroup + "</td>";
                         data += "<td>" + response.d[i].Description + "</td>";
                         data += "<td>" + response.d[i].TenantName + "</td>";
                         data += "<td>";
                         if (UserRoleDataID != "4") {
                             data += "<a onClick='editMaterial(" + response.d[i].MGroupId + ");' ><i class='material-icons ss'>mode_edit</i></a>";
                             data += "&nbsp;&nbsp;<a onClick='deleteMaterial(" + response.d[i].MGroupId + ");' ><i class='material-icons ss'>delete</i></a>";
                         }
                         data += "</td></tr>";
                         //table.append(txt);
                     }
                     data += '</tbody> </table>';

                    
                     $("#divContent").html(data);
                    // SetTableSettings();
                 }
             });
        }




//        function myFunction() {
//            debugger;
//  var input, filter, table, tr, td, i,row;
//  input = document.getElementById("myInput");
//  filter = input.value.toUpperCase();
//  table = document.getElementById("tbMaterials");
//    tr = table.getElementsByTagName("tr");
//            for (j = 0; j < 4; j++) {
//                for (i = 0; i < tr.length; i++) {
//                    row = document.getElementById("tbMaterials").rows[i];


//                    td = tr[i].getElementsByTagName("td")[j];

//                    if (td) {
//                        if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
//                            tr[i].style.display = "";
//                        } else {
//                            tr[i].style.display = "none";
//                        }
//                    }
//                }
//            }
//}

         function editMaterial(id) {
             debugger;
            // $("#InsertPanel").fadeIn();
              $("#InsertPanel").modal({
                         show: 'true'
                        });
             $("#<%=hifid.ClientID%>").val(id);
                        if (id != 0) {
                            var obj = { "Id": id };
                            $.ajax({
                                type: "POST",
                                url: "MGroupMaster.aspx/EditMaterials",
                                dataType: "JSON",
                                contentType: "application/json",
                                data: JSON.stringify(obj),
                                success: function (response) {
                                    // alert("success");
                                    $("#<%=this.txtTenant.ClientID%>").val(response.d[0].TenantName);
                         $("#<%=this.hifTenant.ClientID%>").val(response.d[0].TenantId);
                         $("#<%=this.txtGroupCode.ClientID%>").val(response.d[0].MaterialGroup);
                         $("#<%=this.txtDescription.ClientID%>").val(response.d[0].Description);
                         $("#<%=this.btnSave.ClientID%>").val("Update");
                        
                     }
                 });

             }
             else {
                 $("#<%=this.txtTenant.ClientID%>").val('');
                 $("#<%=this.hifTenant.ClientID%>").val(0);
                 $("#<%=this.txtGroupCode.ClientID%>").val('');
                 $("#<%=this.txtDescription.ClientID%>").val('');
                  $("#<%=this.btnSave.ClientID%>").val("Save");
                        }
                    }
        function deleteMaterial(id) {
            debugger;
                        if (window.confirm("Do you want to Delete?")) {
                            var obj = { "Id": id };
                            $.ajax({
                                type: "POST",
                                url: "MGroupMaster.aspx/DeleteMaterial",
                                dataType: "JSON",
                                contentType: "application/json",
                                data: JSON.stringify(obj),
                                success: function (response) {
                                    if (response.d == "Exist") {
                                        showStickyToast(false, "Cannot delete Material Group as item is mapped to this.");
                                    }
                                    else {
                                        displayMaterial();
                                        showStickyToast(true, response.d);
                                    }
                                }
                            });
                            displayMaterial();
                        }
                    }


                    function SetTableSettings() {

                        //  $('#tbMaterials').destroy();

                        $('#tbMaterials').DataTable({
                            pageLength: 25,
                            dom: '<"html5buttons"B>lTfgitp',

                            buttons: [
                                { extend: 'copy' },
                                { extend: 'csv' },
                                { extend: 'excel', title: 'ExampleFile' },
                                { extend: 'pdf', title: 'ExampleFile' },

                                {
                                    extend: 'print',
                                    customize: function (win) {
                                        $(win.document.body).addClass('white-bg');
                                        $(win.document.body).css('font-size', '10px');

                                        $(win.document.body).find('table')
                                            .addClass('compact')
                                            .css('font-size', 'inherit');
                                    }
                                }
                            ]

                        });
                    }

                    function cancelUpdate() {

                        $("#<%=this.txtTenant.ClientID%>").val("");
             $("#<%=this.txtGroupCode.ClientID%>").val("");
             $("#<%=this.txtDescription.ClientID%>").val("");
             $("#<%=this.btnSave.ClientID%>").val("Save");
          //  $("#InsertPanel").fadeOut();
         }
    </script>
</asp:Content>
