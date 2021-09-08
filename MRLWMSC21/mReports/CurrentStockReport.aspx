<%@ Page Title="Current Stock:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CurrentStockReport.aspx.cs" Inherits="MRLWMSC21.mReports.CurrentStockReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InvContent" runat="server">


    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="CurrentStockReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <!--==================================== FOR Importaing Data Buttons ====================================== -->
    <script src="Scripts/FileSaver.min.js"></script>    
    <script src="Scripts/html2canvas.min.js"></script>
    <script src="Scripts/jspdf.min.js"></script>
    <script src="Scripts/jspdf.plugin.autotable.js"></script>
    <script src="Scripts/tableExport.min.js"></script>
    <script src="Scripts/xlsx.core.min.js"></script>
    <!--==================================== FOR Importaing Data Buttons ====================================== -->
<%--    <script src="../mMaterialManagement/Scripts/BrowserPrint-1.0.4.js"></script>
    <script src="../mMaterialManagement/Scripts/DevDemo.js"></script>--%>
<%--        <script type="text/javascript">
        $(document).ready(setup_web_print);
    </script>--%>
    <style>
         /* Absolute Center Spinner */
        .btnStyle {
            min-width:30px !important;
            justify-content:center;
                padding: 0px;
    border-radius: 100%;
    justify-content: center;
    height: 30px;
        }

        #divDetails .btn .fa {
            margin-left:0px !important;
        }

   

        .loading
        {
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
            .loading:before
            {
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
            .loading:not(:required)
            {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after
                {
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

        @-webkit-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>

    <script>

        //================================== Getting Column Options ===================================//
       
        let MenuID = 0;
        const GetDisplayColumns = () => {
            debugger;
            MenuID = $(".activeMenu a").attr("class").split("MenuID")[1];
            $.ajax({
                url: '../mReports/CurrentStockReport.aspx/GetColumnsWithMenuID',
                data: "{'MenuID' : '" + MenuID + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response) {
                    debugger;
                    const dt = JSON.parse(response.d);           
                    GetConfigureColumnData(dt);
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        }
        //================================== Getting Column Options ===================================//


        //================================== Bind Column Options ===================================//
        const GetConfigureColumnData = (data) => {
            debugger;
            if (data.Table != null && data.Table.length > 0) {
                let ConfigurationwithMandatory = [];

                const IsMandatory = $.grep(data.Table, function (a) { return a.IsMandatory == true });
                const NonMandatory = $.grep(data.Table, function (a) { return a.IsMandatory == false || a.IsMandatory == null });

                if (data.Table1 != null && data.Table1.length > 0) {

                    const NonConfigureData = differenceInFirstArray(data.Table, data.Table1, "DisplayText");
                    $("#selAvailable").empty();
                    for (let i = 0; i < NonConfigureData.length; i++) {
                        $("#selAvailable").append($("<option id='OptionAvb" + (i + 1) + "'></option>").val(NonConfigureData[i].GEN_MST_DynamicColumn_ID).html(NonConfigureData[i].DisplayText));
                    }

                    ConfigurationwithMandatory = IsMandatory.concat(data.Table1);
                    const getDuplicateData = removeDuplicates(ConfigurationwithMandatory, "DisplayText");
                    debugger;
                    $("#selSelected").empty();
                    for (i = 0; i < getDuplicateData.length; i++) {
                        $("#selSelected").append($("<option id='OptionSel" + (i + 1) + "'></option>").val(getDuplicateData[i].GEN_MST_DynamicColumn_ID).html(getDuplicateData[i].DisplayText));
                    }
                }

                else {
                    $("#selAvailable").empty();
                    for (let i = 0; i < NonMandatory.length; i++) {
                        $("#selAvailable").append($("<option id='OptionAvb" + (i + 1) + "'></option>").val(NonMandatory[i].GEN_MST_DynamicColumn_ID).html(NonMandatory[i].DisplayText));
                    }


                    $("#selSelected").empty();
                    for (let i = 0; i < IsMandatory.length; i++) {
                        $("#selSelected").append($("<option id='OptionSel" + (i + 1) + "'></option>").val(IsMandatory[i].GEN_MST_DynamicColumn_ID).html(IsMandatory[i].DisplayText));
                    }
                }

                if (data.Table2 != null && data.Table2.length > 0) {
                    $("#selSort").empty();
                    for (let k = 0; k < data.Table2.length; k++) {
                        var type = "";
                        if (data.Table2[k].SortType == 0) {
                            type = "Asc";
                        }
                        else {
                            type = "Desc";
                        }
                        $("#selSort").append($("<option id='OptionSort" + (k + 1) + "'></option>").val(data.Table2[k].GEN_MST_DynamicColumn_ID).html(data.Table2[k].DisplayText + " - [ " + type + " ]"));
                    }
                }

                else {
                    const dt = $.grep(data.Table, function (a) { return a.DisplayText == "Tenant" });
                    $("#selSort").empty();
                    $("#selSort").append($("<option id='OptionSort" + (0 + 1) + "'></option>").val(dt[0].GEN_MST_DynamicColumn_ID).html(dt[0].DisplayText + " - [ Asc ]"));
                }
            }
        }
        //================================== Bind Column Options ===================================//


        //================================== Add Column Options From Available To Selected Options ===================================//
        const AddAttributes = () => {
            debugger;
            let $Sel = $("#selSelected");
            const sellAr = [];
            $Sel.find('option').each(function () {
                sellAr.push({ value: $(this).val(), text: $(this).text() });
            });
            $("#selSelected").empty();
            for (let i = 0; i < sellAr.length; i++) {
                $("#selSelected").append($("<option id='OptionSel" + (i + 1) + "' data-attr='" + (i + 1) + "'></option>").val(sellAr[i].value).html(sellAr[i].text));
            }

            let data = $("#selAvailable").val();
            const data1 = [];
            let $el = $("#selAvailable");
            $el.find('option:selected').each(function () {
                data1.push({ value: $(this).val(), text: $(this).text() });
            });
            let dt = data1;
            let avb = $("#selSelected")[0].length;
            for (let i = 0; i < data1.length; i++) {
                avb = avb + 1;
                $("#selSelected").append($("<option id='OptionSel" + avb + "' data-attr='" + (i + 1) + "'></option>").val(data1[i].value).html(data1[i].text));
            }
            $('#selAvailable option:selected').each(function (index, option) {
                $(option).remove();
            });
            $("#btnRight").attr("disabled", false);
        }
        //================================== Add Column Options From Available To Display Options ===================================//

        //================================== Return Column Options From Display To Available Options ===================================//

        const ReturnAttributes = () => {
            debugger;
            let $Avbl = $("#selAvailable");
            const avblAr = [];
            $Avbl.find('option').each(function () {
                avblAr.push({ value: $(this).val(), text: $(this).text() });
            });
            $("#selAvailable").empty();
            for (let i = 0; i < avblAr.length; i++) {
                $("#selAvailable").append($("<option id='OptionAvb" + (i + 1) + "' data-attr='" + (i + 1) + "'></option>").val(avblAr[i].value).html(avblAr[i].text));
            }

            let data = $("#selSelected").val();
            const data1 = [];
            let $el = $("#selSelected");
            $el.find('option:selected').each(function () {
                data1.push({ value: $(this).val(), text: $(this).text() });
            });

            let rtnavb = $("#selAvailable")[0].length;
            debugger;
            const dt = data1;
            const comp = $.grep(data1, function (a) { return a.text == "Tenant" || a.text == "Part#" || a.text == "WH" || a.text == "SLoc" || a.text == "QoH" || a.text == "Bin" || a.text == "InAct" || a.text == "OutAct" || a.text == "InQty(R)" || a.text == "OutQty(R)" });
            if (comp.length == 0) {               
                for (let i = 0; i < data1.length; i++) {
                    const checkdt = $.grep(avblAr, function (a) { return a.text == data1[i].text });
                    if (checkdt.length == 0) {
                        rtnavb = rtnavb + 1;
                        $("#selAvailable").append($("<option id='OptionAvb" + rtnavb + "'></option>").val(data1[i].value).html(data1[i].text));
                    }
                }
                $('#selSelected option:selected').each(function (index, option) {
                    $(option).remove();
                });
            }
            else {
                showStickyToast(false, "This Option is Mandatory", false);
                return false;
            }
        }
        //================================== Return Column Options From Display To Available Options ===================================//


        //================================== Up Data For Display Columns ===================================//
        const upData = () => {
            debugger;
            $("#selSelected").moveSelectedUp();
        }
        //================================== Up Data For Display Columns ===================================//

        //================================== Down Data For Display Columns ===================================//
        const downData = () => {
            debugger;
            $("#selSelected").moveSelectedDown();
        }
        //================================== Down Data For Display Columns ===================================//


       //================================== Add Column Options From Display To Sorting Options ===================================//

        const AddAttributesSort = () => {
            debugger;            
            const data1 = [];
            const filterData = [];
            let $el = $("#selSelected");
            $el.find('option:selected').each(function () {
                filterData.push({ value: $(this).val(), text: $(this).text(), id: $(this).attr("id") });
            });

            const dt = [];
            const data3 = [];
            let data2 = $("#selSort")[0].length;
            let srt = $("#selSort")[0].length;
            if (data2 != 0) {
                for (let j = 0; j < data2; j++) {
                    dt.push({ value: $("#selSort option")[j].value, text: $("#selSort option")[j].text.split("-")[0].trim() });
                }
            }
            $("#selSort").empty();
            for (i = 0; i < dt.length; i++) {
                $("#selSort").append($("<option id='OptionSort" + (i + 1) + "' data-attr='" + (i + 1) + "'></option>").val(dt[i].value).html(dt[i].text + " - [ Asc ]"));
            }


            if (filterData != null && filterData.length > 0) {
                for (let i = 0; i < filterData.length; i++) {
                    srt = srt + 1;
                    const obj = $.grep(dt, function (a) { return a.text == filterData[i].text });
                    if (obj.length == 0) {
                        $("#selSort").append($("<option id='OptionSort" + srt + "' data-attr='" + (i + 1) + "'></option>").val(filterData[i].value).html(filterData[i].text + " - [ Asc ]"));
                    }
                }
                $("#btnRight").attr("disabled", false);
            }
        }

         //================================== Add Column Options From Display To Sorting Options ===================================//

        //================================== Return Column Options From Sorting To Display Options ===================================//
        const ReturnAttributesSort = () => {
            debugger;
            const dt = [];
            const data3 = [];
            let data2 = $("#selSelected")[0].length;
            if (data2 != 0) {
                for (let j = 0; j < data2; j++) {
                    dt.push({ value: $("#selSelected option")[j].value, text: $("#selSelected option")[j].text, id: 'OptionSel' + (j + 1) });
                }
            }

            $("#selSelected").empty();
            for (let i = 0; i < dt.length; i++) {
                $("#selSelected").append($("<option id='OptionSel" + (i + 1) + "' data-attr='" + (i + 1) + "'></option>").val(dt[i].value).html(dt[i].text));
            }

            const data1 = [];
            const filterData = [];
            let $el = $("#selSort");
            $el.find('option:selected').each(function () {
                filterData.push({ value: $(this).val(), text: $(this).text().split("-")[0].trim(), id: $(this).attr("id") });
            });           

            let selcount = data2;
            if (filterData != null && filterData.length > 0) {
                for (let x = 0; x < filterData.length; x++) {
                    var obj = $.grep(dt, function (a) { return a.text == filterData[x].text });
                    if (obj.length != 0) {
                        const comp = $.grep(filterData, function (a) { return a.text == "Tenant" });
                        if (comp.length == 0) {
                            $("#" + filterData[x].id).remove();
                        }
                        else
                        {
                            showStickyToast(false, "This Option is Mandatory", false);
                            return false;
                        }
                    }
                    else
                    {
                        selcount = selcount + 1;
                        $("#selSelected").append($("<option id='OptionSel" + selcount + "' data-attr='" + selcount + "'></option>").val(filterData[x].value).html(filterData[x].text));
                        $("#" + filterData[x].id).remove();
                    }
                }
            }            
        } 
        //================================== Return Column Options From Sorting To Display Options ===================================//

        //================================== Up Data For Sorting Columns ===================================//
        const upData1 = () => {
            debugger;
            $("#selSort").moveSelectedUp();
        }
         //================================== Up Data For Sorting Columns ===================================//


         //================================== Down Data For Sorting Columns ===================================//
        const downData1 = () => {
            debugger;
            $("#selSort").moveSelectedDown();
        }
         //================================== Down Data For Sorting Columns ===================================//


        //================================== Ascending Data For Sorting Columns ===================================//
        var accendingObj = []
        var essendingArt = []       
     
              const ascData = () => {
            if (document.getElementById("selSort").selectedIndex < 0) {
                showStickyToast(false,"Please Select Sorting Columns", false);
                return;
            }
      
            accendingObj = [];
            essendingArt = [];
            debugger;
              var total = $('#selSort option').length;
            var seleTot = $('#selSort option:selected').length

            if (total == seleTot) {
                $('#selSort option:selected').each(function (index, option) {

                    var val = $(this).text().split("-")[0];
                    essendingArt = accendingObj.push(val);
                });
                $('#selSort option').remove();
                accendingObj.sort();



                accendingObj.map(each => {
                    $('#selSort').append('<option>' + each + " - [ Asc ]" + '</option>');
                });
            }
            else {
                showStickyToast(false, "Please select all sorting columns", false)
            }
        }

       


        //================================== Ascending Data For Sorting Columns ===================================//

         //================================== Descending Data For Sorting Columns ===================================//

        const descData = () => {
              if (document.getElementById("selSort").selectedIndex < 0) {
                showStickyToast(false,"Please Select Sorting Columns", false);
                return;
            }

            var total = $('#selSort option').length;
            var seleTot = $('#selSort option:selected').length

            if (total == seleTot) {
                //console.log(seleTot)
                accendingObj = [];
                essendingArt = [];
                $('#selSort option:selected').each(function (index, option) {
                    var val = $(this).text().split("-")[0];
                    essendingArt = accendingObj.push(val);
                });

                $('#selSort option').remove();
                accendingObj.sort().reverse();

                accendingObj.map(each => {
                    $('#selSort').append('<option>' + each + " - [ Desc]" + '</option>');
                });
            } else {
                showStickyToast(false, "Please select all sorting columns", false)
            }
        }


         //================================== Descending Data For Sorting Columns ===================================//


        //================================== Remove Duplicates in an array ===================================//

        const removeDuplicates = (arr, prop) => {
            const obj = {};
            for (let i = 0, len = arr.length; i < len; i++) {
                if (!obj[arr[i][prop]]) obj[arr[i][prop]] = arr[i];
            }
            var newArr = [];
            for (let key in obj) newArr.push(obj[key]);
            return newArr;
        }

        //================================== Remove Duplicates in an array ===================================//

       //================================== Comparing Between two arrays ===================================//
     
        const differenceInFirstArray = (array1, array2, compareField) => {
            return array1.filter(function (current) {
                return array2.filter(function (current_b) {
                    return current_b[compareField] === current[compareField];
                }).length == 0;
            });
        }

         //================================== Comparing Between two arrays ===================================//


         //================================== Moving Upward & Downward Columns ===================================//
        const moveSelected = (select, up) => {
                debugger;
                var $select = $(select);
                var $selected = $(":selected", $select);
                if (!up) {
                    $selected = $($selected.get().reverse());
                }
                $selected.each(function () {
                    var $this = $(this);
                    if (up) {
                        var $before = $this.prev();
                        if ($before.length > 0 && !$before.is(":selected")) {
                            $this.insertBefore($before);
                        }
                    } else {
                        var $after = $this.next();
                        if ($after.length > 0 && !$after.is(":selected")) {
                            $this.insertAfter($after);
                        }
                    }
                });
            }
            $.fn.moveSelectedUp = function () {
                debugger;
                return this.each(function () {
                    moveSelected(this, true);
                });
            };
            $.fn.moveSelectedDown = function () {
                debugger;
                return this.each(function () {
                    moveSelected(this, false);
                });
            };

            

         //================================== Moving Upward & Downward Columns ===================================//

    </script>

<style>


    td .checkbox{
        margin-top:0px;
    }

    .btnStyle {padding:3px;width:30px;}

    .radio, .checkbox {
        margin-top:0 !important;
        margin-bottom:0 !important;
    }

    #divPage {
        float: right;
        margin: 0px 10px;
    }

        #divPage button {
            background: #fff;
            border: #fff;
            box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
            border-radius: 3px;
            color: black;
            border: 1px soliD #FFF;
        }

        #divPage .Highlight {
            background: #29328b;
            border: #29328b;
            box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
            border-radius: 3px;
            color: black;
            border: 1px soliD #29328b;
            color: #FFF;
        }

    .text{
  mso-number-format:"\@";
}
    .tableLoader::before {
        height:392px;
    }
   
</style>


    

    <script>
       
    </script>

    <script type="text/javascript">


        //var wcppGetPrintersDelay_ms = 0;
        var wcppGetPrintersTimeout_ms = 10000; //10 sec
        var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec
        // var jsWebClientPrint=(function(){var setA=function(){var e_id='id_'+new Date().getTime();if(window.chrome){$('body').append('<a id="'+e_id+'"></a>');$('#'+e_id).attr('href','webclientprintiv:'+arguments[0]);var a=$('a#'+e_id)[0];var evObj=document.createEvent('MouseEvents');evObj.initEvent('click',true,true);a.dispatchEvent(evObj)}else{$('body').append('<iframe name="'+e_id+'" id="'+e_id+'" width="1" height="1" style="visibility:hidden;position:absolute" />');$('#'+e_id).attr('src','webclientprintiv:'+arguments[0])}setTimeout(function(){$('#'+e_id).remove()},5000)};return{print:function(){setA('http://192.168.1.24/ITWMS/DemoPrintCommandsHandler.ashx?clientPrint'+(arguments.length==1?'&'+arguments[0]:''))},getPrinters:function(){setA('-getPrinters:http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?sid='+'3pufxe55b5dtxg453ywcy055');var delay_ms=(typeof wcppGetPrintersDelay_ms==='undefined')?0:wcppGetPrintersDelay_ms;if(delay_ms>0){setTimeout(function(){$.get('http://localhost/WebClientPrintAPI.ashx?getPrinters&sid='+'3pufxe55b5dtxg453ywcy055',function(data){if(data.length>0){wcpGetPrintersOnSuccess(data)}else{wcpGetPrintersOnFailure()}})},delay_ms)}else{var fncGetPrinters=setInterval(getClientPrinters,wcppGetPrintersTimeoutStep_ms);var wcpp_count=0;function getClientPrinters(){if(wcpp_count<=wcppGetPrintersTimeout_ms){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getPrinters&sid='+'3pufxe55b5dtxg453ywcy055',{'_':$.now()},function(data){if(data.length>0){clearInterval(fncGetPrinters);wcpGetPrintersOnSuccess(data)}});wcpp_count+=wcppGetPrintersTimeoutStep_ms}else{clearInterval(fncGetPrinters);wcpGetPrintersOnFailure()}}}},getPrintersInfo:function(){setA('-getPrintersInfo:http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?sid='+'3pufxe55b5dtxg453ywcy055');var delay_ms=(typeof wcppGetPrintersDelay_ms==='undefined')?0:wcppGetPrintersDelay_ms;if(delay_ms>0){setTimeout(function(){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getPrintersInfo&sid='+'3pufxe55b5dtxg453ywcy055',function(data){if(data.length>0){wcpGetPrintersOnSuccess(data)}else{wcpGetPrintersOnFailure()}})},delay_ms)}else{var fncGetPrintersInfo=setInterval(getClientPrintersInfo,wcppGetPrintersTimeoutStep_ms);var wcpp_count=0;function getClientPrintersInfo(){if(wcpp_count<=wcppGetPrintersTimeout_ms){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getPrintersInfo&sid='+'3pufxe55b5dtxg453ywcy055',{'_':$.now()},function(data){if(data.length>0){clearInterval(fncGetPrintersInfo);wcpGetPrintersOnSuccess(data)}});wcpp_count+=wcppGetPrintersTimeoutStep_ms}else{clearInterval(fncGetPrintersInfo);wcpGetPrintersOnFailure()}}}},getWcppVer:function(){setA('-getWcppVersion:http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?sid='+'3pufxe55b5dtxg453ywcy055');var delay_ms=(typeof wcppGetVerDelay_ms==='undefined')?0:wcppGetVerDelay_ms;if(delay_ms>0){setTimeout(function(){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getWcppVersion&sid='+'3pufxe55b5dtxg453ywcy055',function(data){if(data.length>0){wcpGetWcppVerOnSuccess(data)}else{wcpGetWcppVerOnFailure()}})},delay_ms)}else{var fncWCPP=setInterval(getClientVer,wcppGetVerTimeoutStep_ms);var wcpp_count=0;function getClientVer(){if(wcpp_count<=wcppGetVerTimeout_ms){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getWcppVersion&sid='+'3pufxe55b5dtxg453ywcy055',{'_':$.now()},function(data){if(data.length>0){clearInterval(fncWCPP);wcpGetWcppVerOnSuccess(data)}});wcpp_count+=wcppGetVerTimeoutStep_ms}else{clearInterval(fncWCPP);wcpGetWcppVerOnFailure()}}}},send:function(){setA.apply(this,arguments)}}})();
        function wcpGetPrintersOnSuccess()
        {
            console.log(arguments);
         
            debugger;
            // Display client installed printers
            if (arguments[0].length > 0) {
                var p = arguments[0].split("|");
                var options = '';
                for (var i = 0; i < p.length; i++) {
                    options += '<option>' + p[i] + '</option>';
                }
                $('#installedPrinterName').html(options);
                $('#installedPrinterName').focus();
                $('#loadPrinters').hide();
            } else
            {
                // alert("No printers are installed in your system.");
                showStickyToast(false, "Please install Web Client Print Software in your PC", false);
                return false;
            }
        }

        function wcpGetPrintersOnFailure() {
            // Do something if printers cannot be got from the client 
            //alert("No printers are installed in your system.");
            showStickyToast(false, "No printers are installed in your system", false);
            return false;
        }
    </script>
    <script type="text/javascript">

        function doClientPrint() {
            debugger;
            //collect printer settings and raw commands
            var printerSettings = $("#myForm :input").serialize();

            //store printer settings in the server cache...
            $.post('CurrentStockClientPrintDemo.ashx',
                printerSettings
            );

            // Launch WCPP at the client side for printing...
            var sessionId = $("#sid").val();
            jsWebClientPrint.print('sid=' + sessionId);

        }


        $(document).ready(function () {

            //jQuery-based Wizard
            $("#myForm").formToWizard();

            //change printer options based on user selection
            $("#pid").change(function ()
            {
                debugger;
                var printerId = $("select#pid").val();

                displayInfo(printerId);
                hidePrinters();
                if (printerId == 2)
                {
                    $("#installedPrinter").show();
                    // $("#installedPrinterName").removeAttr("disabled");
                    javascript: jsWebClientPrint.getPrinters();


                }
                else if (printerId == 3)
                {
                    $("#installedPrinter").hide();
                    $("#netPrinter").show();
                }
                else if (printerId == 4) {
                    $("#installedPrinter").hide();
                    $("#parallelPrinter").show();
                }
                else if (printerId == 5) {
                    $("#installedPrinter").hide();
                    $("#serialPrinter").show();
                }
            });

            hidePrinters();
            displayInfo(0);


        });

        function displayInfo(i)
        {
         
            if (i == 0)
                $("#info").html('This will make the WCPP to send the commands to the printer installed in your machine as "Default Printer" without displaying any dialog!');
            else if (i == 1)
                $("#info").html('This will make the WCPP to display the Printer dialog so you can select which printer you want to use.');
            else if (i == 2)
                $("#info").html('Please specify the <b>Printer\'s Name</b> as it figures installed under your system.');
            else if (i == 3)
                $("#info").html('Please specify the Network Printer info.<br /><strong>On Linux &amp; Mac</strong> it\'s recommended you install the printer through <strong>CUPS</strong> and set the assigned printer name to the <strong>"Use an installed Printer"</strong> option on this demo.');
            else if (i == 4)
                $("#info").html('Please specify the Parallel Port which your printer is connected to.<br /><strong>On Linux &amp; Mac</strong> you must install the printer through <strong>CUPS</strong> and set the assigned printer name to the <strong>"Use an installed Printer"</strong> option on this demo.');
            else if (i == 5)
                $("#info").html('Please specify the Serial RS232 Port info which your printer does support.<br /><strong>On Linux &amp; Mac</strong> you must install the printer through <strong>CUPS</strong> and set the assigned printer name to the <strong>"Use an installed Printer"</strong> option on this demo.');
        }

        function hidePrinters() {
            $("#installedPrinter").hide();
            $("#netPrinter").hide();
            $("#parallelPrinter").hide();
            $("#serialPrinter").hide();
        }




        /* FORM to WIZARD */
        /* Created by jankoatwarpspeed.com */

        (function ($) {
            $.fn.formToWizard = function () {

                var element = this;

                var steps = $(element).find("fieldset");
                var count = steps.size();


                // 2
                $(element).before("<ul id='steps'></ul>");

                steps.each(function (i) {
                    $(this).wrap("<div id='step" + i + "'></div>");
                    $(this).append("<p id='step" + i + "commands'></p>");

                    // 2
                    var name = $(this).find("legend").html();
                    $("#steps").append("<li id='stepDesc" + i + "'>Step " + (i + 1) + "<span>" + name + "</span></li>");

                    if (i == 0) {
                        createNextButton(i);
                        selectStep(i);
                    }
                    else if (i == count - 1) {
                        $("#step" + i).hide();
                        createPrevButton(i);
                    }
                    else {
                        $("#step" + i).hide();
                        createPrevButton(i);
                        createNextButton(i);
                    }
                });

                function createPrevButton(i) {
                    var stepName = "step" + i;
                    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Prev' class='prev btn btn-info'>< Back</a>");

                    $("#" + stepName + "Prev").bind("click", function (e) {
                        $("#" + stepName).hide();
                        $("#step" + (i - 1)).show();

                        selectStep(i - 1);
                    });
                }

                function createNextButton(i) {
                    var stepName = "step" + i;
                    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Next' class='next btn btn-info'>Next ></a>");

                    $("#" + stepName + "Next").bind("click", function (e) {
                        $("#" + stepName).hide();
                        $("#step" + (i + 1)).show();

                        selectStep(i + 1);
                    });
                }

                function selectStep(i) {
                    $("#steps li").removeClass("current");
                    $("#stepDesc" + i).addClass("current");
                }

            }
        })(jQuery);

    </script>


    <div class="dashed"></div>
     <div ng-app="myApp" ng-controller="CurrentStockReport" class="pagewidth">     
         <inv-preloader is="show" style="display:none"></inv-preloader>
         <div class="divlineheight"></div>
         <div class="" style="padding:0;">
            <div>
                <div class="row" style="margin:0px;">
                     <div class="col m3 s3">
                        <div class="flex">
                          
                            <input type="text" id="txtWarehouse" class="" required=""/>
                            <%--<label>Warehouse</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "Warehouse")%>  </label>
                        </div>
                    </div>    

                    <div class="col m3 s3">
                            <div class="flex">
                               <%-- /*changed the labels with Globalization Tag for multilingual by Althi Mohan*/--%>
                                <input type="text" id="txtTenant"  ngchange="getTenant()" ng-click="getTenant()"   class="TextboxInventoryAuto" required="" />
                               <%-- <label>Tenant </label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                 <input type="hidden" class="p1save" id="Account" />
                            </div>
                            
                    </div>
                       
                    <div class="col m3 s3">
                            <div class="flex">
                                <input type="text" id="txtPartnumber"  ngchange="getpartnumber()" ng-click="getpartnumber()"  class="TextboxInventoryAuto" required="" />
                            <%--    <label>Part Number</label>--%>
                                    <label> <%= GetGlobalResourceObject("Resource", "PartNumber")%> </label>
                                </div>
                           
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" id="txtMaterialType" ngchange="getmtype()" ng-click="getmtype()"  class="TextboxInventoryAuto" required="" />
                           <%--  <label>Material Type </label>--%>
                              <label><%= GetGlobalResourceObject("Resource", "MaterialType")%>  </label>
                            </div>
                    </div>
                   
                </div>
                <div class="ishide">
                    <div class="row" style="margin:0px;">
                        <div class="col m3 s3"> 
                            <div class="flex">
                                 <input type="text" id="txtLocation"  ngchange="getlocation()" ng-click="getlocation()"  class="TextboxInventoryAuto" required="" />
                                 <%--<label>Location</label> --%>
                                <label> <%= GetGlobalResourceObject("Resource", "Location")%></label> 
                            </div>
                        </div>
                        <div class="col m3 s3"> 
                           <div class="flex">                              
                               <input type="text" id="txtKitId"  ngchange="getkitplannerid()" ng-click="getkitplannerid()"  class="TextboxInventoryAuto" required="" />
                          <%--  <label>Kit Code</label>  --%>
                                 <label> <%= GetGlobalResourceObject("Resource", "KitCode")%> </label>  
                           </div> 
                        </div>
                         <div class="col m3 s3">  
                             <div class="flex">
                                  <input type="text" id="txtBatchNo" class="" required="" />
                              <%--  <label>Batch No.</label>  --%>
                                   <label><%= GetGlobalResourceObject("Resource", "BatchNo")%> </label>  
                               </div>
                        </div>  
                        <div class="col m3 s3">
                            <div class="flex">
                                      <input type="text" id="txtOEMpartNum" class="" required="" />
                               <%-- <label>OEM Part No.</label>   --%>
                                 <label><%= GetGlobalResourceObject("Resource", "OEMPartNo")%> </label>   
                                </div>
                        </div>                   
                    </div>

                    <div class="row" style="margin:0px;">
                        

                        <div class="col m3 s3">
                           <div class="flex">
                            <input type="text" id="txtIndustry" class="" SkinID="txt_Req" required="">
                         <%--   <label>Industry</label>--%>
                                  <label><%= GetGlobalResourceObject("Resource", "Industry")%></label>
                            <input type="hidden" id="IndustryID" value="0" /></div>
                        </div>

                        <div class="col m3 s3">
                            <div class="flex">
                                <input type="text" id="txtContainer" class="" skinid="txt_Req" required="">
                                <label>Pallet</label>
                            </div>
                        </div>
                        <div class="col m3 s3">
                            <%--<div class="flex" id="divFirstAttibute"></div>--%>
                             <div class="flex" id="divFirstAttibute" >
                            <input type="text" id="txttype" class="" SkinID="txt_Req" required="">
                            <label>Search Type</label>
                                 
                            <input type="hidden" id="types" value="0" /></div>
                        </div>

                        <div class="col m3 s3">
                             <div class="flex"  id="divFirstAttibute1">
                            <input type="text" id="txtTypeText" class="" SkinID="txt_Req" required="">
                            <label>Data</label>
                                  
                            <%--<div class="flex" id="divFirstAttibute1"></div>--%>
                        </div>

                        <div class="col m3 s3">
                            <div class="flex" id="divFirstAttibute2"></div>
                        </div>

                    </div>

                            <div class="col m3 s3"> 
                 <%--   <div class="row" style="margin:0px;">   --%>
                         <div id="divIndustryContent" class="col m12" ></div>
                    </div>
                 </div>
                <div>
                    <div>
                          <%----------------------------------------- Panel 9 ---------------------------------------------%>
                  
                       
                        <%----------------------------------------- Panel 9 ---------------------------------------------%>
                    </div>
                </div>

                
            </div>  
             
           
         </div>
         <div class="row" style="margin:0" hidden>

             <div class="col m3 s3" >
                 <div class="flex" >
                     <select ng-model="ddlPrinterType" id="selPrinterType" class="" ng-options="print.PrinterId as print.PrinterName for print in printers" required="">
                         <option value=""></option>
                     </select>
                      <label> <%= GetGlobalResourceObject("Resource", "Printer")%></label>
                 </div>

                 <div id="printer_select" class="flex" style="display:none;">
                     <select id="printers"></select>
                      <label><%= GetGlobalResourceObject("Resource", "Printer")%> </label>
                 </div>

             </div>

             


             
            
            
        </div>

             <div class="row" id="myForm">
                 <div class="col m3">
                     <div class="flex">
                        <%-- <input type="hidden" id="sid" name="sid" value="<%= HttpContext.Current.Session.SessionID %>" />--%>
                         <input type="hidden" id="sid" name="sid" value="currentStock" />
                         <select id="pid" name="pid" class="form-control">
                             <option selected="selected" value="0">Use Default Printer</option>
                            <%-- <option value="1">Display a Printer dialog</option>--%>
                             <option value="2">Use an installed Printer</option>
                             <option value="3">Use an IP/Ethernet Printer</option>
<%--                             <option value="4">Use a LPT port</option>
                             <option value="5">Use a RS232 (COM) port</option>--%>
                         </select>
                         <label><%= GetGlobalResourceObject("Resource", "Printer")%></label>
                         <span class="errorMsg"></span>
                     </div>
                 </div>
                 <div class="col m3" id="installedPrinter">
                     <div class="flex">
                         <select name="installedPrinterName" id="installedPrinterName" class="form-control"></select>
                         <label for="installedPrinterName">Select an installed Printer:</label>
                         <span class="errorMsg"></span>

                         <div id="loadPrinters" name="loadPrinters" hidden>
                             <a onclick="javascript:jsWebClientPrint.getPrinters();" class="btn btn-success">Load installed printers...</a>
                         </div>
                     </div>
                 </div>
                 <div class="col m6" id="netPrinter">
                     <div class="col m6">
                         <div class="flex">
                             <input type="text" name="netPrinterHost" id="netPrinterHost" class="form-control" required="" />
                             <label for="netPrinterHost">Printer's IP Address:</label>
                             <span class="errorMsg"></span>
                         </div>
                     </div>
                     <div class="col m6">
                         <div class="flex">
                             <input type="text" name="netPrinterPort" id="netPrinterPort" class="form-control" required="" />
                             <label for="netPrinterPort">Printer's Port:</label>
                             <span class="errorMsg"></span>
                         </div>
                     </div>
                 </div>
                 <div class="col m12" hidden>
                     <div class="flex">
                         <textarea id="printerCommands" name="printerCommands" rows="10" cols="80" class="form-control" style="min-width: 100%"></textarea>
                     </div>
                 </div>
                 <div class="col m3 s3">
                     <div class="flex">
                         <select ng-model="ddlPrintLabel" id="selPrintLabel" class="" ng-options="label.BarCodeId as label.BarCode for label in labels" required="">
                             <option value=""></option>
                         </select>
                         <label><%= GetGlobalResourceObject("Resource", "Label")%> </label>
                         <span class="errorMsg"></span>
                     </div>
                 </div>                 
             </div>
             <div class="row">
                 <div class="col m12" style="margin-bottom: 0px;">
                     <gap></gap>
                     <flex end>
                     <button type="button" id="btnSearch" ng-click="Getgedetails(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                     <button type="button" class="btn btn-primary isvisibleadv"><%= GetGlobalResourceObject("Resource", "AdvancedSearch")%>  <i class="material-icons">arrow_drop_down</i></button>
                     <button type="button" id="btnColOptions" class="btn btn-primary" onclick=" GetDisplayColumns();" data-target="#showColOptions" data-toggle="modal"><%= GetGlobalResourceObject("Resource", "ColumnOptions")%>  <i class="material-icons">list</i></button>
                     <button type="button" ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                    <%-- <button type="button" data-target="#Print" class="btn btn-primary" data-toggle="modal">Print <i class="material-icons">print</i></button>--%>
                     </flex>
                 </div>
             </div>
          <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12"  style="padding: 0px 10px;">
                <div class="divmainwidth" >
                    <table class="table-striped tableLoader" id="tbldatas" style="display:none;">
                        <thead>
                            <tr class="">
                                <th style="text-align: left !important;" ng-repeat="HDR in DynamicHeaders" ng-if="HDR.DynamicColumn != 'TenantID' && HDR.DynamicColumn != 'MaterialMasterID'">
                                    <div>{{ HDR.DynamicColumn }}</div>
                                </th>
                            <%--    <th style="text-align:left !important;" id="thhead" ng-show="DynamicColumn[0] != 'Part#'">Print--%>
                                    <th style="text-align:left !important;" id="thhead" ng-show="DynamicColumn[0] != 'Part#'"><%= GetGlobalResourceObject("Resource", "Print")%>
                                 
                                </th>
                            </tr>
                        </thead>
                        <tbody dir-paginate="row in DynamicColumns | itemsPerPage : noofrecords" total-items="Totalrecords">
                            <%--ng-if="row.QoH != 0 && row.InQty(R) != 0 && row.OutQty(R)!=0"--%>
                                <tr>
                                    <td ng-repeat="key in cols" ng-if="key != 'TenantID' && key != 'MaterialMasterID' && key != 'RID'">
                                        
                                        <div>{{row[key]}}</div>
                                      
                                          <%--  <div ng-if="key == 'InAct'" style="text-align: left !important;">
                                                <a class="inward" style="text-decoration: none;" target="_blank" ng-href="../mInventory/ActivityReport.aspx?mmid={{row.MaterialMasterID}}&TransactionType=1&tid={{row.TenantID}}"><i class="material-icons" style="color: green;">system_update_alt</i><em class="sugg-tooltis" style="left: -135px;">Inbound activity</em></a>
                                            </div>
                                            <div ng-if="key == 'OutAct'" style="text-align: left !important;">
                                                <a class="outward" style="text-decoration: none;" target="_blank" ng-href="../mInventory/ActivityReport.aspx?mmid={{row.MaterialMasterID}}&TransactionType=2&tid={{row.TenantID}}"><i class="material-icons" style="color: red;">open_in_new</i><em class="sugg-tooltis" style="left: -135px;">Outbound activity</em></a>
                                            </div>--%>
                                    </td>
                                    <td style="text-align:left !important;" ng-show="DynamicColumn[0] != 'Part#'">                                       
                                        <%--<a style="text-decoration:none;color:black;" ng-click="GetMspData(row.MaterialMasterID);" data-target="#showMspItems" data-toggle="modal"><i class="material-icons">&#xE8AD;</i> <em class="sugg-tooltis">Print</em></a>--%>

                                       <a style="text-decoration:none;color:black;" ng-click="GetMspData1(row);" data-target="#showMspItems" data-toggle="modal"><i class="material-icons">&#xE8AD;</i> <em class="sugg-tooltis">Print</em></a>


                                    </td>
                                </tr>
                             <tr ng-show="Data.length == 0">
                            <%--<td colspan="20" style="text-align:center !important;">No Data Found</td>--%>
                                 <td colspan="20" style="text-align:center !important;"><%= GetGlobalResourceObject("Resource", "NoDataFound")%> </td>
                        </tr>
                        </tbody>
                        
                    </table>

                </div>
                <div flex end><dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="Getgedetails(newPageNumber)"></dir-pagination-controls><br /></div>
                <table id="tbldata"></table>
            </div>
        <div class="divlineheight"></div>
              
          </div>    

          <!-- ========================= Modal Popup For Column Options ========================================== -->
         <div class="modal inmodal" id="showColOptions" tabindex="-1" role="dialog" aria-hidden="true">
             <div class="modal-dialog" style="width: 60% !important;">
                 <div class="modal-content animated fadeIn">
                     <div class="modal-header">
                         <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <%-- <h4 class="modal-title">Column Options</h4>--%>
                          <h4 class="modal-title"> <%= GetGlobalResourceObject("Resource", "ColumnOptions")%></h4>
                     </div>
                     <div class="modal-body">
                         <div id="divValidationCycleCountMessages" class="text-danger" style="color: red !important;"></div>
                         <p></p>
                         <div id="divDetails" class="form-horizontal">
                             <form role="form">
                                 <div class="form-group">
                                     <div class="col-md-3">
                                         <%--<label class="lblFormItem">Available Columns : </label>--%>
                                         <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "AvailableColumns")%>  </label>
                                         <select multiple style="height: 200px;" id="selAvailable">
                                         </select>
                                     </div>
                                     <div class="col-md-1" style="height: 200px !important;">
                                         <div style="margin-top: calc( 100px - 5px );">
                                             <button type="button" class="btn btn-primary btnStyle" id="btnLeft"  onclick="AddAttributes();"><i class="fa fa-chevron-circle-right"></i></button>
                                             <p style="height: 7px !important;"></p>
                                             <button type="button" class="btn btn-primary btnStyle" id="btnRight"  onclick="ReturnAttributes();"><i class="fa fa-chevron-circle-left"></i></button>
                                         </div>
                                     </div>
                                     <div class="col-md-3">                                         
                                       <%--  <label class="lblFormItem">Display Columns : </label>--%>
                                           <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "DisplayColumns")%>  </label>
                                         <select multiple style="height: 200px;" id="selSelected">
                                         </select>                                        
                                     </div>
                                     <div class="col-md-1">
                                         <br />
                                         <p style="height: 10px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnUp" onclick="upData();"><i class="fa fa-chevron-circle-up"></i></button>
                                         <p style="height: 7px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnDown" onclick="downData();"><i class="fa fa-chevron-circle-down"></i></button>
                                         <p style="height: 40px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnLeft1" onclick="AddAttributesSort();"><i class="fa fa-chevron-circle-right"></i></button>
                                             <p style="height: 7px !important;"></p>
                                             <button type="button" class="btn btn-primary btnStyle" id="btnRight1" onclick="ReturnAttributesSort();"><i class="fa fa-chevron-circle-left"></i></button>
                                     </div>
                                     <div class="col-md-3">
                                         <%-- <label class="lblFormItem">Sorting Columns : </label>--%>
                                          <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "SortingColumns")%>  </label>
                                         <select multiple style="height: 200px;" id="selSort">
                                         </select>
                                     </div>
                                     <div class="col-md-1">
                                         <br />
                                         <p style="height: 10px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnUp1" onclick="upData1();"><i class="fa fa-chevron-circle-up"></i></button>
                                         <p style="height: 7px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnDown1" onclick="downData1();"><i class="fa fa-chevron-circle-down"></i></button>
                                         <br />
                                         <p style="height: 40px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnAsc" onclick="ascData();"><i class="fa fa-sort-alpha-asc"></i></button>
                                         <p style="height: 7px !important;"></p>
                                         <button type="button" class="btn btn-primary btnStyle" id="btnDesc" onclick="descData();"><i class="fa fa-sort-alpha-desc"></i></button>
                                         <br />
                                         <br />
                                     </div>
                                 </div>
                             </form>
                         </div>
                     </div>
                     <div class="modal-footer">
                         <input type="hidden" value="0" id="CCM_TRN_CycleCount_ID" class="fieldToGet" />
                         <%--<button type="button" class="btn btn-primary" id="btnCreate" ng-click="UpsertData();">OK</button>--%>
                         <button type="button" class="btn btn-primary" data-dismiss="modal" style="color: #fff !important;">Close</button>
                         <button type="button" class="btn btn-primary" id="btnCreate" ng-click="UpsertData();"><%= GetGlobalResourceObject("Resource", "OK")%></button>
                         
                     </div>
                 </div>
             </div>
         </div>
    <!-- ========================= END Modal Popup For Column Options ========================================== -->


         <!--=========================== Modal Popup For Msp Items =================================== -->

         <div class="modal inmodal" id="showMspItems" tabindex="-1" role="dialog" aria-hidden="true">
             <div class="modal-dialog" style="width: 55% !important;">
                 <div class="modal-content animated fadeIn">
                     <div class="modal-header">
                         <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>--%>
                         <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only"><%= GetGlobalResourceObject("Resource", "Close")%> </span></button>
                         <%--<h4 class="modal-title">MSP Items</h4>--%>
                         <%--<h4 class="modal-title"> <%= GetGlobalResourceObject("Resource", "MSPItems")%> </h4>--%>
                         <h4 class="modal-title"> Item [{{MspItems.MCode}}] </h4>
                     </div>
                     <div class="modal-body">                       
                         <div class="form-horizontal">
                             <form role="form" style="padding:20px;">
                                 <br />
                                 <table class="table-striped">
                                             <thead>
                                                 <tr>
                                                    <%-- <th>Part #</th>--%>
                                                      <th><%= GetGlobalResourceObject("Resource", "Part")%>  </th>
                                                    <%-- <th>Batch No.</th>--%>
                                                      <th> <%= GetGlobalResourceObject("Resource", "BatchNo")%></th>
                                                   <%--  <th>Serial No.</th>--%>
                                                       <th> <%= GetGlobalResourceObject("Resource", "SerialNo")%> </th>
                                                    <%-- <th>Mfg. Date</th>--%>
                                                      <th><%= GetGlobalResourceObject("Resource", "MfgDate")%> </th>
                                                  <%--   <th>Exp. Date</th>--%>
                                                        <th><%= GetGlobalResourceObject("Resource", "ExpDate")%> </th>
                                                   <%--  <th>Project Ref. No.</th>--%>
                                                       <th><%= GetGlobalResourceObject("Resource", "ProjectRefNo")%> </th>
                                                      <th>MRP</th>
                                                     <th>GRN Number</th>
                                                     <th>HU No.</th>
                                                     <th>HU Size</th>
                                                    <%-- <th>Quantity</th>--%>
                                                      <th> <%= GetGlobalResourceObject("Resource", "Quantity")%> </th>
                                                     <%--<th>No. Of Prints</th>--%>
                                                     <th>  <%= GetGlobalResourceObject("Resource", "NoOfPrints")%></th>
                                                    <%-- <th>Action</th>--%>
                                                      <th> <%= GetGlobalResourceObject("Resource", "Action")%> </th>
                                                 </tr>
                                             </thead>
                                             <tbody>
                                               <%--<tr ng-repeat="msp in MspItems">
                                                   <td>{{msp.MCode}}</td>
                                                     <td>{{msp.BatchNo}}</td>
                                                     <td>{{msp.SerialNo}}</td>
                                                     <td>{{msp.MfgDate}}</td>
                                                     <td>{{msp.ExpDate}}</td>
                                                     <td>{{msp.ProjectRefNo}}</td>
                                                     <td>{{msp.Quantity}}</td>--%>

                                                     <tr >                                                     
                                                     <td>{{MspItems.MCode}}</td>
                                                     <td>{{MspItems.BatchNo}}</td>
                                                     <td>{{MspItems.SerialNo}}</td>
                                                     <td>{{MspItems.MfgDate}}</td>
                                                     <td>{{MspItems.ExpDate}}</td>
                                                     <td style="text-align:center">{{MspItems.ProjectRefNo}}</td>
                                                      <td style="text-align:center">{{MspItems.MRP}}</td>
                                                     <td style="width: 85px;">{{MspItems.GRNNumber}}</td>
                                                         <td>1</td>
                                                         <td>1</td>
                                                     <td style="text-align:right">{{MspItems.Quantity}}</td>

                                                     <td>
                                                        
                                                            <%-- <input type="text" ng-model="msp.NoOfCopies" id="txtCopies" required="" style="border:0;border-bottom:1px solid var(--paper-grey-300);width:76px;" />--%>

                                                          <input type="text" ng-model="MspItems.NoOfCopies" id="txtCopies" required="" style="border:0;border-bottom:1px solid var(--paper-grey-300);width:76px;" />
                                                        
                                                     </td>
                                                     <td><a style="text-decoration:none;padding-left: 13px;" ng-click="GetPrintData(MspItems);"><i class="material-icons">&#xE8AD;</i> <em class="sugg-tooltis">Print</em></a></td>
                                                 </tr>
                                             </tbody>
                                         </table>
                                 <br />
                             </form>
                         </div>
                     </div>
                     <div class="modal-footer">                        
                         <%--<button type="button" class="btn btn-white" data-dismiss="modal" style="color: #fff !important;">Close</button>--%>
                         <button type="button" class="btn btn-primary" data-dismiss="modal" style="color: #fff !important;"><%= GetGlobalResourceObject("Resource", "Close")%> </button>
                     </div>
                 </div>
             </div>
         </div>

         <!--=========================== Modal Popup For Msp Items =================================== -->




        <%-- <div class="loading" id="divLoading" style="display: none;"></div>--%>
           <!-- /navigation -->
    <div class="container" style="width: 500px;display:none;">
        <div id="main">
            <div id="printer_data_loading" style="display: none">
                <%--<span id="loading_message">Loading Printer Details...</span><br />--%>
                <span id="loading_message"><%= GetGlobalResourceObject("Resource", "LoadingPrinterDetails")%></span><br />
                <div class="progress" style="width: 100%">
                    <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                    </div>
                </div>
            </div>
            <!-- /printer_data_loading -->
            <div style="display:none;">
            <div id="printer_details" style="display: none">
              <%--  <span id="selected_printer">No data</span>--%>
                  <span id="selected_printer"><%= GetGlobalResourceObject("Resource", "Nodata")%> </span>
                <%--<button type="button" class="btn btn-success" onclick="changePrinter()">Change</button>--%>
                <button type="button" class="btn btn-success" onclick="changePrinter()"><%= GetGlobalResourceObject("Resource", "Change")%> </button>
            </div></div>
            <br />
            <!-- /printer_details -->
            <div class="row" style="display:none;">
                <div class="col m3 s3">
                   
                    <!-- /printer_select -->
                </div>
                <div class="col m3 s3" >
               
                    <div id="print_form" style="display: none" class="flex">
                        
                <input type="text" id="entered_name" />
                       <%-- <label>ZPL String</label> --%>
                         <label> <%= GetGlobalResourceObject("Resource", "ZPLString")%></label>  
                    </div>                    
                    <!-- /print_form -->
                </div>

                <div class="col m4">
                    <%--<button type="button" class="btn btn-lg btn-primary" onclick="sendData();" value="Print">Print Label</button>--%>
                    <button type="button" class="btn btn-lg btn-primary" onclick="sendData();" value="Print"> <%= GetGlobalResourceObject("Resource", "PrintLabel")%> </button>
                </div>
            </div>



        </div>
        <!-- /main -->
        <div id="error_div" style="width: 500px; display: none">
            <div id="error_message"></div>
         <%--   <button type="button" class="btn btn-lg btn-success" onclick="trySetupAgain();">Try Again</button>--%>
               <button type="button" class="btn btn-lg btn-success" onclick="trySetupAgain();"> <%= GetGlobalResourceObject("Resource", "TryAgain")%> </button>
        </div>
        <!-- /error_div -->
    </div>
    <!-- /container -->
        </div>

         <%--<div id="myForm">
        
    <h3>Print Raw/Text Commands</h3>

<div class="container">
    <div class="row">



            <input type="hidden" id="sid" name="sid" value="<%= HttpContext.Current.Session.SessionID %>" />
            <fieldset>
                <legend>Client Printer Settings</legend>

                <div>
                    WebClientPrint does support all common printer communications like USB-Installed
                    Drivers, Network/IP-Ethernet, Serial COM-RS232 and Parallel (LPT).
                    <br />
                    <br />
                    I want to:&nbsp;&nbsp;
                    <select id="pid" name="pid" class="form-control">
                        <option selected="selected" value="0">Use Default Printer</option>
                        <option value="1">Display a Printer dialog</option>
                        <option value="2">Use an installed Printer</option>
                        <option value="3">Use an IP/Etherner Printer</option>
                        <option value="4">Use a LPT port</option>
                        <option value="5">Use a RS232 (COM) port</option>
                    </select>
                    <br />
                    <br />
                    <div id="info" class="alert alert-info" style="font-size:11px;"></div>
                    <br />
                </div>

                <div id="installedPrinter">
                    <div id="loadPrinters" name="loadPrinters">
                        WebClientPrint can detect the installed printers in your machine. <a onclick="javascript:jsWebClientPrint.getPrinters();" class="btn btn-success">Load installed printers...</a>
                        <br /><br />
                    </div>
                    <label for="installedPrinterName">Select an installed Printer:</label>
                    <select name="installedPrinterName" id="installedPrinterName" class="form-control"></select>



                </div>

                <div id="netPrinter">
                    <label for="netPrinterHost">Printer's DNS Name or IP Address:</label>
                    <input type="text" name="netPrinterHost" id="netPrinterHost" class="form-control"/>
                    <label for="netPrinterPort">Printer's Port:</label>
                    <input type="text" name="netPrinterPort" id="netPrinterPort" class="form-control"/>
                </div>

                <div id="parallelPrinter">
                    <label for="parallelPort">Parallel Port:</label>
                    <input type="text" name="parallelPort" id="parallelPort" value="LPT1" class="form-control"/>
                </div>

                <div id="serialPrinter">
                    <table border="0">
                        <tr>
                            <td valign="top">
                                <label for="serialPort">Serial Port:</label>
                                <input type="text" name="serialPort" id="serialPort" value="COM1" class="form-control"/>
                                <label for="serialPortBauds">Baud Rate:</label>
                                <input type="text" name="serialPortBauds" id="serialPortBauds" value="9600" class="form-control"/>
                                <label for="serialPortDataBits">Data Bits:</label>
                                <input type="text" name="serialPortDataBits" id="serialPortDataBits" value="8" class="form-control"/>
                            </td>
                            <td style="width:30px;"></td>
                            <td valign="top">
                                <label for="serialPortParity">Parity:</label>
                                <select id="serialPortParity" name="serialPortParity" class="form-control">
                                    <option selected="selected">None</option>
                                    <option>Odd</option>
                                    <option>Even</option>
                                    <option>Mark</option>
                                    <option>Space</option>
                                </select>

                                <label for="serialPortStopBits">Stop Bits:</label>
                                <select id="serialPortStopBits" name="serialPortStopBits" class="form-control">
                                    <option selected="selected">One</option>
                                    <option>Two</option>
                                    <option>OnePointFive</option>
                                </select>

                                <label for="serialPortFlowControl">Flow Control:</label>
                                <select id="serialPortFlowControl" name="serialPortFlowControl" class="form-control">
                                    <option selected="selected">None</option>
                                    <option>XOnXOff</option>
                                    <option>RequestToSend</option>
                                    <option>RequestToSendXOnXOff</option>
                                </select>
                            </td>
                        </tr>
                    </table>


                </div>

            </fieldset>
            <fieldset>
                <legend>Printer Commands</legend>

                <p>
                    Enter the printer's commands you want to send and is supported by the specified printer (ESC/P, PCL, ZPL, EPL, DPL, IPL, EZPL, etc).
                    <br /><br />
                    <b>NOTE:</b> You can use the <b>hex notation of VB or C# for non-printable characters</b> e.g. for Carriage Return (ASCII 13) you could use &H0D or 0x0D
                </p>
                <div class="alert alert-info" style="font-size:11px;">
                    <b>Upload Files</b><br />
                    This online demo does not allow you to upload files. So, if you have a file containing the printer commands like a PRN file, Postscript, PCL, ZPL, etc, then we recommend you to <a href="//neodynamic.com/products/printing/raw-data/aspnet-mvc/download/" target="_blank">download WebClientPrint</a> and test it by using the sample source code included in the package. Feel free to <a href="http://www.neodynamic.com/support" target="_blank">contact our Tech Support</a> for further assistance, help, doubts or questions.
                </div>
                <textarea id="printerCommands" name="printerCommands" rows="10" cols="80" class="form-control" style="min-width: 100%"></textarea>
                
            </fieldset>
            <fieldset>
                <legend>Ready to print!</legend>
                <h3>Your settings were saved! Now it's time to <a href="#" onclick="javascript:doClientPrint();" class="btn btn-lg btn-success">Print</a></h3>
                <br /><br />
            </fieldset>
            <br /><br />
        </div>
    </div>
        <br />
        <br />
        <br />
        <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/MRLWMSC21_SL/mReports/WebClientPrintAPI.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +  "/MRLWMSC21_SL/mReports/DemoPrintCommandsHandler.ashx", HttpContext.Current.Session.SessionID)%>

    </div>--%>

 <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mReports/CurrentStockClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mReports/CurrentStockClientPrintDemo.ashx", "currentStock")%>
 <%--<%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/TransCrate_SL_2020/mReports/CurrentStockClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +  "/TransCrate_SL_2020/mReports/CurrentStockClientPrintDemo.ashx", "currentStock")%>--%>



     </div>
     <script>
         $(function () {
             $('.isvisibleadv').on('click', function () {
                 $('.ishide').slideToggle();
             });
             
                 //$('#btnSearch').on('click', function () {
                 //    //$('.loaderforCurrentStock').css('display', 'block !important');
                 //    alert();
                 //});
            });
       $("#Account").val('<%=this.cp1.AccountID%>');
     </script>

</asp:Content>
