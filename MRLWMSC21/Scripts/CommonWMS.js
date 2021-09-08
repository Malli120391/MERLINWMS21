//========================= For Number Validation =========================//
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//========================= END Number Validation =========================//

//========================= Formate For Date (dd-MMM-yyyy) =========================//
function formatDate(timestamp) {
    var x = new Date(timestamp);
    var dd = x.getDate();
    var mm = GetMonthName(x.getMonth() + 1);;
    var yy = x.getFullYear();
    return dd + "-" + mm + "-" + yy;
}
//========================= END Date Formate =========================//

//========================= Formate For Date (dd-MMM-yyyy hh:mm:ss) =========================/
function formatDateTime(timestamp) {
    var x = new Date(timestamp);
    var dd = x.getDate();
    var mm = GetMonthName(x.getMonth() + 1);
    var dt = new Date();
    var yy = x.getFullYear();
    var hr = x.getHours();
    var min = x.getMinutes();
    var sec = x.getSeconds();
    return dd + "-" + mm + "-" + yy + " " + hr + ":" + min + ":" + sec;
}

function GetMonthName(monthNumber) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return months[monthNumber - 1];
}
//========================= END Date Formate =========================//
function getFormattedDate(getDate) {
    //var t_sdate = "6/1/2012";
    if (getDate == "" || getDate == undefined) {
        return "";
    }
    else {
        var sptdate = String(getDate).split("-");
        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var myYear = sptdate[0];
        var myMonth = sptdate[1];
        var myDay = sptdate[2].split('T')[0];
        var combineDatestr = myDay + "-" + months[myMonth - 1] + "-" + myYear;
        return combineDatestr;
    }
}

//========================= Formate For Date (time Stamp To dd-MMM-yyyy) =========================//

function formatDateOnly(timestamp) {
    var x = new Date(timestamp);
    var dd = x.getDate();
    var mm = GetMonthName(x.getMonth() + 1);
    var dt = new Date();
    var yy = x.getFullYear();
    return dd + "-" + mm + "-" + yy;
}

//========================= Formate For Date (time Stamp To dd-MMM-yyyy) =========================//

//========================= Formate For Date (dd-MMM-yyyy hh:mm:ss AM/PM) =========================//
function formatDateTimeAmPm(timestamp) {
    var x = new Date(timestamp);
    var dd = x.getDate();
    var mm = GetMonthNames(x.getMonth() + 1);
    var dt = new Date();
    var yy = x.getFullYear();
    var hr = x.getHours();
    var min = x.getMinutes();
    var sec = x.getSeconds();
    var ampm = hr >= 12 ? 'PM' : 'AM';
    return dd + "-" + mm + "-" + yy + " " + hr + ":" + min + ":" + sec + " " + ampm;
}

function GetMonthNames(monthNumber) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return months[monthNumber - 1];
}

//========================= END Formate For Date (dd-MMM-yyyy hh:mm:ss AM/PM) =========================//


//========================= For Dropdown list filling Common function =========================//
function BindInvDropdown(dt, id) {
    //KeyText, KeyValue
    if (dt != null && dt != '') {
        $('#' + id).empty();
        $("#" + id).append($("<option></option>").val(0).html("Please Select"));
        for (var x = 0; x < dt.length ; x++) {
            $("#" + id).append($("<option></option>").val(dt[x].KeyValue).html(dt[x].KeyText));
        }
    }
    else {
        $('#' + id).empty();
        $("#" + id).append($("<option></option>").val(0).html("Please Select"));
    }
}

//========================= END Dropdown list filling Common function =========================//

//========================= Duplicate Checking function =========================//


function CheckDuplicate(obj, field, value, PK) {
    var status = true;
    var item = obj, Count = 0;

    if ($('#' + PK).val() != 0) {
        item = $.grep(obj, function (data) {
            return data[PK] != $('#' + PK).val();
        });
    }
    Count = $.grep(item, function (data) {
        return data[field] == value || data[field].toUpperCase() == value.toUpperCase() || data[field].toLowerCase() == value.toLowerCase();
    });

    if (Count.length != 0) {
        status = false;
    }
    return status;
}

//========================= END Duplicate Checking function =========================//