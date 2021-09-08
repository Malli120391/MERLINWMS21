<%@ Page Title="" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="SkipLog.aspx.cs" Inherits="MRLWMSC21.mReports.OBDSkipLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="SkipLog.js"></script>

      <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });
            $("#txtToDate").datepicker(
                {
                    dateFormat: "dd-M-yy",
                    maxDate: new Date()
                });         
        });
    
    </script>

     <style>
        /* Absolute Center Spinner */
        .loading {
            position: fixed;
            z-index: 9999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            .loading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>

    <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Reports</span> <i class="material-icons">arrow_right</i><span>Audit</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Skip Log Report </span></div>
                
            </div>

        </div>
    <div class="container" ng-app="OBDSkipApp" ng-controller="SkipItem">
         <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                  <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>

        <div style="">
            <div class="row">
             
                <div class="col s3 m2 ">
                    <div class="flex">
                        <div>
                           
                            <input type="text" id="txtTenant"  required="" />
                           <label>Tenant</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
            

                <div class="col s3 m2">
                    <div class="flex">
                        <div>
                            <%--<input type="text" id="txtType"   required=""/>--%>
                            <select ng-model="type" ng-change="ClearList()" required="">
                                <option value="" selected="selected">Select Skip Type</option>
                                <option value="inb">Putaway</option>
                                <option value="obd">Outbound</option>
                            </select>
                           <label style="top: -5px;">SkipLog Type</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>


              <%--  <div class="col s3 m2">
                    <div class="gap10"></div>
                    <div class="">
                      
                        <div class="flex__ end">
                        </div>
                    </div>
                </div>--%>

                 <div class="col s3 m2 ">
                    <div class="flex">
                        <div>
                           
                            <input type="text" id="txtPartnumber"  required=""  />
                           <label>Part No.</label>

                        </div>
                    </div>
                </div>
           <%--     <div class="col s3 m2">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtlocation"   required=""/>
                           <label>Location</label>
                            <input  type="hidden" id="hdfLocation" />
                        </div>
                    </div>
                </div>--%>
                 


                    <div class="col s3 m2">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtFromDate"   required=""/>
                           <label>From Date</label>
                        </div>
                    </div>
                </div>

                <div class="col s3 m2">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtToDate"   required=""/>
                           <label>ToDate</label>
                        </div>
                    </div>
                </div>
            </div>







<div class="row">
              <div class="flex__ end">
                <div class="flex " style="margin-right: 1% !important">
                    <button type="button" ng-click="GetSkipData()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                     <button type='button' class="btn btn-primary" ng-click="downloadExcel()">Export <i class="material-icons">cloud_download</i></button>
                </div>
            </div>
        </div>

            <div  >
             <table  class="table-striped">
                    <thead>
                        <tr>
                           <th><input ng-model="SelectALL" ng-change="ChangeSelectALL()" type="checkbox" /></th>
                            <th>User</th>
                            <th>Reference Number</th>                                                
                             <th>SKU</th>
                            <th>Description</th>
                            <th>Location</th>
                            <th>Skip Qty.</th>
                            <th>Created On</th>           
                        </tr>
                    </thead>

                    <tbody>                       
                        <tr dir-paginate="cc in Skipdata  |itemsPerPage:25" pagination-id="main">
                             <td><input type="checkbox" ng-model="cc.Isselected" /></td>
                            <td>{{cc.FirstName + cc.LastName}}</td>
                            <td>{{cc.RefNo}}</td>                                               
                             <td>{{cc.Mcode}}</td>
                            <td>{{cc.MDescription}}</td>
                             <td>{{cc.Location}}</td>
                             <td>{{cc.SkipQuantity}}</td>
                            <td>{{cc.CreatedOn}}</td>
                            
                        </tr>
                    </tbody>
                 <tfoot ng-show="Skipdata.length==0">
                        <tr><td></td><td></td><td></td><td></td>                                                     
                            <td>
                                <div class="text-center" style="font-size: 13px !important;">No data Found. </div>
                            </td>
                           <td></td><td></td><td></td>
                        </tr>
                    </tfoot>
                </table>
         <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
        </div>
       <br />

         <div class="flex__ end" ng-show="Skipdata!=undefined && Skipdata!=null && Skipdata.length!=0 ">
            <div class="flex " style="margin-right:1% !important">
                <img src="../Images/bx_loader.gif" id="imgLLoadingSAP" style="width:60px;display:none;" />
                <button type="button" id="btnsave" type="button" id="SaveInfo"  class="btn btn-primary" ng-click="UpdateSkipItemLog()">Clear Skip Log</button>

            </div>
        </div>
    </div>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MMContent" runat="server">
</asp:Content>
