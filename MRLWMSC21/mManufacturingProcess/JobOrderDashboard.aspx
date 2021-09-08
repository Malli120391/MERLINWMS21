<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobOrderDashboard.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.JobOrderDashboard" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Job Order Dashboard</title>
      <link href="../Images/RT_Icon.ico" rel="shortcut icon" type="image/x-icon" />
    <%-- <link href="../Images/inventraxIco.ico" rel="shortcut icon" type="image/x-icon" />--%>
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.24.js"></script>
    <script src="../mMaterialManagement/jQuery2/jquery.blockUI.js"></script>

    <style type="text/css">
         .cssOperation {
             font-family:Calibri;
              font-size:12pt;
              color:black;
         }

         table {
              border-collapse:collapse;
              padding:5px;
              
         }
         .cssOperation td, th {
             border: 1px solid black;
             padding:5px;
         }
         .cssOperation  th {
             background-color:black;
             color:white;
             font-weight:bold;
         }
         #MessageDialog  {

             background-color:#F3F781;
             overflow:auto;
             width:300px;
             padding:10px;
             border-radius:8px;
             
             box-shadow:5px 8px 10px #808080;
         }
          .dvSubHeading {
              font-family:Arial Black;
              font-size:12pt;
              font-weight:bold;
              color:black;
              text-align:center;
          }

          .dvErrMsg {

              font-family:Calibri;
              font-size:10pt;
              font-weight:bold;
              color:#FF0303;
              text-align:center;
          }

     </style>

    <script type="text/javascript">
        function unblockAycDialog() {
            $.unblockUI();
        }

        function showAycBlock() {

            $.blockUI({ message: '<h3> Just a moment...</h3>' });
        }

        var DashboardData;
        var MainCanvas;
        var JobOrdersInformation = new Array();
        var MainContext;
        var endOfLine = 0;
        var flagImg;
        var Defic_Img;
        $(document).ready(function () {
            MainCanvas = document.getElementById('JobOrderDashboard');
            MainContext = MainCanvas.getContext("2d");
            flagImg = document.createElement('img');
            flagImg.src = "../Images/blue_menu_icons/Redflag-16.png";
            flagImg.width = 10;
            flagImg.height = 10;

            Defic_Img  = document.createElement('img');
            Defic_Img.src = "../Images/blue_menu_icons/Mat_Def_16.png";
            Defic_Img.width = 10;
            Defic_Img.height = 10;
            showAycBlock();
            $.ajax({
                url: '<%# ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDashboardData") %>',
                data: "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    DashboardData = data.d.toString();
                    if (DashboardData == '') {
                        alert('No inprocess \'Job Order\' is available');
                    }
                    else {
                        BuildDashboardData();
                    }
                    unblockAycDialog();
                    
                    //testgradiant();
                }
                //},
                //error: function (response) {
                //    alert(response.responseText);
                //},
                //failure: function (response) {
                //    alert(response.responseText);
                //}
                
            });
        });

        function BuildDashboardData() {
            //Job order List            
            var JobList = DashboardData.split('þ');
            JobCount = JobList.length;
                        
            SetHeightWidth();

            DashboardData = null;
            //set height based on list of job orders
            var width = MainCanvas.width;

            MainContext.fillStyle = "#808080";
            MainContext.font = "bold 9pt Arial Black";
            MainContext.fillRect(width-200, 65, 30, 10);
            MainContext.stroke();
            MainContext.fillText('Not Started', width - 150, 75);
            MainContext.stroke();

            MainContext.fillStyle = "#FFA500";
            MainContext.font = "bold 9pt Arial Black";
            MainContext.fillRect(width - 200, 80, 30, 10);
            MainContext.stroke();
            MainContext.fillText('In Process', width - 150, 90);
            MainContext.stroke();

            MainContext.fillStyle = "#008000";
            MainContext.font = "bold 9pt Arial Black";
            MainContext.fillRect(width - 200, 95, 30, 10);
            MainContext.stroke();
            MainContext.fillText('Completed', width - 150, 105);
            MainContext.stroke();

            MainContext.fillStyle = "#ff0000";
            MainContext.font = "bold 9pt Arial Black";
            MainContext.fillRect(width - 200, 110, 30, 10);
            MainContext.stroke();
            MainContext.fillText('Failure', width - 150, 120);
            MainContext.stroke();

           
           // MainCanvas.height = JobCount * 180 + 80;
            
            var JobOrderDetails;
            var OperationList;

            //initial Position
            var X_Position = 50;
            var Y_Position = 40;

            var OPerationCount;
            var OperationDetails;
            var ActivityList;
            var ActivityCount;
            var ActivityDetails;

            MainContext.fillStyle = "#4B7203";
            MainContext.font = "bold 25pt Arial Black";
            MainContext.fillText('Work In Progress', screen.width/2-150, 50);

            MainContext.fillStyle = "#F5A004";
            MainContext.font = "bold 13pt Calibri";

            var today = new Date();
            MainContext.fillText('[' + (today.getDate() < 10 ? '0' + today.getDate() : '' + today.getDate()) + '/' + (today.getMonth() + 1 < 10 ? '0' + (today.getMonth() + 1) : ''+(today.getMonth() + 1)) + '/' + today.getFullYear() + ']', screen.width / 2-2, 76);

            // Job order Details
            for (jobIndex = 0; jobIndex < JobCount; jobIndex++) {

                //reset Y Coordinate into initial
                Y_Position = Y_Position + 220;
                X_Position = 20;
                
                //Job oder refno.,kitcode,operationdetails
                JobOrderDetails = JobList[jobIndex].split('*');

                //draw Job Order Details
                drawHeaderName(Y_Position, JobOrderDetails[0],parseInt(JobOrderDetails[2]),parseInt(JobOrderDetails[3]),JobOrderDetails[4]);
                
                //Operation List for Job Order
                OperationList = JobOrderDetails[1].split('©');

                OPerationCount=OperationList.length;
                for (operationindex = 0; operationindex < OPerationCount; operationindex++) {

                    //list Of Operation for the job
                    OperationDetails = OperationList[operationindex].split('Ñ');

                    //increase x coordinate to next step
                    X_Position = X_Position + 33;

                    //draw Operation 

                    drawOPerations(X_Position, Y_Position, OperationDetails[0], OperationDetails[2], OperationDetails[3]);
                   
                    //list of Activities
                    ActivityList = OperationDetails[1].split('§');

                    //count of Activities
                    ActivityCount = ActivityList.length;
                    for (activityindex = 0; activityindex < ActivityCount; activityindex++) {

                        //Activity Details
                        ActivityDetails = ActivityList[activityindex].split('Ø');
                        
                        //increase x coordinate to next step
                        X_Position = X_Position + 28;

                        //Draw Activity
                        //          X_position, Y_position, ActivityName,       OP_STATUS, SUP_STATUS, QC_STATUS, CQC_STATUS, IsFailed,isPosiFlag,ActivityDetails,PositiveRecallDetails
                        drawActivity(X_Position, Y_Position, ActivityDetails[0], ActivityDetails[1], ActivityDetails[2], ActivityDetails[3], ActivityDetails[4], ActivityDetails[5], ActivityDetails[6], ActivityDetails[7], ActivityDetails[8], ActivityDetails[9], ActivityDetails[10])

                    }
                }

                //draw Line for job
                drawLineForJob(X_Position, Y_Position);
            }
        }

        function testgradiant() {
            

            /*var grdcol = MainContext.createLinearGradient(0, 0, 0, 170);
            grdcol.addColorStop(0, "#F8A710");
            grdcol.addColorStop(1, "#170F01");

            MainContext.fillStyle = grdcol;
            MainContext.fillRect(20, 20, 10, 100);
            */
            //var gradiantCol = MainContext.createLinearGradient(0, 0, 200, 0);
            //gradiantCol.addColorStop(0, "#F8A710");
            //gradiantCol.addColorStop(1, "#170F01");
            //MainContext.fillStyle = gradiantCol;'Ž'
            MainContext.fillStyle = "#170F01";

            MainContext.fillRect(200, 200, 200, 200);
        }
    </script>

    <script>

        function drawActivity(X_position, Y_position, ActivityName, OP_STATUS, SUP_STATUS, QC_STATUS, CQC_STATUS, IsFailed,isPosiFlag,WorkCenterDetails,ActivityDetails,PositiveRecallDetails,NCDetails) {
            var isRequire = true;
            var LenthofActivity = 20;

            
            CheckActivityStatusWithIsfail(OP_STATUS, IsFailed, X_position, Y_position - 20)
            
            
            isRequire = CheckActivityStatus(SUP_STATUS, X_position, Y_position - 41)
            

            //if (isRequire) {
                LenthofActivity += 21;
                isRequire = CheckActivityStatus(QC_STATUS, X_position, Y_position - 62)

                //if (isRequire) {
                    LenthofActivity += 21;
                    isRequire = CheckActivityStatus(CQC_STATUS, X_position, Y_position - 83)

                    //if (isRequire) {
                        LenthofActivity += 21;
            //        }
            //    }
            //}
            
            JobOrdersInformation.push({ init_x: X_position+7, init_y: Y_position - LenthofActivity, final_x: X_position + 17, final_y: Y_position, Datatype: 3, Data: ActivityDetails });

            
            MainContext.fillStyle = "#000000";
            MainContext.font = "bold 8pt Calibri";
            MainContext.fillText(ActivityName, X_position, Y_position + 15);
            
            //var gradiantCol = MainContext.createLinearGradient(0, 0, LenthofActivity, LenthofActivity);
            //gradiantCol.addColorStop(0, WorkCenterDetails.split('^')[2].split('-')[0]);
            //gradiantCol.addColorStop(1, WorkCenterDetails.split('^')[2].split('-')[1]);
            //MainContext.fillStyle = gradiantCol;
            
            var grdcol = MainContext.createLinearGradient(0, 0, 0, LenthofActivity);
            grdcol.addColorStop(0, WorkCenterDetails.split('Ð')[2].split('-')[0]);
            grdcol.addColorStop(1, WorkCenterDetails.split('Ð')[2].split('-')[1]);
            
            MainContext.fillStyle = grdcol;
            
            MainContext.fillRect(X_position - 5, Y_position - LenthofActivity, 5, LenthofActivity);
            //MainContext.fillRect(X_position - 3, Y_position - LenthofActivity, 3, LenthofActivity);
            JobOrdersInformation.push({ init_x: X_position + 2, init_y: Y_position -  LenthofActivity, final_x: X_position + 7, final_y: Y_position, Datatype: 5, Data: WorkCenterDetails });
            
            if (isPosiFlag == '1') {
                MainContext.drawImage(flagImg, X_position + 11, Y_position - 10, 10, 10);
                JobOrdersInformation.push({ init_x: X_position +17, init_y: Y_position-10, final_x: X_position + 27, final_y: Y_position, Datatype: 4, Data: PositiveRecallDetails });
            }

            if (NCDetails != "¬") {
                
               /* MainContext.beginPath();
                MainContext.arc(X_position, Y_position - (LenthofActivity + 10), 4, 0, 2 * Math.PI);
                MainContext.fillStyle = "#FF0000";
                MainContext.fill();
                */
                MainContext.beginPath();
                MainContext.strokeStyle = "red";
                MainContext.arc(X_position+5, Y_position - (LenthofActivity + 10), 4, 0, 2 * Math.PI);
                MainContext.fillStyle = "red";
                MainContext.fill();
                //MainContext.stroke();
                JobOrdersInformation.push({ init_x: X_position + 7, init_y: Y_position - (LenthofActivity + 15), final_x: X_position + 20, final_y: Y_position - (LenthofActivity + 2), Datatype: 6, Data: NCDetails });
            }
        }

        function drawOPerations(X_position, Y_position, OperatioName, Status,OperationDetails) {
            if (Status == '0') {
                MainContext.fillStyle = "#808080";//gray color
            }
            else if (Status == '1') {
                MainContext.fillStyle = "#FFA500";//orange color
            }
            else if (Status == '2') {
                MainContext.fillStyle = "#008000";//green color
            }
            else
                MainContext.fillStyle = "#ff0060";
            
            //MainContext.fillStyle = "#ff0000";
            MainContext.fillRect(X_position,Y_position-110, 12, 110);
            MainContext.stroke();

            JobOrdersInformation.push({ init_x: X_position+7, init_y: Y_position - 110, final_x: X_position + 19, final_y: Y_position, Datatype: 2, Data: OperationDetails });

            MainContext.fillStyle = "#000000";
            MainContext.font = "bold 9pt Calibri";
            MainContext.fillText(OperatioName, X_position, Y_position + 15);
        }

        function drawLineForJob(X_position, Y_position) {
            MainContext.fillStyle = "#870078";
            MainContext.fillRect(53, Y_position, X_position - 43, 5);
            MainContext.stroke();
        }

        function drawHeaderName(Y_position,JobDetails,ActivityCount,OperationCount,MaterialDeficiancyDetails) {
            
            JobInfo = JobDetails.split('Ð');

            MainContext.fillStyle = "#FF8000";
            MainContext.font = "bold 16pt Calibri";
            MainContext.fillText(JobInfo[1], 50, Y_position - 135);

            MainContext.fillStyle = "#0040FF";
            MainContext.font = "bold 9pt Calibri";

            MainContext.fillText('[' + JobInfo[2] + ']', 210, Y_position - 135);

            MainContext.fillStyle = "#086A87";
            MainContext.font = "bold 10pt Calibri";
            MainContext.fillText(JobInfo[0], 50, Y_position - 120);
            JobOrdersInformation.push({ init_x: 50, init_y: Y_position - 150, final_x: 300, final_y: Y_position - 120, Datatype: 1, Data: JobDetails });

            MainContext.fillStyle = "#2E2EFE";
            MainContext.font = "bold 9pt Calibri";
            MainContext.fillText('Plan St. Dt. ' + JobInfo[3], 50, Y_position + 32);

            MainContext.fillStyle = "#2E2EFE";
            MainContext.font = "bold 9pt Calibri";
            if (((ActivityCount * 28) + (OperationCount * 33)) > 610) {
                MainContext.fillText('Plan. Due Dt.' + JobInfo[4], ((ActivityCount * 28) + (OperationCount * 33)) - 232, Y_position + 32);
            }
            else {
                MainContext.fillText('Plan. Due Dt. ' + JobInfo[4], 335, Y_position + 32);
            }

            if (JobInfo[7] != "") {
                MainContext.fillStyle = "#610B0B";
                MainContext.font = "bold 9pt Calibri";
                MainContext.fillText('| Act. St. Dt. '+JobInfo[7], 155, Y_position + 32);

                MainContext.fillStyle = "#610B0B";
                MainContext.font = "bold 9pt Calibri";
                if (((ActivityCount * 28) + (OperationCount * 33)) > 610) {
                    //MainContext.fillText('| Act. Due Dt. ' + JobInfo[8], ((ActivityCount * 28) + (OperationCount * 33)) - 145, Y_position + 32);
                    MainContext.fillText('| Act. Due Dt. ' + JobInfo[8], ((ActivityCount * 28) + (OperationCount * 33)) - 117, Y_position + 32);
                }
                else {
                    MainContext.fillText('| Act. Due Dt. ' + JobInfo[8], 452, Y_position + 32);
                }

                MainContext.font = "bold 10pt Calibri";
                MainContext.fillStyle = "#000000";
                MainContext.fillText('Time Elapsed since Act. St. Dt.', ((ActivityCount * 28) + (OperationCount * 33)) + 65, Y_position - 74);

                if (JobInfo[11] == '0') {                  //due date is not over condition
                    MainContext.fillStyle = "#000000";
                }//  (green color)
                else if (JobInfo[11] == '1') {       //due date is over condition

                    MainContext.font = "bold 10pt Calibri";
                    MainContext.fillStyle = "#000000";
                    MainContext.fillText('Time Elapsed since Plan. Due Dt.', ((ActivityCount * 28) + (OperationCount * 33)) + 65, Y_position - 34);
                    MainContext.fillStyle = "#F70505";

                    MainContext.font = "bold 13pt Calibri";
                    MainContext.fillText(JobInfo[10], ((ActivityCount * 28) + (OperationCount * 33)) +65, Y_position - 14);

                }// red color
                MainContext.font = "bold 13pt Calibri";
                MainContext.fillText(JobInfo[9], ((ActivityCount * 28) + (OperationCount * 33)) + 65, Y_position - 54);

                
                //if (MaterialDeficiancyDetails != "")
                //{

                //    MainContext.drawImage(Defic_Img, 320, Y_position - 145, 16, 16);
                //    JobOrdersInformation.push({ init_x: 328, init_y: Y_position - 145, final_x: 343, final_y: Y_position - 129, Datatype: 7, Data: MaterialDeficiancyDetails });
                   
                //}


            }
            if (MaterialDeficiancyDetails != "") {

                MainContext.drawImage(Defic_Img, 320, Y_position - 145, 16, 16);
                JobOrdersInformation.push({ init_x: 328, init_y: Y_position - 145, final_x: 343, final_y: Y_position - 129, Datatype: 7, Data: MaterialDeficiancyDetails });

            }
        }

        function SetHeightWidth() {

            var jobOrderList = DashboardData.split('þ');
            var JobOrdercount = jobOrderList.length;
            var JobDetails;
            var MaxWidth = 0;
            var ActivitiesCount = 0;
            var OperationCount = 0;
           

            for (JobIndex = 0; JobIndex < JobOrdercount; JobIndex++) {
                JobDetails = jobOrderList[JobIndex].split('*');
                ActivitiesCount = parseInt(JobDetails[2]);
                OperationCount = parseInt(JobDetails[3]);

                if (((ActivitiesCount * 28) + (OperationCount * 33)) > MaxWidth) {
                    MaxWidth = (ActivitiesCount * 28) + (OperationCount * 33);
                }
            }
           
            if (screen.height > JobOrdercount * 220 + 120)
                MainCanvas.height = screen.height;
            else
                MainCanvas.height = JobOrdercount * 220 + 120;

            if (screen.width > MaxWidth + 240)
                MainCanvas.width = screen.width;
            else
                MainCanvas.width = MaxWidth+ 240;
        }

        function CheckActivityStatus(Status, X_position, Y_position) {
            var isRequire = true;
            if (Status == '0') {
                MainContext.fillStyle = "#808080";//gray color
            }
            else if (Status == '1') {
                MainContext.fillStyle = "#FFA500";//orange color
            }
            else if (Status == '2') {
                MainContext.fillStyle = "#008000";//green color
            }
            else
            {
                MainContext.fillStyle = "#ff0060";
            }

            if (Status == '-1') {
                MainContext.fillStyle = "#cacaca";
                isRequire = false;
            }
            
            //if (isRequire)
            {
                
                MainContext.fillRect(X_position, Y_position, 10, 20);
                MainContext.stroke();
            }
            return isRequire;
        }
        
        function CheckActivityStatusWithIsfail(Status, IsFailed, X_position, Y_position) {
            if (Status == '0') {
                MainContext.fillStyle = "#808080";//gray color
            }
            else if (Status == '1') {
                MainContext.fillStyle = "#FFA500";//orange color
            }
            else if (Status == '2' && IsFailed == '0') {
                MainContext.fillStyle = "#008000";
            }
            else if (Status == '2' && IsFailed == '1') {
                MainContext.fillStyle = "#ff0000";
            }
            else
                MainContext.fillStyle = "#ff0060";
           
            //MainContext.fillStyle = "#ff0060";
            MainContext.fillRect(X_position, Y_position, 10, 20);
            MainContext.stroke();
        }

    </script>

    <script>

        function CanvasMouseMove(event) {
            
            var dialog = document.getElementById('MessageDialog');
            var page_x = event.pageX;
            var page_y = event.pageY;

            var clint_x = event.clientX;
            var client_y = event.clientY;
            dialog.style.left = (page_x ) + 'px';
             dialog.style.top = (page_y ) + 'px';

            //dialog.style.left = (clint_x) + 'px';
            //dialog.style.top = (client_y) + 'px';
            MainCanvas.style.cursor = 'default';
            var xposition = event.pageX;
            var yposition = event.pageY;
            dialog.style.display = "none";
            JobOrdersInformation.forEach(function (element)
            {
               
                if (element.init_x < xposition && element.final_x > xposition && element.init_y < yposition && element.final_y > yposition)
                {
                    //alert(element.Datatype);
                    MainCanvas.style.cursor = 'pointer';
                    if (element.Datatype == 4) {
                        dialog.style.width = 400 + 'px';
                        dialog.style.height = 250 + 'px';
                        if (xposition + 420 > MainCanvas.width) {
                            dialog.style.left = (xposition - 420) + 'px';
                        }
                        if (yposition + 250 > MainCanvas.height) {
                            dialog.style.top = (yposition - 250) + 'px';
                        }
                        dialog.innerHTML = SetFlagDatatoDialog(element.Data);
                    }
                    else if (element.Datatype == 3) {
                        //alert(element.Data);
                        dialog.style.width = 400 + 'px';
                        dialog.style.height = 300 + 'px';
                        if (xposition + 419 > MainCanvas.width) {
                            dialog.style.left = (xposition - 419) + 'px';
                        }
                        if (yposition + 319 > MainCanvas.height) {
                            dialog.style.top = (yposition - 319) + 'px';
                        }
                        dialog.innerHTML = SetActivityDetailsDailog(element.Data);
                    }
                    else if (element.Datatype == 1) {
                        dialog.style.width = 350 + 'px';
                        dialog.style.height = 220 + 'px';
                        if (xposition + 369 > MainCanvas.width) {
                            dialog.style.left = (xposition - 369) + 'px';
                        }
                        if (yposition + 239 > MainCanvas.height) {
                            dialog.style.top = (yposition - 239) + 'px';
                        }
                        dialog.innerHTML = setHeaderDetailsDialog(element.Data);
                    }
                    else if (element.Datatype == 2) {
                        dialog.style.width = 300 + 'px';
                        dialog.style.height = 150 + 'px';
                        if (xposition + 319 > MainCanvas.width) {
                            dialog.style.left = (xposition - 319) + 'px';
                        }
                        if (yposition + 169 > MainCanvas.height) {
                            dialog.style.top = (yposition - 169) + 'px';
                        }
                        dialog.innerHTML= setOperationDetailsDailog(element.Data);
                    }
                    else if (element.Datatype == 5) {
                        
                        dialog.style.width = 300 + 'px';
                        dialog.style.height = 150 + 'px';
                        if (xposition + 319 > MainCanvas.width) {
                            dialog.style.left = (xposition - 319) + 'px';
                        }
                        if (yposition + 169 > MainCanvas.height) {
                            dialog.style.top = (yposition - 169) + 'px';
                        }
                        dialog.innerHTML= setWorkCenterDetails(element.Data);
                    }
                    else if (element.Datatype == 6) {

                        dialog.style.width = 420 + 'px';
                        dialog.style.height = 250 + 'px';
                        if (xposition + 399 > MainCanvas.width) {
                            dialog.style.left = (xposition - 399) + 'px';
                        }
                        if (yposition + 269 > MainCanvas.height) {
                            dialog.style.top = (yposition - 269) + 'px';
                        }
                        dialog.innerHTML = setNCDeteilsDailog(element.Data);
                    }
                    else if (element.Datatype == 7) {
                        dialog.style.width = 380 + 'px';
                        dialog.style.height = 250 + 'px';
                        if (xposition + 399 > MainCanvas.width) {
                            dialog.style.left = (xposition - 399) + 'px';
                        }
                        if (yposition + 269 > MainCanvas.height) {
                            dialog.style.top = (yposition - 269) + 'px';
                        }
                        
                        dialog.innerHTML = setMat_Def_Dailog(element.Data);
                    }
                    else
                        dialog.innerHTML = '';
                    dialog.style.display = "block";
              }
            });
                   

        }

        function SetFlagDatatoDialog(FlagInformation) {
            var table = '';//'<center><span class=\"dvSubHeading\" align=\"center\">Positive Recall Details</span></center><br/>';
            
            if (FlagInformation != "") {
                table += '<table width=\"300px\" class=\"cssOperation\">';
                var MaterialDetails = FlagInformation.split('Ð');
                var MaterialCount = MaterialDetails.length;
                var Material;
                table += '<tr><th>Part Number</th><th><nobr>Batch No.</nobr></th><th><nobr>Recall ON</nobr></th><th><nobr>Recall OFF</nobr></th></tr>';
                for (MaterialIndex = 0; MaterialIndex < MaterialCount; MaterialIndex++) {
                    Material = MaterialDetails[MaterialIndex].split('®');
                    table += '<tr><td>' + Material[0] + '</td><td>' + Material[5] + '</td><td>' + Material[1] + '<br/><span style=\"font-size:9pt;\">' + Material[3] + '</span></td><td>' + Material[2] + '<br/><span style=\"font-size:9pt;\">' + Material[4] + '</span></td></tr>';
                }
                table += '</table>';
            }
            else {
                table += '<br/><span class=\"dvErrMsg\">No material is configure to this Recall</span>';
            }
            FlagInformation = null;
            MaterialDetails = null;
            MaterialCount = null;
            Material = null;
            return table;
        }

        function SetActivityDetailsDailog(ActivityDetails) {
            //alert(ActivityDetails);
            var InnerHTML = '<center><table><tr><td>';// '<center><span class=\"dvSubHeading\" align=\"center\">Activity Details</span></center><br/>';

            if (ActivityDetails != "") {

                ActivityInto = ActivityDetails.split('¬');
                //alert(ActivityInto[0]);
                ActivityHeaderData = ActivityInto[0].split('Ð');
                InnerHTML += '<table width=\"350px\" class=\"cssOperation\"><tr><th>Activity No.</th><th>Description</th></tr><tr><td>' + ActivityHeaderData[1] + '</td><td>' + ActivityHeaderData[0] + '</td></tr></table>';

                //alert('capture   '+ActivityInto[1]);
                if (ActivityInto[1] != "") {
                    InnerHTML += '<br/><table width=\"350px\" class=\"cssOperation\">';
                    var CapturingDetails = ActivityInto[1].split('Ð');
                    var CaptureCount = CapturingDetails.length;
                    var CapturingInfo;
                    InnerHTML += '<tr><th>Start Date</th><th>Stop Date</th><th>User</th><th>Role</th></tr>';
                    for (CaptureIndex = 0; CaptureIndex < CaptureCount; CaptureIndex++) {
                        CapturingInfo = CapturingDetails[CaptureIndex].split('®');

                        InnerHTML += '<tr><td>' + CapturingInfo[0] + '</td><td>' + CapturingInfo[1] + '</td><td>' + CapturingInfo[2] + '</td><td>' + CapturingInfo[3] + '</td></tr>';
                    }
                    InnerHTML += '</table>';
                }
                else {
                    //InnerHTML += ' <span class=\"dvErrMsg\">No Activity WorkStation Details</span><br/>';
                    InnerHTML += '';// ' <span class=\"dvErrMsg\">No Workstation is configured to this \'Activity\'</span><br/>';
                }
                //alert('Mateirl'+ActivityInto[2]);
                if (ActivityInto[2] != "") {
                    InnerHTML += '<br/><table width=\"350px\" class=\"cssOperation\">';
                    var MaterialDetails = ActivityInto[2].split('Ð');
                    var MaterialCount = MaterialDetails.length;
                    var Material;
                    var MaterialReceivelist;
                    var MaterialReceiveListdetails;
                    InnerHTML += '<tr><th  >Part Number</th><th>Required Qty.</th><th>Batch No.</th><th>Consumed Qty.</th></tr>';
                    for (MaterialIndex = 0; MaterialIndex < MaterialCount; MaterialIndex++) {
                        Material = MaterialDetails[MaterialIndex].split('®');
                        MaterialReceivelist = Material[2].split('Œ');
                        if (Material[2] == '') {
                            InnerHTML += '<tr><td>' + Material[0] + '</td><td  align=\"center\">' + Material[1] + '</td><td  align=\"center\"></td><td  align=\"center\"></td></tr>';
                        }
                        else {
                            for (ReceiveMaterialindex = 0; ReceiveMaterialindex < MaterialReceivelist.length; ReceiveMaterialindex++) {
                                MaterialReceiveListdetails = MaterialReceivelist[ReceiveMaterialindex].split('Ž');
                                if (ReceiveMaterialindex == 0) {
                                    InnerHTML += '<tr><td  rowspan=\"' + MaterialReceivelist.length + ' \">' + Material[0] + '</td><td  align=\"center\"  rowspan=\"' + MaterialReceivelist.length + ' \">' + Material[1] + '</td><td  align=\"center\">' + MaterialReceiveListdetails[0] + '</td><td  align=\"center\">' + MaterialReceiveListdetails[1] + '</td></tr>';
                                }
                                else {
                                    InnerHTML += '<tr><td  align=\"center\">' + MaterialReceiveListdetails[0] + '</td><td  align=\"center\">' + MaterialReceiveListdetails[1] + '</td></tr>';
                                }
                            }
                        }
                    }
                    InnerHTML += '</table><br/>';
                }
                else {
                    InnerHTML += ' <span class=\"dvErrMsg\">No material is configured to this \'Activity\'</span><br/>';
                }

               /* if (ActivityInto[3] != "") {
                    InnerHTML += '<br/><table width=\"350px\" class=\"cssOperation\">';
                    var NCList = ActivityInto[3].split('Œ');
                    var NCCount = NCList.length;
                    var NCDetails;
                    var NCMaterialDetails;
                    var MaterialCount;
                    var NCMaterial;
                    InnerHTML += '<tr><th  >NC Ref. No.</th><th >Part Number</th><th>Qty.</th></tr>';
                    for (NCIndex = 0; NCIndex < NCCount; NCIndex++) {
                        NCDetails = NCList[NCIndex].split('Ž');
                        NCMaterialDetails = NCDetails[1].split('Ÿ');
                        MaterialCount = NCMaterialDetails.length;
                        InnerHTML += '<tr><td rowspan=\"' + MaterialCount + '\">' + NCDetails[0] + '</td>'
                        for (NCMaterialIndex = 0; NCMaterialIndex < MaterialCount; NCMaterialIndex++) {
                            NCMaterial = NCMaterialDetails[NCMaterialIndex].split('š');
                            if (NCMaterialIndex == 0) {
                                InnerHTML += '<td>' + NCMaterial[0] + '</td><td  align=\"center\">' + NCMaterial[1] + '</td></tr>';
                            }
                            else
                                InnerHTML += '<tr><td>' + NCMaterial[0] + '</td><td  align=\"center\">' + NCMaterial[1] + '</td></tr>';
                        }                        
                    }
                    InnerHTML += '</table>';
                }
                else {
                    InnerHTML += ' <span class=\"dvErrMsg\">No NC materials to this Activity</span>';
                }
                */
            }
            MaterialDetails = null;
            MaterialCount = null;
            Material = null;
            CapturingDetails = null;
            CaptureCount = null;
            CapturingInfo = null;
            MaterialReceivelist=null
            MaterialReceiveListdetails = null;
            return InnerHTML+'<td/><tr/><table/><center/>';
        }

        function setHeaderDetailsDialog(JobOrderDetails) {
            var table = '';
            if (JobOrderDetails != "") {
                table = '<table width=\"340px\" class=\"cssOperation\">';// '<center><span class=\"dvSubHeading\" align=\"center\">Job Order Details</span></center><br/><table width=\"340px\" class=\"cssOperation\">';
                var JobInfo = JobOrderDetails.split('Ð');
                var JobDetailsCount = JobInfo.length;
                table += '<tr><td>Job Ref. No.</td><td>' + JobInfo [0]+ '</td></tr>';
                table += '<tr><td>OEM Part Number</td><td>' + JobInfo[1] + '</td></tr>';
                table += '<tr><td>Kit Code</td><td>' + JobInfo[2] + '</td></tr>';
                table += '<tr><td>Start Date</td><td>' + JobInfo[3] + '</td></tr>';
                table += '<tr><td>Due Date</td><td>' + JobInfo[4] + '</td></tr>';
                table += '<tr><td>Release Date</td><td>' + JobInfo[5] + '</td></tr>';
                table += '<tr><td>Released By</td><td>' + JobInfo[6] + '</td></tr>';

                table += '</table>';
            }
            JobOrderDetails = null;
            JobInfo = null;
            JobDetailsCount = null;
            return table;
        }

        function setOperationDetailsDailog(OperationData) {
            var table = '';// '<center><span class=\"dvSubHeading\" align=\"center\">Operation Details</span></center>';

            if (OperationData != "") {
                table += '<center><table class=\"cssOperation\">';
                var OperationDetails = OperationData.split('®');
                
                table += '<tr><th valign=\'middle\'>Opr.#</th><th valign=\'middle\'>Operation Name</th></tr>';
                table += '<tr><td valign=\'middle\'>' + OperationDetails[1] + '</td><td valign=\'middle\'>' + OperationDetails[0] + '</td></tr>';
                table += '</table><center>';
            }
            else {
                table += '<br/> <span class=\"dvErrMsg\">No Details</span>';
            }
            OperationData = null;
            OperationDetails = null;
            return table;
        }

        function setWorkCenterDetails(WorkCenterDetails) {
            var table = '';// '<center><span class=\"dvSubHeading\" align=\"center\">Workstation Details</span></center><br/>';
            if (WorkCenterDetails != "") {
                table += '<center><table class=\"cssOperation\"><tr><th>Workstation</th><tr><td>' + WorkCenterDetails.split('Ð')[0] + "</td></table></center>";
            }
            else {
                table += '<br/><span class=\"dvErrMsg\">No Details</span>';
            }
            WorkCenterDetails = null;
            return table;
        }

        function setNCDeteilsDailog(NCDetails) {
            var NCInfo = NCDetails.split('¬');
            var InnerHTML = '<center>';

            if (NCInfo[0] != "") {
                NCCaptureDetails = NCInfo[0].split('Œ');
                NCCaptureCount = NCCaptureDetails.length;
                var NCCaptureInfo;
                InnerHTML += '<table width=\"350px\" class=\"cssOperation\">';
                InnerHTML += '<tr><th>NC No.</th><th>Remarks</th><th>Raised By</th><th>Raised On</th><th>User Role</th></tr>';
                for (NCCaptureIndex = 0; NCCaptureIndex < NCCaptureCount; NCCaptureIndex++) {
                    NCCaptureInfo = NCCaptureDetails[NCCaptureIndex].split('Ž');
                    InnerHTML += '<tr><td>' + NCCaptureInfo[0] + '</td><td>' + NCCaptureInfo[3] + '</td><td>' + NCCaptureInfo[1] + '</td><td>' + NCCaptureInfo[2] + '</td><td>' + NCCaptureInfo[4] + '</td></tr>';
                }
                InnerHTML += '</table><br/>';
            }
            else {
                InnerHTML += '<span class=\"dvErrMsg\">No NC Capturing Details to this \'Activity\'</span><br/>';
            }



            if (NCInfo[1] != "") {
                InnerHTML += '<br/><table width=\"400px\" class=\"cssOperation\">';
                var NCList = NCInfo[1].split('Œ');
                var NCCount = NCList.length;
                var NCDetails;
                var NCMaterialDetails;
                var MaterialCount;
                var NCMaterial;
                InnerHTML += '<tr><th  >NC Ref. No.</th><th >Part Number</th><th>Batch No.</th><th>Qty.</th></tr>';
                for (NCIndex = 0; NCIndex < NCCount; NCIndex++) {
                    NCDetails = NCList[NCIndex].split('Ž');

                    if (NCDetails[1] == '') {
                        InnerHTML += '<tr><td>' + NCDetails[0] + '</td><td></td><td  align=\"center\"></td><td  align=\"center\"></td></tr>';
                    }
                    else {

                        NCMaterialDetails = NCDetails[1].split('Ÿ');
                        MaterialCount = NCMaterialDetails.length;
                        //InnerHTML += '<tr><td rowspan=\"' + MaterialCount + '\">' + NCDetails[0] + '</td>'
                        for (NCMaterialIndex = 0; NCMaterialIndex < MaterialCount; NCMaterialIndex++) {
                            NCMaterial = NCMaterialDetails[NCMaterialIndex].split('š');
                            if (NCMaterialIndex == 0) {
                                InnerHTML += '<tr><td rowspan=\"' + MaterialCount + '\">' + NCDetails[0] + '</td><td>' + NCMaterial[0] + '</td><td  align=\"center\">' + NCMaterial[2] + '</td><td  align=\"center\">' + NCMaterial[1] + '</td></tr>';
                            }
                            else
                                InnerHTML += '<tr><td>' + NCMaterial[0] + '</td><td  align=\"center\">' + NCMaterial[2] + '</td><td  align=\"center\">' + NCMaterial[1] + '</td></tr>';
                        }
                    }
                }
                InnerHTML += '</table>';
            }
            else {
                InnerHTML += ' <span class=\"dvErrMsg\">No NC material is configured to this \'Activity\'</span>';
            }
            NCInfo = null;
            NCDetails = null;
            NCCaptureDetails = null;
            NCCaptureCount = null;
            NCList = null;
            NCCount = null;
            NCDetails = null;
            NCMaterialDetails = null;
            MaterialCount = null;
            NCMaterial=null;
            return InnerHTML + "</center>";
        }

        function setMat_Def_Dailog(MDefDetails) {
            var MaterialDetails;
            var MaterialDetailsList=MDefDetails.split('§');
            
            var InnerHTML;
            InnerHTML = '<center style=\"font-size:20px;padding-bottom:10px\">Material Deficiency List</center><center><table width=\"350px\" class=\"cssOperation\">';
            InnerHTML += '<tr><th>Part Number</th><th>Pending Qty.</th><th>Available Qty.</th><th>Deficiency Qty.</th></tr>';
            for (MDIndex = 0; MDIndex < MaterialDetailsList.length;MDIndex++) {
                
                MaterialDetails = MaterialDetailsList[MDIndex].split('Ø');
                InnerHTML += '<tr><td>' + MaterialDetails[0] + '</td><td>' + MaterialDetails[2] + '</td><td>' + MaterialDetails[3] + '</td><td>' +(parseFloat(MaterialDetails[2])-parseFloat(MaterialDetails[3])) + '</td></tr>';
            }
            InnerHTML += '</table><center/>';

            MaterialDetails = null;
            MaterialDetailsList = null;
            MDefDetails = null;
            return InnerHTML;
        }
    </script>


    
</head>
<body>
    <form id="form1" runat="server">
    <div style="left:0;top:0;">
        <canvas id="JobOrderDashboard" style="left:0;top:0;" onmousemove="CanvasMouseMove(event);" >

        </canvas>
    </div>
    <div style="position:absolute;display:none;width:300px;height:450px;" id="MessageDialog" ></div>
    </form>
    <table>
        <tr>
            <td rowspan="">

            </td>
        </tr>
    </table>
</body>
</html>
