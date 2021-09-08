using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;


namespace MRLWMSC21Common
{
    public class ReportCommon
    {


        public static String FormatDate(String vDate, Char Delemiter)
        {
            String[] vDateArray = vDate.Split(Delemiter);

            return vDateArray[1] + "/" + vDateArray[0] + "/" + vDateArray[2];
        }


        public static String FormatDateTime(String vDate, Char Delemiter)
        {

            String[] DateList = vDate.Split(' ');
            String[] Date = DateList[0].Split(Delemiter);
            String[] time = DateList[1].Split(':');

            String reportTime = Date[1] + "/" + Date[0] + "/" + Date[2] + " " + time[0] + ":" + time[1];


            return reportTime;
        }

        public static String FormatReturnDate(String vDate, Char Delemiter)
        {

            String[] DateList = vDate.Split(' ');
            String[] Date = DateList[0].Split(Delemiter);


            String reportTime = Date[1] + "/" + Date[0] + "/" + Date[2];


            return reportTime;
        }


        public static String FormatReturnDateTime(String vDate, Char Delemiter)
        {

            String[] DateList = vDate.Split(' ');
            String[] Date = DateList[0].Split(Delemiter);
            String[] time = DateList[1].Split(':');

            String reportTime = Date[0] + "-" + Date[1] + "-" + Date[2] + " " + time[0] + ":" + time[1] + " " + CommonLogic.IIF(Convert.ToInt32(time[0]) >= 12, "PM", "AM");


            return reportTime;
        }


        public static String FormatDateTimeWithoutTT(String vDate, Char Delemiter)
        {

            String[] DateList = vDate.Split(' ');
            String[] Date = DateList[0].Split(Delemiter);
            String[] time = DateList[1].Split(':');

            String reportTime = Date[1] + "/" + Date[0] + "/" + Date[2] + " " + time[0] + ":" + time[1];


            return reportTime;
        }


        public static bool IsHtmlEncoded(string text)
        {
            return (HttpUtility.HtmlDecode(text) != text);
        }

        public static void GetCapturedDateTimes(DropDownList ddlReportDateTime, String SPName, String HospitalNumber, String AdmissionDate, Char Delemiter)
        {
            ddlReportDateTime.Items.Clear();

            String SQLStr = SPName + "  @AdmissionDate='" + FormatDate(AdmissionDate, Delemiter) + "', @HospitalNumber='" + HospitalNumber + "'";
            IDataReader rS = DB.GetRS(SQLStr);
            ddlReportDateTime.Items.Clear();

            while (rS.Read())
            {
                String vTimeValue = DB.RSFieldDateTime(rS, "Time").ToString("dd/MM/yyyy HH:mm tt");

                ddlReportDateTime.Items.Add(new ListItem(vTimeValue, vTimeValue));
            }
            rS.Close();
        }


        public static void GetAdmissionDates(DropDownList ddlAdmissionDates, String SPName, String HospitalNumber)
        {

            String SQLStr = SPName + " @HospitalNumber='" + HospitalNumber + "'";
            IDataReader rS = DB.GetRS(SQLStr);
            ddlAdmissionDates.Items.Clear();
            while (rS.Read())
            {
                String vTimeValue = DB.RSField(rS, "AddmissionDate");
                ddlAdmissionDates.Items.Add(new ListItem(vTimeValue, vTimeValue));
            }
            rS.Close();

        }


        public static void GetNurseList(DropDownList ddlNurseList)
        {

            String SQLStr = "Select UserName from MVQUsersView Where UserTypeID=14 AND Status='Active' order by UserName ";
            IDataReader rS = DB.GetRS(SQLStr);
            ddlNurseList.Items.Clear();
            ddlNurseList.Items.Add(new ListItem("Select Nurse", ""));
            while (rS.Read())
            {
                String vUserName = DB.RSField(rS, "UserName");
                ddlNurseList.Items.Add(new ListItem(vUserName, vUserName));
            }
            rS.Close();

        }


        public static void LoadDropdownList(DropDownList ddlDropDown, String SqlQuery)
        {

            IDataReader rS = DB.GetRS(SqlQuery);
            ddlDropDown.Items.Clear();
            ddlDropDown.Items.Add(new ListItem("Select Theme", ""));
            while (rS.Read())
            {
                String vTheme = DB.RSField(rS, "ThemeName");
                Int32 vID = DB.RSFieldInt(rS, "ICU_DashBoardID");
                ddlDropDown.Items.Add(new ListItem(vTheme, vID.ToString()));
            }
            rS.Close();


        }



        public static void LoadDropDownfromXML(DropDownList ddlDropDown, string xmlFile)
        {

            ddlDropDown.Items.Clear();
            ddlDropDown.Items.Add(new ListItem("Select Printer", ""));

            DataSet ds = new DataSet();
            ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath(xmlFile));
            ddlDropDown.DataTextField = "PrinterName";
            ddlDropDown.DataValueField = "PrinterIP";
            ddlDropDown.DataSource = ds;
            ddlDropDown.DataBind();


            /*
            string parentElementName = "";
            string childElementName = "";
            string childElementValue = "";
            bool element = false;
           // lblXml.Text = "";

            ddlDropDown.Items.Clear();
            ddlDropDown.Items.Add(new ListItem("Select Printer", ""));

            XmlTextReader xmlReader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath(xmlFile));
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (element)
                    {
                        parentElementName = parentElementName + childElementName + "<br/>";
                    }
                    element = true;
                    childElementName = xmlReader.Name;
                }
                else if (xmlReader.NodeType == XmlNodeType.Text | xmlReader.NodeType == XmlNodeType.CDATA)
                {
                    element = false;
                    childElementValue = xmlReader.Value;
                  //  lblXml.Text = lblXml.Text + "<b>" + parentElementName + "<br/>" + childElementName + "</b><br/>" + childElementValue;

                    ddlDropDown.Items.Add(new ListItem(childElementName, childElementValue));
                   parentElementName = "";
                  childElementName = "";

                 
                }
            }
            xmlReader.Close();
             
            */

        }

    }
}
