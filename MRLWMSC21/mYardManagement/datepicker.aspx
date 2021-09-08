<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="datepicker.aspx.cs" Inherits="MRLWMSC21.mYardManagement.datepicker" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
            <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="Scripts/Datepicker/jquery.datetimepicker.css" rel="stylesheet" />
<%--    <script src="Scripts/Datepicker/jquery.datetimepicker.js"></script>--%>
    <script src="Scripts/Datepicker/jquery.js"></script>
    <script src="Scripts/Datepicker/jquery.datetimepicker.full.js"></script>


    <style type="text/css">

.custom-date-style {
	background-color: red !important;
}

.input{	
}
.input-wide{
	width: 500px;
}

</style>
       <script type="text/javascript">
           $(document).ready(function () {
               $('#default_datetimepicker').datetimepicker({
                   formatTime: 'H:i',
                   formatDate: 'd-m-Y',
                   //defaultDate:'8.12.1986', // it's my birthday
                   defaultDate: '+03-Jan-1970', // it's my birthday
                   defaultTime: '1:00',
                   step:5,
                   timepickerScrollbar: false
               });
           });
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="text" id="default_datetimepicker"/>
        </div>
     
    </form>
</body>
</html>
