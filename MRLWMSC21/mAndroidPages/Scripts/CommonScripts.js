//Generating Pop-up Print Preview page
function getPrint(print_area) {
    //Creating new page

    var pp = window.open();

    //Adding HTML opening tag with <HEAD> … </HEAD> portion

    pp.document.writeln('<HTML><HEAD><LINK href="ATCDTrackStyle.css"  type="text/css" rel="stylesheet">');
    pp.document.writeln('<LINK href="PrintPreviewStyle.css"  type="text/css" rel="stylesheet"><base target="_self">');
    pp.document.writeln('<LINK href="PrintStyle.css"  type="text/css" rel="stylesheet" media="print"><base target="_self"></HEAD>');
    //Adding Body Tag
    pp.document.writeln('<body MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">');
    //Adding form Tag
    pp.document.writeln('<form  method="post">');
    //Creating two buttons Print and Close within a table
    pp.document.writeln('<TABLE width="730" height="500" align="center" border="0" cellpadding="0" cellspacing="0">');
    //pp.document.writeln('<TR><TD align=smlText><img src="images/Header.jpg" border="0" width="150" height="60"></TD><TD class="smlTextR" valign="bottom"><span class="headingM"></span></TD></TR>');
    pp.document.writeln('<TR ID="PRINT"><TD align=right colspan=2><a ID="PRINT" href="#" class="home" onclick="javascript:location.reload(true);window.print();">Print</a> | <a  ID="CLOSE" href="#" class="home" onclick="window.close();">Close</a></TD></TR>');
    pp.document.writeln('<TR><TD colspan=2>');
    //Writing print area of the calling page
    pp.document.writeln(document.getElementById(print_area).innerHTML);
    //Ending Tag of </form>, </body> and </HTML>

    pp.document.writeln('</TD></TR></TABLE>');


    pp.document.writeln('</form></body></HTML>');

}





//Makes sure that the entered value is a number
// use via onKeyPress="checkNum()" on the textbox

function checkSpecialChar(event) {

    var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
    // alert("The Unicode character code is: " + chCode);
    if (chCode == 62 || chCode == 60 || chCode == 39 || chCode == 37 || chCode == 43 || chCode == 45 || chCode == 64 || chCode == 94 || chCode == 42 || chCode == 96 || chCode == 126 || chCode == 40 || chCode == 33 || chCode == 35 || chCode == 36 || chCode == 38 || chCode == 41)
        return false;
    else
        return true;
}
function checkNumandChar(event) {

    var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
    // alert("The Unicode character code is: " + chCode);
    if ((chCode >= 65 && chCode <= 122 && chCode && !(chCode > 90 && chCode < 97)) || chCode >= 48 && chCode <= 57)
        return true;
    else
        return false;
}




function checkNum(evt) {

    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        status = "This field accepts numbers only."
        return false
    }
    status = "";
    return true;
}

// To Check Decimal Nnumbers //for Currency
function checkDec(textbox, evt) {

    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode == 46) {
        if (textbox.value.indexOf(".") < 0)
            return true;
        else
            return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
        status = "This field accepts numbers only."
        return false
    }
    status = "";
    return true;
}

function disableKey(evt) {
    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    return false;
}
//-->

//Check for Valida date
function checkdate(input) {
    var validformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
    var returnval = false
    if (!validformat.test(input.value))
        alert("Invalid Date Format. Please correct and submit again.")
    else { //Detailed check for valid date ranges
        var dayfield = input.value.split("/")[0]
        var monthfield = input.value.split("/")[1]
        var yearfield = input.value.split("/")[2]
        var dayobj = new Date(yearfield, monthfield - 1, dayfield)
        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
            alert("Invalid Day, Month, or Year range detected. Please correct and submit again.")
        else
            returnval = true
    }
    if (returnval == false) input.select()
    return returnval
}

