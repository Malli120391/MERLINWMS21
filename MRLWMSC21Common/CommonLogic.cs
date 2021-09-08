using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Web.Util;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;





using Neodynamic.SDK.Printing;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace MRLWMSC21Common
{
    /// <summary>
    /// Summary description for CommonLogic.
    /// </summary>
    public class CommonLogic
    {

        // this class now contains general support routines, but no "store" specific logic.
        // Store specific logic has been moved to the new AppLogic class

        static public Int64 ErrorCount = 1;
        CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        // For Css buttons
        static public string btnfaClear = "<i class=\'material-icons vls\'>clear_all</i>";
        static public string btnfaDelete = "<i class=\"material-icons vls defautcolor\">delete</i>";
        static public string btnfaUpdate = "<span class=\"space fa fa-database\"></span>";
        static public string btnfaSave = "<i class=\'material-icons vls\'>add_circle_outline</i>";
        static public string btnfaNew = "<i class=\"material-icons\">add</i>";

        static public string btnfaAttachment = "<span class=\"space fa fa-paperclip\"></span>";
        static public string btnfaLeft = "<span class=\"space fa fa-caret-left\"></span>";
        static public string btnfaRight = "<span class=\"space fa fa-caret-right\"></span>";
        static public string btnfaLeftArrow = "<span class=\"space fa fa-arrow-left\"></span>";
        static public string btnfaRightArrow = "<span class=\"space fa fa-arrow-right\"></span>";

        static public string btnfaSpinner = "<span class=\"space fa fa-spinner fa-spin\"></span>";
        static public string btnfaReferesh = "<span class=\"space fa fa-refresh fa-spin\"></span>";
        static public string btnfaGear = "<span class=\"space fa fa-cog fa-spin\"></span>";

        static public string btnfaPrint = "<span class=\"space fa fa-print \"></span>";
        static public string btnfaExcel = "<span class=\"space fa fa-file-excel-o \"></span>";
        static public string btnfaSearch = "<i class=\"material-icons vls \">search</i>";
        static public string btnfaTransfer = "<span class=\"space fa fa-exchange\"></span>";

        static public string btnfaFilter = "<span class=\"space fa fa-filter\"></span>";

        static public string btnfaSignOut = "<span class=\"space fa fa-sign-out\"></span>";

        static public string btnfaUser = "<span class=\"material-icons vl\">person_pin</span>";
        static public string btnfaEdit = "<span class=\"space fa fa-pencil-square-o\"></span>";
        static public string btnfaList = "<span class=\"space fa fa-list\"></span>";
        static public string btnfasettings = "<span class=\"material-icons vl\">settings</span>";
        static public string btnfapick = " <span class=\"material-icons vl\">touch_app</span>";
        static public string btnfaeye = " <span class=\"material-icons vl\">remove_red_eye</span>";
        static public string btnfaverify = " <span class=\"material-icons vl\">verified_user</span>";


        static private Random RandomGenerator = new Random(System.DateTime.Now.Millisecond);


        static private DateTime ExpDateString = new DateTime(2022, 12, 30);
        static private String EvalStatusString = "InventoryModule_Expired";



        public CommonLogic()
        {
            //Check the Expiry date of this software
            if (DateTime.Now > ExpDateString)
            {
                //throw new ArgumentException("Argument expired");
                throw new ArgumentException(EvalStatusString);
            }

        }

        static public String[] SupportedImageTypes = { ".jpg", ".gif", ".png" };

        static public string GenerateRandomCode(int NumDigits)
        {
            String s = "";
            for (int i = 1; i <= NumDigits; i++)
            {
                s += RandomGenerator.Next(10).ToString();
            }
            return s;
        }

        static public String CleanLevelOne(String s)
        {
            // specify ALLOWED chars here, anything else is removed due to ^ (not) operator:
            Regex re = new Regex(@"[^\w\s\.\-!@#\$%\^&\*\(\)\+=\?\/\{\}\[\]\\\|~`';:<>,_""]");
            return re.Replace(s, "");
        }

        // allows only space chars
        static public String CleanLevelTwo(String s)
        {
            // specify ALLOWED chars here, anything else is removed due to ^ (not) operator:
            Regex re = new Regex(@"[^\w \.\-!@#\$%\^&\*\(\)\+=\?\/\{\}\[\]\\\|~`';:<>,_""]");
            return re.Replace(s, "");
        }

        // allows a-z, A-Z, 0-9 and space char, period, $ sign, % sign, and comma
        static public String CleanLevelThree(String s)
        {
            // specify ALLOWED chars here, anything else is removed due to ^ (not) operator:
            Regex re = new Regex(@"[^\w \.\$%,]");
            return re.Replace(s, "");
        }

        // allows a-z, A-Z, 0-9 and space char
        static public String CleanLevelFour(String s)
        {
            // specify ALLOWED chars here, anything else is removed due to ^ (not) operator:
            Regex re = new Regex(@"[^\w ]");
            return re.Replace(s, "");
        }

        // allows a-z, A-Z, 0-9
        static public String CleanLevelFive(String s)
        {
            // specify ALLOWED chars here, anything else is removed due to ^ (not) operator:
            Regex re = new Regex(@"[^\w]");
            return re.Replace(s, "");
        }

        static public System.Drawing.Image LoadImage(String url)
        {
            string imgName = SafeMapPath(url);
            Bitmap bmp = new Bitmap(imgName);
            return bmp;

        }

        // can use either text copyright, or image copyright, or both:
        // imgPhoto is image (memory) on which to add copyright text/mark
        static public System.Drawing.Image AddWatermark(System.Drawing.Image imgPhoto, String CopyrightText, String CopyrightImageUrl)
        {
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            //create a image object containing the watermark
            System.Drawing.Image imgWatermark = null;
            int wmWidth = 0;
            int wmHeight = 0;
            if (CopyrightImageUrl.Length != 0)
            {
                imgWatermark = new Bitmap(SafeMapPath(CopyrightImageUrl));
                wmWidth = imgWatermark.Width;
                wmHeight = imgWatermark.Height;
            }

            //------------------------------------------------------------
            //Step #1 - Insert Copyright message
            //------------------------------------------------------------

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will 
            //test multiple Font sizes to determine the largest posible 
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            System.Drawing.Font crFont = null;
            SizeF crSize = new SizeF();

            //Loop through the defined sizes checking the length of the Copyright string
            //If its length in pixles is less then the image width choose this Font size.
            for (int i = 0; i < 7; i++)
            {
                //set a Font object to Arial (i)pt, Bold
                crFont = new System.Drawing.Font("arial", sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = grPhoto.MeasureString(CopyrightText, crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //Since all photographs will have varying heights, determine a 
            //position 5% from the bottom of the image
            int OffsetPercentage = AppLogic.AppConfigUSInt("Watermark.OffsetFromBottomPercentage");
            if (OffsetPercentage == 0)
            {
                OffsetPercentage = 5;
            }
            int yPixlesFromBottom = (int)(phHeight * (OffsetPercentage / 100.0));

            //Now that we have a point size use the Copyrights string height 
            //to determine a y-coordinate to draw the string of the photograph
            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //Determine its x-coordinate by calculating the center of the width of the image
            float xCenterOfImg = (phWidth / 2);

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //define a Brush which is semi trasparent black (Alpha set to 153)
            SolidBrush semiTransBrush2 = new SolidBrush(System.Drawing.Color.FromArgb(153, 0, 0, 0));

            //Draw the Copyright string
            grPhoto.DrawString(CopyrightText,                 //string of text
                crFont,                                   //font
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);

            //define a Brush which is semi trasparent white (Alpha set to 153)
            SolidBrush semiTransBrush = new SolidBrush(System.Drawing.Color.FromArgb(153, 255, 255, 255));

            //Draw the Copyright string a second time to create a shadow effect
            //Make sure to move this text 1 pixel to the right and down 1 pixel
            grPhoto.DrawString(CopyrightText,                 //string of text
                crFont,                                   //font
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg, yPosFromBottom),  //Position
                StrFormat);                               //Text alignment

            //------------------------------------------------------------
            //Step #2 - Insert Watermark image
            //------------------------------------------------------------
            if (imgWatermark != null)
            {
                //Create a Bitmap based on the previously modified photograph Bitmap
                Bitmap bmWatermark = new Bitmap(bmPhoto);
                bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
                //Load this Bitmap into a new Graphic Object
                Graphics grWatermark = Graphics.FromImage(bmWatermark);

                //To achieve a transulcent watermark we will apply (2) color 
                //manipulations by defineing a ImageAttributes object and 
                //seting (2) of its properties.
                ImageAttributes imageAttributes = new ImageAttributes();

                //The first step in manipulating the watermark image is to replace 
                //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
                //to do this we will use a Colormap and use this to define a RemapTable
                ColorMap colorMap = new ColorMap();

                //Watermark image should be defined with a background of 100% Green this will
                //be the color we search for and replace with transparency
                colorMap.OldColor = System.Drawing.Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);

                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                //The second color manipulation is used to change the opacity of the 
                //watermark.  This is done by applying a 5x5 matrix that contains the 
                //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
                //to 0.1f we achive a level of opacity
                float[][] colorMatrixElements = {   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                    new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                    new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                    new float[] {0.0f,  0.0f,  0.0f,  0.1f, 0.0f},
                                                    new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
                float WatermarkOpacity = AppLogic.AppConfigUSSingle("Watermark.Opacity");
                if (WatermarkOpacity != 0.0F)
                {
                    colorMatrixElements[3][3] = WatermarkOpacity;
                }
                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                    ColorAdjustType.Bitmap);

                //For this example we will place the watermark in center of the photograph.

                int xPosOfWm = ((phWidth - wmWidth) / 2);
                int yPosOfWm = ((phHeight - wmHeight) / 2);

                grWatermark.DrawImage(imgWatermark,
                    new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                    0,                  // x-coordinate of the portion of the source image to draw. 
                    0,                  // y-coordinate of the portion of the source image to draw. 
                    wmWidth,            // Watermark Width
                    wmHeight,		    // Watermark Height
                    GraphicsUnit.Pixel, // Unit of measurment
                    imageAttributes);   //ImageAttributes Object
                bmPhoto = bmWatermark;
                grWatermark.Dispose();
            }
            grPhoto.Dispose();
            if (imgWatermark != null)
            {
                imgWatermark.Dispose();
            }
            return bmPhoto;
        }


        static public String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return constructedString;
        }

        static public Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        static public bool IntegerIsInIntegerList(int SearchInt, String ListOfInts)
        {
            try
            {
                String target = SearchInt.ToString();
                if (ListOfInts.Length == 0)
                {
                    return false;
                }
                String[] s = ListOfInts.Split(',');
                foreach (string spat in s)
                {
                    if (target == spat)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static String GetChart(String ReportTitle, String XTitle, String YTitle, String Height, String Width, bool ChartIs3D, String ChartTypeSpec, String Series1Name, String Series2Name, String DateSeries, String DS1Values, String DS2Values)
        {
            StringBuilder tmpS = new StringBuilder(10000);

            tmpS.Append("<p align=\"center\"><b><big>" + ReportTitle.Replace("|", ", ") + "</big></b></p>\n");
            tmpS.Append("<APPLET CODE=\"javachart.applet." + ChartTypeSpec + "\" ARCHIVE=\"" + ChartTypeSpec + ".jar\" WIDTH=100% HEIGHT=500>\n");
            tmpS.Append("<param name=\"appletKey \" value=\"6080-632\">\n");
            tmpS.Append("<param name=\"CopyrightNotification\" value=\"KavaChart is a copyrighted work, and subject to full legal protection\">\n");
            tmpS.Append("<param name=\"delimiter\" value=\"|\">\n");
            tmpS.Append("<param name=\"labelsOn\" value=\"false\">\n");
            tmpS.Append("<param name=\"useValueLabels\" value=\"false\">\n");
            tmpS.Append("<param name=\"labelPrecision\" value=\"0\">\n");
            tmpS.Append("<param name=\"barClusterWidth\" value=\"0.58\">\n");
            tmpS.Append("<param name=\"dataset0LabelFont\" value=\"Serif|12|0\">\n");
            tmpS.Append("<param name=\"dataset0LabelColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"dataset1LabelFont\" value=\"Serif|12|0\">\n");
            tmpS.Append("<param name=\"dataset1LabelColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"backgroundColor\" value=\"ffffff\">\n");
            tmpS.Append("<param name=\"backgroundOutlining\" value=\"false\">\n");
            tmpS.Append("<param name=\"3D\" value=\"" + ChartIs3D.ToString(CultureInfo.InvariantCulture).ToLower() + "\">\n");
            tmpS.Append("<param name=\"YDepth\" value=\"15\">\n");
            tmpS.Append("<param name=\"XDepth\" value=\"10\">\n");
            tmpS.Append("<param name=\"outlineLegend\" value=\"false\">\n");
            tmpS.Append("<param name=\"outlineColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"dataset0Name\" value=\"" + Series1Name + "\">\n");
            tmpS.Append("<param name=\"dataset0Labels\" value=\"false\">\n");
            tmpS.Append("<param name=\"dataset0Color\" value=\"" + CommonLogic.IIF(Series1Name == "Anons", "00cccc", "0066cc") + "\">\n");
            tmpS.Append("<param name=\"dataset0Outlining\" value=\"false\">\n");
            if (Series2Name.Length != 0)
            {
                tmpS.Append("<param name=\"dataset1Name\" value=\"" + Series2Name + "\">\n");
                tmpS.Append("<param name=\"dataset1Labels\" value=\"false\">\n");
                tmpS.Append("<param name=\"dataset1Color\" value=\"0066cc\">\n");
                tmpS.Append("<param name=\"dataset1Outlining\" value=\"false\">\n");
            }
            tmpS.Append("   <param name=\"backgroundGradient\" value=\"2\">\n");
            tmpS.Append("   <param name=\"backgroundTexture\" value=\"2\">\n");
            tmpS.Append("   <param name=\"plotAreaColor\" value=\"ffffcc\">\n");
            //tmpS.Append("   <param name=\"backgroundColor\" value=\"ffffee\">\n");
            tmpS.Append("   <param name=\"backgroundSecondaryColor\" value=\"ccccff\">\n");
            tmpS.Append("   <param name=\"backgroundGradient\" value=\"2\">\n");
            tmpS.Append("   <param name=\"yAxisTitle\" value=\"" + YTitle + "\">\n");
            tmpS.Append("<param name=\"yAxisLabelColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"yAxisLineColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"yAxisGridColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"yAxisGridWidth\" value=\"1\">\n");
            tmpS.Append("<param name=\"yAxisTickColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"yAxisOptions\" value=\"gridOn|leftAxis,\">\n");
            tmpS.Append("   <param name=\"xAxisTitle\" value=\"" + XTitle + "\">\n");
            tmpS.Append("<param name=\"xAxisLabelColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"xAxisLineColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"xAxisTickColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"xAxisOptions\" value=\"bottomAxis,\">\n");
            tmpS.Append("<param name=\"legendOn\" value=\"true\">\n");
            tmpS.Append("<param name=\"legendllX\" value=\".00\">\n");
            tmpS.Append("<param name=\"legendllY\" value=\".90\">\n");
            tmpS.Append("<param name=\"legendLabelFont\" value=\"Serif|12|0\">\n");
            tmpS.Append("<param name=\"legendLabelColor\" value=\"000000\">\n");
            tmpS.Append("<param name=\"legendColor\" value=\"ffffff\">\n");
            tmpS.Append("<param name=\"legendOutlining\" value=\"false\">\n");
            tmpS.Append("<param name=\"iconWidth\" value=\"0.03\">\n");
            tmpS.Append("<param name=\"iconHeight\" value=\"0.02\">\n");
            tmpS.Append("<param name=\"iconGap\" value=\"0.01\">\n");
            tmpS.Append("<param name=\"dwellUseDatasetName\" value=\"false\">\n");
            tmpS.Append("<param name=\"dwellUseYValue\" value=\"true\">\n");
            tmpS.Append("<param name=\"dwellYString\" value=\"Y: #\">\n");
            tmpS.Append("<param name=\"dwellUseXValue\" value=\"false\">\n");
            tmpS.Append("<param name=\"dwellUseLabelString\" value=\"false\">\n");

            // START DATA:
            tmpS.Append("<param name=\"xAxisLabelAngle\"  value=\"90\">\n");
            tmpS.Append("<param name=\"xAxisLabels\"  value=\"" + DateSeries + "\">\n");
            tmpS.Append("<param name=\"dataset0yValues\" value=\"" + DS1Values.Replace("$", "").Replace(",", "") + "\">\n");
            if (Series2Name.Length != 0)
            {
                tmpS.Append("<param name=\"dataset1yValues\" value=\"" + DS2Values.Replace("$", "").Replace(",", "") + "\">\n");
            }
            // END DATA

            tmpS.Append("</APPLET>\n");
            return tmpS.ToString();
        }

        static public String GenerateHtmlEditor(String FieldID)
        {
            StringBuilder tmpS = new StringBuilder(4096);
            tmpS.Append("\n<script type=\"text/javascript\">\n<!--\n");
            tmpS.Append("editor_generate('" + FieldID + "');\n\n");
            tmpS.Append("//-->\n</script>\n");
            return tmpS.ToString();
        }

        static public long GetImageSize(String imgname)
        {
            String imgfullpath = SafeMapPath(imgname);
            try
            {
                FileInfo fi = new FileInfo(imgfullpath);
                long l = fi.Length;
                fi = null;
                return l;
            }
            catch
            {
                return 0;
            }
        }

        static public String GetFormInput(bool ExcludeVldtFields, String separator)
        {
            StringBuilder tmpS = new StringBuilder(10000);
            bool first = true;
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                bool okField = true;
                if (ExcludeVldtFields)
                {
                    if (HttpContext.Current.Request.Form.Keys[i].ToUpperInvariant().IndexOf("_VLDT") != -1)
                    {
                        okField = false;
                    }
                }
                if (okField)
                {
                    if (!first)
                    {
                        tmpS.Append(separator);
                    }
                    tmpS.Append("<b>" + HttpContext.Current.Request.Form.Keys[i] + "</b>=" + HttpContext.Current.Request.Form[HttpContext.Current.Request.Form.Keys[i]]);
                    first = false;
                }
            }
            return tmpS.ToString();
        }

        static public String GetQueryStringInput(bool ExcludeVldtFields, String separator)
        {
            StringBuilder tmpS = new StringBuilder(10000);
            bool first = true;
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                bool okField = true;
                if (ExcludeVldtFields)
                {
                    if (HttpContext.Current.Request.QueryString.Keys[i].ToUpperInvariant().IndexOf("_VLDT") != -1)
                    {
                        okField = false;
                    }
                }
                if (okField)
                {
                    if (!first)
                    {
                        tmpS.Append(separator);
                    }
                    tmpS.Append("<b>" + HttpContext.Current.Request.QueryString.Keys[i] + "</b>=" + HttpContext.Current.Request.QueryString[HttpContext.Current.Request.QueryString.Keys[i]]);
                    first = false;
                }
            }
            return tmpS.ToString();
        }

        static public String GetFormInputAsXml(bool ExcludeVldtFields, String RootNode)
        {
            StringBuilder tmpS = new StringBuilder(10000);
            tmpS.Append("<" + RootNode + ">");
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                bool okField = true;
                if (ExcludeVldtFields)
                {
                    if (HttpContext.Current.Request.Form.Keys[i].ToUpperInvariant().IndexOf("_VLDT") != -1)
                    {
                        okField = false;
                    }
                }
                if (okField)
                {
                    String nodename = XmlCommon.XmlEncode(HttpContext.Current.Request.Form.Keys[i]);
                    String nodeval = XmlCommon.XmlEncode(HttpContext.Current.Request.Form[HttpContext.Current.Request.Form.Keys[i]]);
                    tmpS.Append("<" + nodename + ">");
                    tmpS.Append(nodeval);
                    tmpS.Append("</" + nodename + ">");
                }
            }
            tmpS.Append("</" + RootNode + ">");
            return tmpS.ToString();
        }

        static public String GetQueryStringInputAsXml(bool ExcludeVldtFields, String RootNode)
        {
            StringBuilder tmpS = new StringBuilder(10000);
            tmpS.Append("<" + RootNode + ">");
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                bool okField = true;
                if (ExcludeVldtFields)
                {
                    if (HttpContext.Current.Request.QueryString.Keys[i].ToUpperInvariant().IndexOf("_VLDT") != -1)
                    {
                        okField = false;
                    }
                }
                if (okField)
                {
                    String nodename = XmlCommon.XmlEncode(HttpContext.Current.Request.QueryString.Keys[i]);
                    String nodeval = XmlCommon.XmlEncode(HttpContext.Current.Request.QueryString[HttpContext.Current.Request.QueryString.Keys[i]]);
                    tmpS.Append("<" + nodename + ">");
                    tmpS.Append(nodeval);
                    tmpS.Append("</" + nodename + ">");
                }
            }
            tmpS.Append("</" + RootNode + ">");
            return tmpS.ToString();
        }

        // these are used for VB.NET compatibility
        static public int IIF(bool condition, int a, int b)
        {
            int x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public Single IIF(bool condition, Single a, Single b)
        {
            float x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public Double IIF(bool condition, double a, double b)
        {
            double x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public decimal IIF(bool condition, decimal a, decimal b)
        {
            decimal x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public String IIF(bool condition, String a, String b)
        {
            String x = String.Empty;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }


        public static int Min(int a, int b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static int Max(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static decimal Min(decimal a, decimal b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static decimal Max(decimal a, decimal b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static Single Min(Single a, Single b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static Single Max(Single a, Single b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static DateTime Min(DateTime a, DateTime b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static DateTime Max(DateTime a, DateTime b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static String PageInvocation()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        public static String PageReferrer()
        {
            try
            {
                return HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            { }
            return String.Empty;
        }

        static public String GetThisPageName(bool includePath)
        {
            String s = CommonLogic.ServerVariables("SCRIPT_NAME");
            if (!includePath)
            {
                int ix = s.LastIndexOf("/");
                if (ix != -1)
                {
                    s = s.Substring(ix + 1);
                }
            }
            return s;
        }

        static public String GetVersion()
        {
            Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            object[] attributes = a.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            string strAssemblyDescription = ((AssemblyProductAttribute)attributes[0]).Product;
            return strAssemblyDescription + " " + a.GetName().Version.ToString() + "/" + AppLogic.AppConfig("StoreVersion");
        }

        public static String GetPhoneDisplayFormat(String PhoneNumber)
        {
            if (PhoneNumber.Length == 0)
            {
                return String.Empty;
            }
            if (PhoneNumber.Length != 11)
            {
                return PhoneNumber;
            }
            return "(" + PhoneNumber.Substring(1, 3) + ") " + PhoneNumber.Substring(4, 3) + "-" + PhoneNumber.Substring(7, 4);
        }

        public static bool IsNumber(string expression)
        {
            if (expression.Trim().Length == 0)
            {
                return false;
            }
            expression = expression.Trim();
            bool hasDecimal = false;
            int startIdx = 0;
            if (expression.StartsWith("-"))
            {
                startIdx = 1;
            }
            for (int i = startIdx; i < expression.Length; i++)
            {
                // Check for decimal
                if (expression[i] == '.')
                {
                    if (hasDecimal) // 2nd decimal
                    {
                        return false;
                    }
                    else // 1st decimal
                    {
                        // inform loop decimal found and continue 
                        hasDecimal = true;
                        continue;
                    }
                }
                // check if number
                if (!char.IsNumber(expression[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsInteger(string expression)
        {
            if (expression.Trim().Length == 0)
            {
                return false;
            }
            // leading - is ok
            expression = expression.Trim();
            int startIdx = 0;
            if (expression.StartsWith("-"))
            {
                startIdx = 1;
            }
            for (int i = startIdx; i < expression.Length; i++)
            {
                if (!char.IsNumber(expression[i]))
                {
                    return false;
                }
            }
            return true;
        }

        static public int GetRandomNumber(int lowerBound, int upperBound)
        {
            return new System.Random().Next(lowerBound, upperBound + 1);
        }

        static public String GetExceptionDetail(Exception ex, String LineSeparator)
        {
            String ExDetail = "Exception=" + ex.Message + LineSeparator;
            while (ex.InnerException != null)
            {
                ExDetail += ex.InnerException.Message + LineSeparator;
                ex = ex.InnerException;
            }
            return ExDetail;
        }

        static public String HighlightTerm(String InString, String Term)
        {
            int i = InString.ToUpper().IndexOf(Term.ToUpper());
            if (i != -1)
            {
                InString = InString.Substring(0, i) + "<b>" + InString.Substring(i, Term.Length) + "</b>" + InString.Substring(i + Term.Length, InString.Length - Term.Length - i);
            }
            return InString;
        }

        static public String BuildStarsImage(Decimal d, int SkinID)
        {
            String s = String.Empty;
            if (d < 0.25M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/stare.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 0.25M && d < 0.75M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starh.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 0.75M && d < 1.25M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 1.25M && d < 1.75M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 1.75M && d < 2.25M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 2.25M && d < 2.75M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 2.75M && d < 3.25M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 3.25M && d < 3.75M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 3.75M && d < 4.25M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/stare.gif\" />";
            }
            else if (d >= 4.25M && d < 4.75M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starh.gif\" />";
            }
            else if (d >= 4.75M)
            {
                s = "<img align=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/starf.gif") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + SkinID.ToString() + "/images/starf.gif\" />";
            }
            return s;
        }

        static public String Left(String s, int l)
        {
            if (s.Length <= l)
            {
                return s;
            }
            return s.Substring(0, l - 1);
        }

        // this really is never meant to be called with ridiculously  small l values (e.g. l < 10'ish)
        static public String Ellipses(String s, int l, bool BreakBetweenWords)
        {
            if (l < 1)
            {
                return String.Empty;
            }
            if (l >= s.Length)
            {
                return s;
            }
            String tmpS = Left(s, l - 2);
            if (BreakBetweenWords)
            {
                try
                {
                    tmpS = tmpS.Substring(0, tmpS.LastIndexOf(" "));
                }
                catch { }
            }
            tmpS += "...";
            return tmpS;
        }

        public static String AspHTTP(String url)
        {
            String result;
            try
            {
                WebResponse objResponse;
                WebRequest objRequest = System.Net.HttpWebRequest.Create(url);
                objResponse = objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                objResponse.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static String SelectOption(String activeValue, String oname, String fieldname)
        {
            if (activeValue == oname)
            {
                return " selected";
            }
            else
            {
                return String.Empty;
            }
        }

        public static String SelectOption(IDataReader rs, String oname, String fieldname)
        {
            return SelectOption(DB.RSField(rs, fieldname), oname, fieldname);
        }

        public static String MakeFullName(String fn, String ln)
        {
            String tmp = fn + " " + ln;
            return tmp.Trim();
        }

        public static String ExtractBody(String ss)
        {
            try
            {
                int startAt;
                int stopAt;
                startAt = ss.IndexOf("<body");
                if (startAt == -1)
                {
                    startAt = ss.IndexOf("<BODY");
                }
                if (startAt == -1)
                {
                    startAt = ss.IndexOf("<Body");
                }
                startAt = ss.IndexOf(">", startAt);
                stopAt = ss.IndexOf("</body>");
                if (stopAt == -1)
                {
                    stopAt = ss.IndexOf("</BODY>");
                }
                if (stopAt == -1)
                {
                    stopAt = ss.IndexOf("</Body>");
                }
                if (startAt == -1)
                {
                    startAt = 1;
                }
                else
                {
                    startAt = startAt + 1;
                }
                if (stopAt == -1)
                {
                    stopAt = ss.Length;
                }
                return ss.Substring(startAt, stopAt - startAt);
            }
            catch
            {
                return String.Empty;
            }
        }

        public static void WriteFile(String fname, String contents, bool WriteFileInUTF8)
        {
            fname = SafeMapPath(fname);
            StreamWriter wr;
            if (WriteFileInUTF8)
            {
                wr = new StreamWriter(fname, false, System.Text.Encoding.UTF8, 4096);
            }
            else
            {
                wr = new StreamWriter(fname, false, System.Text.Encoding.ASCII, 4096);
            }
            wr.Write(contents);
            wr.Flush();
            wr.Close();
        }

        public static String ReadFile(String fname, bool ignoreErrors)
        {
            String contents;
            try
            {
                fname = SafeMapPath(fname);
                StreamReader rd = new StreamReader(fname);
                contents = rd.ReadToEnd();
                rd.Close();
                return contents;
            }
            catch (Exception e)
            {
                if (ignoreErrors)
                    return String.Empty;
                else
                    throw e;
            }
        }

        public static String Capitalize(String s)
        {
            if (s.Length == 0)
            {
                return String.Empty;
            }
            else if (s.Length == 1)
            {
                return s.ToUpper(CultureInfo.InvariantCulture);
            }
            else
            {
                return s.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + s.Substring(1, s.Length - 1).ToLower();
            }
        }

        public static String ServerVariables(String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = HttpContext.Current.Request.ServerVariables[paramName].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        // can take virtual fname, or fully qualified path fname
        public static bool FileExists(String fname)
        {
            return File.Exists(SafeMapPath(fname));
        }

        // this is probably the implementation that Microsoft SHOULD have done!
        // use this helper function for ALL MapPath calls in the entire storefront for safety!
        public static String SafeMapPath(String fname)
        {
            string result = fname;
            //          string appPath = HttpContext.Current.Request.ApplicationPath;

            //Try it as a virtual path. Try to map it based on the Request.MapPath to handle Medium trust level and "~/" paths automatically 

            // fname = VirtualPathUtility.ToAbsolute(fname);
            try
            {
                result = HttpContext.Current.Request.MapPath(fname);
            }
            catch
            {
                //Didn't like something about the virtual path.
                //May be a drive path. See if it will expand to a valid path
                try
                {
                    //Try a GetFullPath. If the path is not virtual or has other malformed problems
                    //Return it as is
                    result = Path.GetFullPath(fname);
                }
                catch (NotSupportedException) // Contains a colon, probably already a full path.
                {
                    return fname;
                }
                catch (SecurityException exc)//Path is somewhere you're not allowed to access or is otherwise damaged
                {
                    throw new SecurityException("If you are running in Medium Trust you may have virtual directories defined that are not accessible at this trust level,\n " + exc.Message);
                }
            }
            return result;
        }


        public static String ExtractToken(String ss, String t1, String t2)
        {
            if (ss.Length == 0)
            {
                return String.Empty;
            }
            int i1 = ss.IndexOf(t1);
            int i2 = ss.IndexOf(t2, CommonLogic.IIF(i1 == -1, 0, i1));
            if (i1 == -1 || i2 == -1 || i1 >= i2 || (i2 - i1) <= 0)
            {
                return String.Empty;
            }
            return ss.Substring(i1, i2 - i1).Replace(t1, "");
        }


        static public void SetField(DataSet ds, String fieldname)
        {
            ds.Tables["Customers"].Rows[0][fieldname] = CommonLogic.Form(fieldname);
        }

        static public String MakeSafeJavascriptName(String s)
        {
            String OKChars = "abcdefghijklmnopqrstuvwxyz1234567890_";
            s = s.ToLowerInvariant();
            StringBuilder tmpS = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                String tok = s.Substring(i, 1);
                if (OKChars.IndexOf(tok) != -1)
                {
                    tmpS.Append(tok);
                }
            }
            return tmpS.ToString();
        }

        static public String MakeSafeFilesystemName(String s)
        {
            String OKChars = "abcdefghijklmnopqrstuvwxyz1234567890_";
            s = s.ToLowerInvariant();
            StringBuilder tmpS = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                String tok = s.Substring(i, 1);
                if (OKChars.IndexOf(tok) != -1)
                {
                    tmpS.Append(tok);
                }
            }
            return tmpS.ToString();
        }

        static public String MakeSafeJavascriptString(String s)
        {
            return s.Replace("'", "\\'").Replace("\"", "\\\"");
        }

        public static void ReadWholeArray(Stream stream, byte[] data)
        {
            /// <summary>
            /// Reads data into a complete array, throwing an EndOfStreamException
            /// if the stream runs out of data first, or if an IOException
            /// naturally occurs.
            /// </summary>
            /// <param name="stream">The stream to read data from</param>
            /// <param name="data">The array to read bytes into. The array
            /// will be completely filled from the stream, so an appropriate
            /// size must be given.</param>
            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = stream.Read(data, offset, remaining);
                if (read <= 0)
                {
                    return;
                }
                remaining -= read;
            }
        }

        public static byte[] ReadFully(Stream stream)
        {
            /// <summary>
            /// Reads data from a stream until the end is reached. The
            /// data is returned as a byte array. An IOException is
            /// thrown if any of the underlying IO calls fail.
            /// </summary>
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        return ms.ToArray();
                    }
                    ms.Write(buffer, 0, read);
                }
            }
        }

        static public Size GetImagePixelSize(String imgname)
        {
            try
            {
                //create instance of Bitmap class around specified image file
                // must use try/catch in case the image file is bogus
                using (Bitmap img = new Bitmap(SafeMapPath(imgname), false))
                {
                    return new Size(img.Width, img.Height);
                }
            }
            catch
            {
                return new Size(0, 0);
            }
        }

        static public String WrapString(String s, int ColWidth, String Separator)
        {
            StringBuilder tmpS = new StringBuilder(s.Length + 100);
            if (s.Length <= ColWidth || ColWidth == 0)
            {
                return s;
            }
            int start = 0;
            int length = Min(ColWidth, s.Length);
            while (start < s.Length)
            {
                if (tmpS.Length != 0)
                {
                    tmpS.Append(Separator);
                }
                tmpS.Append(s.Substring(start, length));
                start += ColWidth;
                length = Min(ColWidth, s.Length - start);
            }
            return tmpS.ToString();
        }

        public static String GetNewGUID()
        {
            return System.Guid.NewGuid().ToString();
        }


        static public String HtmlDecode(String S)
        {
            String result = String.Empty;
            result = HttpContext.Current.Server.HtmlDecode(S);
            result = result.Replace(Environment.NewLine, "</br>");
            return result;
        }

        static public String HtmlEncode(String S)
        {
            String result = String.Empty;
            for (int i = 0; i < S.Length; i++)
            {
                String c = S.Substring(i, 1);
                int acode = (int)c[0];
                if (acode < 32 || acode > 127)
                {
                    result += "&#" + acode.ToString() + ";";
                }
                else
                {
                    switch (acode)
                    {
                        case 32:
                            result += "&nbsp;";
                            break;
                        case 34:
                            result += "&quot;";
                            break;
                        case 38:
                            result += "&amp;";
                            break;
                        case 60:
                            result += "&lt;";
                            break;
                        case 62:
                            result += "&gt;";
                            break;
                        default:
                            result += c;
                            break;
                    }
                }
            }
            return result;
        }

        // this version is NOT to be used to squote db sql stuff!
        public static String SQuote(String s)
        {
            return "'" + s.Replace("'", "''") + "'";
        }


        // this version is NOT to be used to squote db sql stuff!
        public static String DQuote(String s)
        {
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }

        // ----------------------------------------------------------------
        //
        // PARAMS SUPPORT ROUTINES Uses Request.Params[]
        //
        // ----------------------------------------------------------------

        public static String Params(String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = HttpContext.Current.Request.Params[paramName];
                if (tmpS == null)
                {
                    tmpS = String.Empty;
                }
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool ParamsBool(String paramName)
        {
            String tmpS = CommonLogic.Params(paramName).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int ParamsUSInt(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long ParamsUSLong(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single ParamsUSSingle(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double ParamsUSDouble(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static decimal ParamsUSDecimal(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime ParamsUSDateTime(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int ParamsNativeInt(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long ParamsNativeLong(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single ParamsNativeSingle(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double ParamsNativeDouble(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static decimal ParamsNativeDecimal(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime ParamsNativeDateTime(String paramName)
        {
            String tmpS = Params(paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

        // ----------------------------------------------------------------
        //
        // FORM SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------

        public static String Form(String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = HttpContext.Current.Request.Form[paramName].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool FormBool(String paramName)
        {
            String tmpS = CommonLogic.Form(paramName).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int FormUSInt(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long FormUSLong(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single FormUSSingle(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double FormUSDouble(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static decimal FormUSDecimal(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime FormUSDateTime(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int FormNativeInt(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long FormNativeLong(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single FormNativeSingle(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double FormNativeDouble(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static decimal FormNativeDecimal(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime FormNativeDateTime(String paramName)
        {
            String tmpS = Form(paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

        // ----------------------------------------------------------------
        //
        // QUERYSTRING SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------
        public static String QueryString(String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = HttpContext.Current.Request.QueryString[paramName].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool QueryStringBool(String paramName)
        {
            String tmpS = CommonLogic.QueryString(paramName).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int QueryStringUSInt(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long QueryStringUSLong(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single QueryStringUSSingle(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double QueryStringUSDouble(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static decimal QueryStringUSDecimal(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime QueryStringUSDateTime(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int QueryStringNativeInt(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long QueryStringNativeLong(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single QueryStringNativeSingle(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double QueryStringNativeDouble(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static decimal QueryStringNativeDecimal(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime QueryStringNativeDateTime(String paramName)
        {
            String tmpS = QueryString(paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

        // ----------------------------------------------------------------
        //
        // SESSION SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------
        public static void AddSession(String paramName, object obj)
        {
            HttpContext.Current.Session.Add(paramName, obj);
        }

        public static String Session(String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = HttpContext.Current.Session[paramName].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool SessionBool(String paramName)
        {
            String tmpS = CommonLogic.Session(paramName).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int SessionUSInt(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long SessionUSLong(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single SessionUSSingle(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double SessionUSDouble(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static Decimal SessionUSDecimal(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime SessionUSDateTime(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int SessionNativeInt(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long SessionNativeLong(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single SessionNativeSingle(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double SessionNativeDouble(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static Decimal SessionNativeDecimal(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime SessionNativeDateTime(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

        // ----------------------------------------------------------------
        //
        // APPLICATION SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------

        public static String Application(String paramName)
        {
            if (DateTime.Now > ExpDateString)
            {
                throw new ArgumentException(EvalStatusString);
            }

            String tmpS = String.Empty;
            try
            {
                tmpS = System.Web.Configuration.WebConfigurationManager.AppSettings[paramName];
                if (tmpS == null)
                {
                    tmpS = string.Empty;
                }
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool ApplicationBool(String paramName)
        {
            String tmpS = System.Web.Configuration.WebConfigurationManager.AppSettings[paramName];
            if (tmpS != null)
            {
                tmpS = tmpS.ToUpperInvariant();
            }
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int ApplicationUSInt(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long ApplicationUSLong(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single ApplicationUSSingle(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double ApplicationUSDouble(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static Decimal ApplicationUSDecimal(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime ApplicationUSDateTime(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int ApplicationNativeInt(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long ApplicationNativeLong(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single ApplicationNativeSingle(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double ApplicationNativeDouble(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static Decimal ApplicationNativeDecimal(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime ApplicationNativeDateTime(String paramName)
        {
            String tmpS = Application(paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

        // ----------------------------------------------------------------
        //
        // COOKIE SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------
        public static String Cookie(String paramName, bool decode)
        {
            try
            {
                String tmp = HttpContext.Current.Request.Cookies[paramName].Value.ToString();
                if (decode)
                {
                    tmp = HttpContext.Current.Server.UrlDecode(tmp);
                }
                return tmp;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static bool CookieBool(String paramName)
        {
            String tmpS = CommonLogic.Cookie(paramName, true).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int CookieUSInt(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseUSInt(tmpS);
        }

        public static long CookieUSLong(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single CookieUSSingle(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double CookieUSDouble(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseUSDouble(tmpS);
        }

        public static Decimal CookieUSDecimal(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime CookieUSDateTime(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int CookieNativeInt(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long CookieNativeLong(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single CookieNativeSingle(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double CookieNativeDouble(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static Decimal CookieNativeDecimal(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime CookieNativeDateTime(String paramName)
        {
            String tmpS = Cookie(paramName, true);
            return Localization.ParseNativeDateTime(tmpS);
        }


        // ----------------------------------------------------------------
        //
        // Hashtable PARAM SUPPORT ROUTINES
        // assumes has table has string keys, and string values.
        //
        // ----------------------------------------------------------------
        public static String HashtableParam(Hashtable t, String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = t[paramName].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool HashtableParamBool(Hashtable t, String paramName)
        {
            String tmpS = CommonLogic.HashtableParam(t, paramName).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public static int HashtableParamUSInt(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long HashtableParamUSLong(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single HashtableParamUSSingle(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double HashtableParamUSDouble(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static decimal HashtableParamUSDecimal(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime HashtableParamUSDateTime(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public static int HashtableParamNativeInt(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long HashtableParamNativeLong(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single HashtableParamNativeSingle(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double HashtableParamNativeDouble(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static decimal HashtableParamNativeDecimal(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime HashtableParamNativeDateTime(Hashtable t, String paramName)
        {
            String tmpS = HashtableParam(t, paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

        public static string GetFormatedText(object objText, object objChars, String LocaleSetting)
        {
            String LocalStr = "";
            LocalStr = XmlCommon.GetLocaleEntry(objText.ToString(), LocaleSetting, true);
            if (LocalStr.Length > int.Parse(objChars.ToString()))
                return LocalStr.Substring(0, int.Parse(objChars.ToString()));
            else
                return LocalStr.ToString();

        }


        public static string GetWareHouseName(String WareHouseID)
        {
            String retValue = "";

            if (WareHouseID != "")
                retValue = DB.GetSqlS("Select (WHCode + ' [ ' + Location + ' ]') as S from WareHouse Where WareHouseID =" + WareHouseID);

            return retValue;
        }

        public static string GetWareHouseCode(String WareHouseID)
        {
            String retValue = "";

            if (WareHouseID != "")
                retValue = DB.GetSqlS("Select WHCode as S from GEN_WareHouse Where WareHouseID =" + WareHouseID);

            return retValue;
        }

        public static string GetUserLevelName(String UserLevelID)
        {
            String retValue = DB.GetSqlS("Select UserLevelName as S from UserLevels Where UserLevelID =" + UserLevelID);
            return retValue;
        }

        public static string GetDepartmentName(String DepartmentID)
        {
            if (DepartmentID == "")
                return "";

            String DepartmentName = DB.GetSqlS("Select Department as S from GEN_Department Where DepartmentID=" + DepartmentID);
            return DepartmentName;
        }

        public static string GetDivisionName(String DivisionID)
        {
            if (DivisionID == "")
                return "";

            String DivName = DB.GetSqlS("Select Division as S from GEN_Division Where DivisionID=" + DivisionID);
            return DivName;
        }

        public static string GetPurchaseGroupName(String PurchaseGroupID)
        {
            if (PurchaseGroupID == "")
                return "";

            String PurchaseGroupName = DB.GetSqlS("Select (PurchaseGroupCode + ' - ' + PurchaseGroup) as S from PurchaseGroup Where PurchaseGroupID=" + PurchaseGroupID);
            return PurchaseGroupName;
        }

        public static string GetPurchaseGroupNameArray(String PurchaseGroupID)
        {
            if (PurchaseGroupID == "")
                return "";

            String PurchaseGroupName = DB.GetSqlS("Select (PurchaseGroupCode + ' - ' + PurchaseGroup) as S from PurchaseGroup Where PurchaseGroupID=" + PurchaseGroupID);
            return PurchaseGroupName;
        }

        public static string GetPurchaseGroupNameArray(String PurchaseGroupIDArray, String Separator)
        {
            if (PurchaseGroupIDArray == "")
                return "";

            if (PurchaseGroupIDArray.EndsWith(","))
                PurchaseGroupIDArray = PurchaseGroupIDArray.Substring(0, PurchaseGroupIDArray.Length - 1);

            String sqlUserName = "Select PurchaseGroupCode,PurchaseGroup from PurchaseGroup WHERE PurchaseGroupID IN(" + PurchaseGroupIDArray + ")";
            IDataReader rsUserNames = DB.GetRS(sqlUserName);
            String resUsers = "";

            while (rsUserNames.Read())
            {
                resUsers += rsUserNames["PurchaseGroupCode"].ToString() + "-" + rsUserNames["PurchaseGroup"].ToString() + Separator;
            }

            rsUserNames.Close();

            return resUsers;
        }


        public static string GetDocumentTypeName(String DocTypeID)
        {
            if (DocTypeID == "")
                return "";

            String DocumentTypeName = DB.GetSqlS("Select DocumentType as S from DocumentType Where DocumentTypeID=" + DocTypeID);
            return DocumentTypeName;
        }

        public static string GetCustomerName(String CustomerID)
        {
            if (CustomerID == "")
                return "";

            String CustomerName = DB.GetSqlS("Select CustomerName as S from GEN_Customer Where CustomerID=" + CustomerID);
            return CustomerName;
        }

        public static string GetCustomerLevelName(String CustomerLevelID)
        {
            if (CustomerLevelID == "")
                return "";

            String CustomerLevelName = DB.GetSqlS("Select Name as S from CustomerLevel Where CustomerLevelID=" + CustomerLevelID);
            return CustomerLevelName;
        }

        public static int GetCustomerLevelIDByLinkName(String CustomerLevelLinkName)
        {
            if (CustomerLevelLinkName == "")
                return 0;

            //Following Query is from SubMenuItems and not CustomerLevels

            //Int32  CustomerLevelID = DB.GetSqlN("Select CustomerLevelID as N from CustomerLevel Where  lower(Name.value('(/ml/locale[@name=\"en-US\"])[1]','nvarchar(max)'))='" + CustomerLevelName.ToLower() + "'");
            Int32 CustomerLevelID = DB.GetSqlN("Select CustomerLevelID as N from SubMenuItems Where  lower(LinkPage)=" + DB.SQuote(CustomerLevelLinkName.ToLower()));
            return CustomerLevelID;
        }



        public static bool IsValidIp(String sIpAddress)
        {
            //  Given IP string, validates

            //Boolean IsValidIp = false;
            char[] seps = new char[] { '.' };
            string[] aTmp = sIpAddress.Split(seps);
            //  There must be 4 fields in a valid IP
            if ((aTmp.Length != 3))
            {
                return false;
            }

            foreach (String field in aTmp)
            {
                if ((Convert.ToInt16(field) > 255))
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetLocalTime(DateTime UTCTime, String DateORTime)
        {
            if (DateORTime.ToLower() == "date")
                return UTCTime.ToString("dd/MM/yy");
            else if (DateORTime.ToLower() == "time")
                return UTCTime.ToString("hh:mm tt");
            else
                return "Error in datetime format";
        }

        public static string GetLocalTime(String mUTCTime, String DateORTime)
        {

            if (mUTCTime == "")
                return "";

            DateTime UTCTime = Convert.ToDateTime(mUTCTime);
            if (DateORTime.ToLower() == "date")
                return UTCTime.ToLocalTime().ToString("dd/MM/yy");
            else if (DateORTime.ToLower() == "time")
                return UTCTime.ToLocalTime().ToString("hh:mm tt");
            else
                return "Error in datetime format";
        }

        public static string GetStoreNames(String StoreIDArray, String Separator)
        {
            if (DateTime.Now > ExpDateString)
            {
                //throw new ArgumentException("Argument expired");
                throw new ArgumentException(EvalStatusString);
            }

            if (StoreIDArray == "")
                return "";

            if (StoreIDArray.EndsWith(","))
                StoreIDArray = StoreIDArray.Substring(0, StoreIDArray.Length - 1);

            String sqlStoreNames = "Select WHCode,Location from GEN_WareHouse WHERE isActive=1 AND isDeleted=0 AND WareHouseID IN(" + StoreIDArray + ")";
            IDataReader rsStoreNames = DB.GetRS(sqlStoreNames);
            String resStores = "";

            while (rsStoreNames.Read())
            {
                resStores += rsStoreNames["WHCode"].ToString() + ",";
            }

            rsStoreNames.Close();

            if (resStores.EndsWith(","))
                resStores = resStores.Substring(0, resStores.Length - 1);

            return resStores;

        }


        public static string GetDeliveryStatus(String DeliveryStatusID)
        {
            if (DeliveryStatusID == "")
                return "";
            else
                return DB.GetSqlS("Select DeliveryStatus as S from OBD_DeliveryStatus Where DeliveryStatusID=" + DeliveryStatusID);

        }
        public static string GetStoreNamesWithPNCStatus(String StoreIDArray, String Separator, String RefNumber, String OBDTrackingID, String TenantID)
        {

            string TenantRootDir = "";
            string OutboundPath = "";
            string OBD_DeliveryNotePath = "";
            string OBD_PickandCheckSheetPath = "";
            string OBD_PODPath = "";


            if (StoreIDArray == "")
                return "";


            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();

            if (StoreIDArray.EndsWith(","))
                StoreIDArray = StoreIDArray.Substring(0, StoreIDArray.Length - 1);



            char[] seps = new char[] { ',' };
            String[] ArrStores = StoreIDArray.Split(seps);


            String sqlStoreNames = "Select WareHouseID,WHCode from GEN_WareHouse WHERE isActive=1 AND isDeleted=0 AND WareHouseID IN(" + StoreIDArray + ")";
            IDataReader rsStoreNames = DB.GetRS(sqlStoreNames);
            String resStores = "";

            while (rsStoreNames.Read())
            {
                String SqlIsVerified = "Select SentForPGIOn from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrackingID.ToString() + " AND OB_RefWarehouse_DetailsID =" + DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and OutboundID=" + OBDTrackingID.ToString() + "  and WarehouseID=" + rsStoreNames["WareHouseID"]);

                IDataReader rsIsVerified = DB.GetRS(SqlIsVerified);

                String prmVerifiedLink = "";
                String resStatus = "";



                if (DB.GetSqlN("Select Count(OutboundID) as N from OBD_Outbound Where OutboundID =" + OBDTrackingID + " AND  DeliveryStatusID  NOT IN (3,10)") != 0)
                {
                    if (rsIsVerified.Read())
                    {

                        //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + OutboundPath + OBD_PickandCheckSheetPath, RefNumber + "_" + DB.RSFieldInt(rsStoreNames, "WarehouseID"));

                        String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + OutboundPath + OBD_PickandCheckSheetPath, RefNumber + "_" + DB.RSFieldInt(rsStoreNames, "WarehouseID"));


                        if (sFileName != "")
                        {
                            String Path = "../ViewImage.aspx?path=" + sFileName;

                            // prmVerifiedLink += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + "<font color='#006666'> <nobr> [Verif.On: " + DB.RSFieldDateTime(rsIsVerified, "SentForPGIOn").ToString("dd/MM/yy") + "  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>";
                        }

                        if (prmVerifiedLink == "")
                        {
                            //prmVerifiedLink += "<a style=\"text-decoration:none;\" href=\"#\"   \" > " + "<font color='#006666'> <nobr> [Verif.On: " + DB.RSFieldDateTime(rsIsVerified, "SentForPGIOn").ToString("dd/MM/yy") + "  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>";
                        }

                        resStatus = rsStoreNames["WHCode"].ToString() + "" + Separator + prmVerifiedLink + Separator;
                    }
                    else
                    {

                        resStatus += rsStoreNames["WHCode"].ToString() + " " + Separator + "<font color='#FF1122'>[ PNC Pending ]</font>" + Separator;
                    }
                }
                else
                {
                    resStatus += rsStoreNames["WHCode"].ToString();
                }

                rsIsVerified.Close();
                resStores += resStatus;

            }

            rsStoreNames.Close();

            return resStores;

        }
        //public static string GetStoreNamesWithPNCStatus(String StoreIDArray, String Separator, String RefNumber, String OBDTrackingID, String TenantID)
        //{

        //    string TenantRootDir = "";
        //    string OutboundPath = "";
        //    string OBD_DeliveryNotePath = "";
        //    string OBD_PickandCheckSheetPath = "";
        //    string OBD_PODPath = "";


        //    if (StoreIDArray == "")
        //        return "";


        //    string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

        //    DataSet dsPath = DB.GetDS(query.ToString(), false);

        //    TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
        //    OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
        //    OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
        //    OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
        //    OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();

        //    if (StoreIDArray.EndsWith(","))
        //        StoreIDArray = StoreIDArray.Substring(0, StoreIDArray.Length - 1);



        //    char[] seps = new char[] { ',' };
        //    String[] ArrStores = StoreIDArray.Split(seps);


        //    String sqlStoreNames = "Select WareHouseID,WHCode from GEN_WareHouse WHERE isActive=1 AND isDeleted=0 AND WareHouseID IN(" + StoreIDArray + ")";
        //    IDataReader rsStoreNames = DB.GetRS(sqlStoreNames);
        //    String resStores = "";

        //    while (rsStoreNames.Read())
        //    {
        //        String SqlIsVerified = "Select SentForPGIOn from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrackingID.ToString() + " AND OB_RefWarehouse_DetailsID =" + DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID] @OutboundID=" + OBDTrackingID.ToString() + ",@WarehouseID=" + rsStoreNames["WareHouseID"]);

        //            IDataReader rsIsVerified = DB.GetRS(SqlIsVerified);

        //            String prmVerifiedLink = "";
        //            String resStatus = "";



        //            if (DB.GetSqlN("Select Count(OutboundID) as N from OBD_Outbound Where OutboundID =" + OBDTrackingID + " AND  DeliveryStatusID  NOT IN (3,10)") != 0)
        //            {
        //                if (rsIsVerified.Read())
        //                {

        //                    //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + OutboundPath + OBD_PickandCheckSheetPath, RefNumber + "_" + DB.RSFieldInt(rsStoreNames, "WarehouseID"));

        //                    String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + OutboundPath + OBD_PickandCheckSheetPath, RefNumber + "_" + DB.RSFieldInt(rsStoreNames, "WarehouseID"));


        //                    if (sFileName != "")
        //                    {
        //                        String Path = "../ViewImage.aspx?path=" + sFileName;

        //                        prmVerifiedLink += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + "<font color='#006666'> <nobr> [Verif.On: " + DB.RSFieldDateTime(rsIsVerified, "SentForPGIOn").ToString("dd-MMM-yyyy") + "  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>";
        //                    }

        //                    if (prmVerifiedLink == "") {
        //                        prmVerifiedLink += "<a style=\"text-decoration:none;\" href=\"#\"   \" > " + "<font color='#006666'> <nobr> [Verif.On: " + DB.RSFieldDateTime(rsIsVerified, "SentForPGIOn").ToString("dd-MMM-yyyy") + "  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>";
        //                    }

        //                resStatus = Separator + prmVerifiedLink + Separator;
        //                    rsStoreNames["WHCode"].ToString() + "&nbsp;&nbsp;&nbsp;" + Separator + prmVerifiedLink + Separator;
        //                }
        //                else 
        //                {

        //                resStatus += Separator + "<font color='#FF1122'>[ PNC Pending ]</font>" + Separator;
        //                    rsStoreNames["WHCode"].ToString() + "&nbsp;&nbsp;&nbsp; " + Separator + "<font color='#FF1122'>[ PNC Pending ]</font>" + Separator;
        //                }
        //            }
        //        //else
        //        //{
        //        //    resStatus += rsStoreNames["WHCode"].ToString();  
        //        //}

        //        rsIsVerified.Close();
        //        resStores += resStatus;

        //    }

        //    rsStoreNames.Close();

        //    return resStores;

        //}


        public static string GetStoreNamesWithDeliveryStatus(String StoreIDArray, String Separator, String RefNumber, String OBDTrackingID)
        {

            if (StoreIDArray == "")
                return "";

            if (OBDTrackingID == "")
            {
                return "";
            }
            if (StoreIDArray.EndsWith(","))
                StoreIDArray = StoreIDArray.Substring(0, StoreIDArray.Length - 1);

            char[] seps = new char[] { ',' };
            String[] ArrStores = StoreIDArray.Split(seps);

            String resStores = "";

            foreach (String vStoreID in ArrStores)
            {
                String SqlIsDelivered = "Select COUNT(OBTW.OutboundID) as N from OBD_OutboundTracking_Warehouse OBTW JOIN OBD_RefWarehouse_Details REFD ON REFD.OB_RefWarehouse_DetailsID=OBTW.OB_RefWarehouse_DetailsID AND REFD.IsActive=1 AND REFD.IsDeleted=0 Where   OBTW.IsActive=1 AND OBTW.IsDeleted=0 AND OBTW.OutboundID =" + OBDTrackingID + " AND WarehouseID=" + vStoreID + " AND   DeliveredBy Is NOT NULL AND DeliveryDate IS NOT NULL";

                String prmVerifiedLink = "";
                String resStatus = "";

                if (DB.GetSqlN(SqlIsDelivered) > 0)
                {
                    //prmVerifiedLink = "<a href=\"#\" onclick=\"popup('ViewImage.aspx?obdn=" + RefNumber + "&type=DCR&storeid=" + vStoreID + "');\" return false; class=\"GvLink\"><font color='#006666'><nobr>[ Delivered  <img src=\"images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font></a>";
                    resStatus = GetWareHouseCode(vStoreID) + "&nbsp;" + prmVerifiedLink + Separator;
                }
                else
                {
                    resStatus = GetWareHouseCode(vStoreID) + "&nbsp;<font color='#FF1122'>[Dlv. Pending ]</font>" + Separator;
                }

                resStores += resStatus;
            }

            return resStores;

        }

        public static string GetStoreNamesWithDeliveryStatusForEmailLink(String StoreIDArray, String Separator, String RefNumber, String OBDTrackingID)
        {

            if (StoreIDArray == "")
                return "";

            if (StoreIDArray.EndsWith(","))
                StoreIDArray = StoreIDArray.Substring(0, StoreIDArray.Length - 1);

            char[] seps = new char[] { ',' };
            String[] ArrStores = StoreIDArray.Split(seps);

            String resStores = "";

            foreach (String vStoreID in ArrStores)
            {
                String SqlIsDelivered = "Select Count(OBDTrackingID) as N from OBDTracking_WareHouse Where OBDTrackingID =" + OBDTrackingID + " AND StoreID =" + vStoreID + " AND DeliveredBy Is NOT NULL AND DeliveryDate IS NOT NULL ";

                String prmVerifiedLinkPrivate = "";
                String prmVerifiedLinkPublic = "";
                String resStatus = "";

                if (DB.GetSqlN(SqlIsDelivered) > 0)
                {
                    prmVerifiedLinkPrivate = "<a href=\"http://trackline/ViewImage.aspx?obdn=" + RefNumber + "&type=DCR&storeid=" + vStoreID + "\"> View POD </a>";
                    prmVerifiedLinkPublic = " OR  <a href=\"http://trackline.atc.com.kw/ViewImage.aspx?obdn=" + RefNumber + "&type=DCR&storeid=" + vStoreID + "\" >View POD on Internet</a>";
                    resStatus = GetWareHouseCode(vStoreID) + "&nbsp;" + prmVerifiedLinkPrivate + prmVerifiedLinkPublic + Separator;
                }

                resStores += resStatus;
            }

            return resStores;

        }


        public static string GetStoreNamesWithVerificationStatus(String StoreIDArray, String Separator, String RefNumber, String InboundTrackingID, String TenantID)
        {
            string TenantRootDir = "";
            string InboundPath = "";
            string InbShpDelNotePath = "";
            string InbShpVerifNotePath = "";
            string InbDescPath = "";

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=1";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            InboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            InbShpDelNotePath = dsPath.Tables[0].Rows[2][0].ToString();
            InbShpVerifNotePath = dsPath.Tables[0].Rows[3][0].ToString();
            InbDescPath = dsPath.Tables[0].Rows[1][0].ToString();

            if (StoreIDArray == "")
                return "";


            String IB_TWH_Sql = "select * from INB_InboundTracking_Warehouse where IsActive=1 AND IsDeleted=0 AND InboundID=" + CommonLogic.IIF(InboundTrackingID != "", InboundTrackingID, "0");

            String vrsStatus = "";

            IDataReader rsIB_TWH = DB.GetRS(IB_TWH_Sql);

            while (rsIB_TWH.Read())
            {

                String WH_Code = DB.GetSqlS("select GEN_W.WHCode AS S from INB_RefWarehouse_Details INB_R join GEN_Warehouse GEN_W ON GEN_W.WarehouseID=INB_R.WarehouseID AND GEN_W.IsActive=1 AND GEN_W.IsDeleted=0 where INB_R.IsActive=1 AND INB_R.IsDeleted=0 AND IB_RefWarehouse_DetailsID=" + DB.RSFieldInt(rsIB_TWH, "IB_RefWarehouse_DetailsID"));

                int WH_ID = DB.GetSqlN("select GEN_W.WarehouseID AS N from INB_RefWarehouse_Details INB_R join GEN_Warehouse GEN_W ON GEN_W.WarehouseID=INB_R.WarehouseID AND GEN_W.IsActive=1 AND GEN_W.IsDeleted=0 where INB_R.IsActive=1 AND INB_R.IsDeleted=0 AND IB_RefWarehouse_DetailsID=" + DB.RSFieldInt(rsIB_TWH, "IB_RefWarehouse_DetailsID"));

                if (DB.RSFieldDateTime(rsIB_TWH, "ShipmentVerifiedOn") != System.DateTime.MinValue && DB.RSFieldDateTime(rsIB_TWH, "ShipmentVerifiedOn") != null)
                {

                    String sVerifiedFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + InboundPath + InbShpVerifNotePath, RefNumber + "_" + WH_ID);
                    if (sVerifiedFileName != "")
                    {
                        vrsStatus += WH_Code + Separator;
                        String Path = "../ViewImage.aspx?path=" + sVerifiedFileName;

                        vrsStatus += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + "<font color='#006666'> <nobr> [ Verified  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>" + Separator;
                    }
                    else
                    {

                        vrsStatus += WH_Code + "&nbsp;&nbsp;&nbsp;<br/><font color='#FF1122'><nobr>[ Verified ]</nobr></font>" + Separator;

                    }
                }
                else if (DB.RSFieldDateTime(rsIB_TWH, "ShipmentReceivedOn") != System.DateTime.MinValue && DB.RSFieldDateTime(rsIB_TWH, "ShipmentReceivedOn") != null)
                {
                    vrsStatus += WH_Code + "&nbsp;&nbsp;&nbsp;<br/><font color='#FF1122'><nobr>[ Received ]</nobr></font>" + Separator;
                }


            }

            rsIB_TWH.Close();





            return vrsStatus;



        }


        public static string GetSearchStoreNamesWithVerificationStatus(String StoreIDArray, String Separator, String RefNumber, String InboundTrackingID, String TenantID)
        {

            string TenantRootDir = "";
            string InboundPath = "";
            string InbShpDelNotePath = "";
            string InbShpVerifNotePath = "";
            string InbDescPath = "";


            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=1";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            InboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            InbShpDelNotePath = dsPath.Tables[0].Rows[2][0].ToString();
            InbShpVerifNotePath = dsPath.Tables[0].Rows[3][0].ToString();
            InbDescPath = dsPath.Tables[0].Rows[1][0].ToString();


            if (StoreIDArray == "")
                return "";

            String IB_TWH_Sql = "select IB_RefWarehouse_DetailsID,ShipmentVerifiedOn,ShipmentReceivedOn from INB_InboundTracking_Warehouse where IsActive=1 AND IsDeleted=0 AND InboundID=" + CommonLogic.IIF(InboundTrackingID != "", InboundTrackingID, "0");

            String vrsStatus = "";

            IDataReader rsIB_TWH = DB.GetRS(IB_TWH_Sql);

            while (rsIB_TWH.Read())
            {

                String WH_Code = DB.GetSqlS("select GEN_W.WHCode AS S from INB_RefWarehouse_Details INB_R join GEN_Warehouse GEN_W ON GEN_W.WarehouseID=INB_R.WarehouseID AND GEN_W.IsActive=1 AND GEN_W.IsDeleted=0 where INB_R.IsActive=1 AND INB_R.IsDeleted=0 AND IB_RefWarehouse_DetailsID=" + DB.RSFieldInt(rsIB_TWH, "IB_RefWarehouse_DetailsID"));

                int WH_ID = DB.GetSqlN("select GEN_W.WarehouseID AS N from INB_RefWarehouse_Details INB_R join GEN_Warehouse GEN_W ON GEN_W.WarehouseID=INB_R.WarehouseID AND GEN_W.IsActive=1 AND GEN_W.IsDeleted=0 where INB_R.IsActive=1 AND INB_R.IsDeleted=0 AND IB_RefWarehouse_DetailsID=" + DB.RSFieldInt(rsIB_TWH, "IB_RefWarehouse_DetailsID"));

                if (DB.RSFieldDateTime(rsIB_TWH, "ShipmentVerifiedOn") != System.DateTime.MinValue && DB.RSFieldDateTime(rsIB_TWH, "ShipmentVerifiedOn") != null)
                {

                    String sVerifiedFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + InboundPath + InbShpVerifNotePath, RefNumber + "_" + WH_ID);

                    if (sVerifiedFileName != "")
                    {
                        vrsStatus += WH_Code + Separator;
                        String Path = "../ViewImage.aspx?path=" + sVerifiedFileName;

                        vrsStatus += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + "<font color='#006666'> <nobr> [ Verified  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>" + Separator;
                    }
                    else
                    {

                        vrsStatus += WH_Code + "&nbsp;&nbsp;&nbsp;<br/><font color='#FF1122'><nobr>[ Verified ]</nobr></font>" + Separator;

                    }
                }
                else if (DB.RSFieldDateTime(rsIB_TWH, "ShipmentReceivedOn") != System.DateTime.MinValue && DB.RSFieldDateTime(rsIB_TWH, "ShipmentReceivedOn") != null)
                {
                    vrsStatus += WH_Code + "&nbsp;&nbsp;&nbsp;<br/><font color='#FF1122'><nobr>[ Received ]</nobr></font>" + Separator;
                }


            }

            rsIB_TWH.Close();

            return vrsStatus;

        }




        public static string GetServerPath(String AttachmentType)
        {
            String resServerPAth = "";
            switch (AttachmentType)
            {
                case "DN": // Delivery Note 
                    resServerPAth = CommonLogic.Application("DeliveryNote");
                    break;

                case "PNC": //Pick N Check  Delivery Note 
                    resServerPAth = CommonLogic.Application("PickNCheck");
                    break;

                case "DCR": //Delivery Confirmation Reciept
                    resServerPAth = CommonLogic.Application("DeliveryCR");
                    break;

                case "IBDN": //Inbound Received Note
                    resServerPAth = CommonLogic.Application("InboundDeliveryNote");
                    break;

                case "IBFCN": //Inbound Freight Company Invoice 
                    resServerPAth = CommonLogic.Application("InboundFreightCompanyNote");
                    break;

                case "IBCCN": //Inbound Clearence Company Invoice 
                    resServerPAth = CommonLogic.Application("InboundClearenceCompanyNote");
                    break;

                case "IBSVN": //Inbound Shipment Verification Note
                    resServerPAth = CommonLogic.Application("InboundShipmentVerifyNote");
                    break;

                case "IBDDP": //Inbound Discrepancy Damage Photo
                    resServerPAth = CommonLogic.Application("InboundDiscrepancyDamagePhoto");
                    break;

                case "GRNUF": //Inbound GRN Update File
                    resServerPAth = CommonLogic.Application("GRNUpdateFile");
                    break;

                case "FComm": //Facility Communication File
                    resServerPAth = CommonLogic.Application("FacilityComm");
                    break;

                case "MMPic": //Facility Communication File
                    resServerPAth = CommonLogic.Application("MMPic");
                    break;

                case "RTNF": //Facility Communication File
                    resServerPAth = CommonLogic.Application("ReturnsForm");
                    break;
            }
            return resServerPAth;
        }



        public static string GetAttachmentFile(String AttacmentType, String RefNumber)
        {
            String[] DirectoryFile = { "" };

            DirectoryFile = Directory.GetFiles(SafeMapPath(GetServerPath(AttacmentType)), RefNumber + ".*");

            if (DirectoryFile.Length == 0)
                return "";
            else
                return DirectoryFile[0].ToString();

        }

        public static string GetAttachmentFile(String AttacmentType, String RefNumber, Boolean IsStoreNeeded)
        {
            String[] DirectoryFile = { "" };

            DirectoryFile = Directory.GetFiles(SafeMapPath(GetServerPath(AttacmentType)), RefNumber + "_*.*");

            if (DirectoryFile.Length == 0)
                return "";
            else
                return DirectoryFile[0].ToString();
        }

        public static string GetAttachmentFile(String AttacmentType, String RefNumber, String StoreID)
        {
            String[] DirectoryFile = { "" };

            if (StoreID != "")
                RefNumber = StoreID + "_" + RefNumber;

            DirectoryFile = Directory.GetFiles(SafeMapPath(GetServerPath(AttacmentType)), RefNumber + ".*");

            if (DirectoryFile.Length == 0)
                return "";
            else
                return DirectoryFile[0].ToString();
        }


        public static string GetAttachmentFile(String AttacmentType, String RefNumber, String StoreID, String ItemSNo)
        {
            String[] DirectoryFile = { "" };
            String vOnlyRefNumber = RefNumber;

            if (StoreID != "")
                RefNumber = StoreID + "_" + RefNumber;

            if (ItemSNo != "")
                RefNumber = ItemSNo + "_" + RefNumber;

            switch (AttacmentType)
            {

                case "DN": // Delivery Note 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryNote")), RefNumber + ".*");
                    break;

                case "PNC": //Pick N Check  Delivery Note 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("PickNCheck")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("PickNCheck")), RefNumber + ".*");
                    break;

                case "DCR": //Delivery Confirmation Reciept
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryCR")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryCR")), RefNumber + ".*");
                    break;

                case "IBDN": //Inbound Received Note
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDeliveryNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDeliveryNote")), RefNumber + ".*");
                    break;

                case "IBFCN": //Inbound Freight Company Invoice 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundFreightCompanyNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundFreightCompanyNote")), RefNumber + ".*");
                    break;

                case "IBCCN": //Inbound Clearence Company Invoice 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundClearenceCompanyNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundClearenceCompanyNote")), RefNumber + ".*");
                    break;

                case "IBSVN": //Inbound Shipment Verification Note
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundShipmentVerifyNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundShipmentVerifyNote")), "*" + RefNumber + ".*");
                    break;

                case "IBDDP": //Inbound Discrepancy Damage Photo
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDiscrepancyDamagePhoto")), "*." + vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDiscrepancyDamagePhoto")), "*" + RefNumber + ".*");
                    break;

                case "GRNUF": //Inbound GRN Update File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("GRNUpdateFile")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("GRNUpdateFile")), "*" + RefNumber + ".*");
                    break;

                case "FComm": //Inbound GRN Update File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("FacilityComm")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("FacilityComm")), "*" + RefNumber + ".*");
                    break;

                case "MMPic": //Material Master Picture File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("MMPic")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("MMPic")), "*" + RefNumber + ".*");
                    break;

                case "RTNF": //Facility Communication File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("ReturnsForm")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("ReturnsForm")), "*" + RefNumber + ".*");
                    break;
            }

            if (DirectoryFile.Length == 0)
                return "";
            else
                return DirectoryFile[0].ToString();
        }

        public static string[] GetAttachmentFile(String AttacmentType, String RefNumber, String StoreID, String ItemSNo, String Array)
        {
            String[] DirectoryFile = { "" };
            String vOnlyRefNumber = RefNumber;

            if (StoreID != "")
                RefNumber = StoreID + "_" + RefNumber;

            if (ItemSNo != "")
                RefNumber = ItemSNo + "_" + RefNumber;

            switch (AttacmentType)
            {

                case "DN": // Delivery Note 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryNote")), RefNumber + ".*");
                    break;

                case "PNC": //Pick N Check  Delivery Note 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("PickNCheck")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("PickNCheck")), RefNumber + ".*");
                    break;

                case "DCR": //Delivery Confirmation Reciept
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryCR")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("DeliveryCR")), RefNumber + ".*");
                    break;

                case "IBDN": //Inbound Received Note
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDeliveryNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDeliveryNote")), RefNumber + ".*");
                    break;

                case "IBFCN": //Inbound Freight Company Invoice 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundFreightCompanyNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundFreightCompanyNote")), RefNumber + ".*");
                    break;

                case "IBCCN": //Inbound Clearence Company Invoice 
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundClearenceCompanyNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundClearenceCompanyNote")), RefNumber + ".*");
                    break;

                case "IBSVN": //Inbound Shipment Verification Note
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundShipmentVerifyNote")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundShipmentVerifyNote")), "*" + RefNumber + ".*");
                    break;

                case "IBDDP": //Inbound Discrepancy Damage Photo
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDiscrepancyDamagePhoto")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("InboundDiscrepancyDamagePhoto")), "*" + RefNumber + ".*");
                    break;

                case "GRNUF": //Inbound GRN Update File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("GRNUpdateFile")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("GRNUpdateFile")), "*" + RefNumber + ".*");
                    break;

                case "FComm": //Inbound GRN Update File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("FacilityComm")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("FacilityComm")), "*" + RefNumber + ".*");
                    break;

                case "MMPic": //Material Master Picture File
                    DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("MMPic")), vOnlyRefNumber + ".*");
                    if (DirectoryFile.Length == 0)
                        DirectoryFile = Directory.GetFiles(SafeMapPath(CommonLogic.Application("MMPic")), "*" + RefNumber + ".*");
                    break;
            }

            return DirectoryFile;
        }

        public static void DeleteAttachmentFile(String AttacmentType, String OBDNumber)
        {


            if (GetAttachmentFile(AttacmentType, OBDNumber) != "")
            {
                File.Delete(GetAttachmentFile(AttacmentType, OBDNumber));
            }

        }

        public static bool UploadAttachmentFile(String AttacmentType, FileUpload fuControl, String OBDNumber)
        {
            Boolean VarResult = false;
            if (fuControl.HasFile)
            {

                if (GetAttachmentFile(AttacmentType, OBDNumber) != "")
                {
                    File.Delete(GetAttachmentFile(AttacmentType, OBDNumber));
                }


                try
                {
                    String FileExt = System.IO.Path.GetExtension(fuControl.FileName).ToLower();

                    if (FileExt == ".jpeg" || FileExt == ".jpg" || FileExt == ".gif" || FileExt == ".png" || FileExt == ".xsl" || FileExt == ".xlsx" || FileExt == ".pdf" || FileExt == ".tif" || FileExt == ".xps" || FileExt == ".txt" || FileExt == ".TXT" || FileExt == ".csv")
                    {
                        //If the file extension does not contain in the above list then exit
                        //string filename = Path.GetFileName(fucOBDDeliveryNote.FileName);
                        fuControl.SaveAs(Path.Combine(SafeMapPath(GetServerPath(AttacmentType)), OBDNumber + FileExt));
                        VarResult = true;

                    }
                    else
                    {
                        VarResult = false;
                    }


                }
                catch (Exception ex)
                {
                    VarResult = false;
                    // VarResult = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }

                return VarResult;
            }
            else
            {
                return false;
            }


        }



        public static bool UploadPicFile(String AttacmentType, FileUpload fuControl, String FileName)
        {
            Boolean VarResult = false;
            if (fuControl.HasFile)
            {

                if (GetAttachmentFile(AttacmentType, FileName) != "")
                {
                    File.Delete(GetAttachmentFile(AttacmentType, FileName));
                }


                try
                {
                    String FileExt = System.IO.Path.GetExtension(fuControl.FileName);

                    if (FileExt == ".jpeg" || FileExt == ".jpg" || FileExt == ".gif" || FileExt == ".png" || FileExt == ".tif")
                    {
                        //If the file extension does not contain in the above list then exit
                        //string filename = Path.GetFileName(fucOBDDeliveryNote.FileName);
                        fuControl.SaveAs(Path.Combine(SafeMapPath(GetServerPath(AttacmentType)), FileName + FileExt));
                        VarResult = true;

                    }
                    else
                    {
                        VarResult = false;
                    }


                }
                catch (Exception ex)
                {
                    VarResult = false;
                    // VarResult = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }

                return VarResult;
            }
            else
            {
                return false;
            }


        }




        public static string GetShipmentStatus(String ShipmentStatusID)
        {
            if (ShipmentStatusID == "")
                return "";
            else
                return DB.GetSqlS("Select InboundStatus as S from INB_InboundStatus Where IsActive=1 and IsDeleted=0 and InboundStatusID=" + ShipmentStatusID);

        }

        public static string GetDiscrepancyStatus(String IBDiscrepancyStatusID)
        {
            if (IBDiscrepancyStatusID == "")
                return "";
            else
                return DB.GetSqlS("Select IBDiscrepancyStatus as S from IBDiscrepancyStatus Where IBDiscrepancyStatusID=" + IBDiscrepancyStatusID);

        }

        public static string GetShipmentType(String ShipTypeID)
        {
            if (ShipTypeID == "")
                return "";
            else
                return DB.GetSqlS("Select ShipmentType AS S from GEN_ShipmentType where IsActive=1 and IsDeleted=0 and ShipmentTypeID = " + ShipTypeID);

        }

        public static string GetPlantName(String MMPlantID)
        {
            if (MMPlantID == "")
                return "";
            else
                return DB.GetSqlS("Select Plant AS S from MMT_MPlant where IsActive=1 and IsDeleted=0 and MPlantID = " + MMPlantID);
        }

        public static string GetMaterialGroup(String MMGroupID)
        {
            if (MMGroupID == "")
                return "";
            else
                return DB.GetSqlS("Select MaterialGroup AS S from MMGroup where MMGroupID = " + MMGroupID);
        }

        public static string GetStockType(String StockTypeID)
        {
            if (StockTypeID == "")
                return "";
            else
                return DB.GetSqlS("Select StockType AS S from MMStockType where StockTypeID = " + StockTypeID);
        }

        public static string GetStockTypeCode(String StockTypeID)
        {
            if (StockTypeID == "")
                return "";
            else
                return DB.GetSqlS("Select StockTypeCode AS S from MMT_StockType where StockTypeID = " + StockTypeID);
        }

        public static string GetOBVehicleType(String OBVehicalTypeID)
        {
            if (OBVehicalTypeID == "")
                return "";
            else
                return DB.GetSqlS("Select OBVehicalType AS S from OBVehicalType where OBVehicalTypeID = " + OBVehicalTypeID);

        }

        public static string DCRUpdate(String DocumentTypeID, String DeliveryStatusID, String IsDCRReceived, String DeliveryDate, String RefStoreID, String OBDNumber, String OBDTrackingID, String DriverName)
        {
            String valResult = "";

            if (DeliveryStatusID == "4" || DeliveryStatusID == "11")
            {
                if (IsDCRReceived == "1")
                {
                    valResult = GetStoreNamesWithDeliveryStatus(RefStoreID, "<br/>", OBDNumber, OBDTrackingID) + DeliveryDate + "<br/>By:" + DriverName;
                }
                else
                {
                    valResult = "<font color ='#FF0000'> POD Not Rcvd. </font> <br/>" + DeliveryDate + "<br/>By:" + DriverName;
                }

            }

            return valResult;
        }

        public static string DCRUpdate(String DocumentTypeID, String DeliveryStatusID, String IsDCRReceived, Boolean IsReservationDelivery, String DeliveryDate, String RefStoreID, String OBDNumber, String OBDTrackingID, String DriverName, String AOBVehicalTypeID_1, String AOBVehicalQty_1, String AOBVehicalTypeID_2, String AOBVehicalQty_2, String AOBVehicalTypeID_3, String AOBVehicalQty_3)
        {


            String valResult = "";
            //if (DeliveryStatusID == "4" || DeliveryStatusID == "7")
            if (DocumentTypeID != "4" || (DocumentTypeID == "4" && IsReservationDelivery == true))
            {
                //DeliveryStatusID =4 for Delivered
                //DeliveryStatusID  =11 for Customer Return
                if (DeliveryStatusID == "4" || DeliveryStatusID == "11" || DeliveryStatusID == "7")
                {
                    if (IsDCRReceived == "1")
                    {
                        valResult = GetStoreNamesWithDeliveryStatus(RefStoreID, "<br/>", OBDNumber, OBDTrackingID) + DeliveryDate + "<br/>Driver:" + DriverName;
                    }
                    else
                    {
                        valResult = "<font color ='#FF0000'> POD Not Rcvd. </font> <br/>Delivered On:" + DeliveryDate + "<br/>Driver:" + DriverName;
                    }

                    if (AOBVehicalTypeID_1 != "")
                    {
                        if (AOBVehicalTypeID_1 != "0")
                            valResult += "<br/><font color ='#993300'>" + CommonLogic.GetOBVehicleType(AOBVehicalTypeID_1) + " : " + AOBVehicalQty_1 + "</font>";
                    }

                    if (AOBVehicalTypeID_2 != "")
                    {
                        if (AOBVehicalTypeID_2 != "0")
                            valResult += "<br/><font color ='#993300'>" + CommonLogic.GetOBVehicleType(AOBVehicalTypeID_2) + " : " + AOBVehicalQty_2 + "</font>";
                    }

                    if (AOBVehicalTypeID_3 != "")
                    {
                        if (AOBVehicalTypeID_3 != "0")
                            valResult += "<br/><font color ='#993300'>" + CommonLogic.GetOBVehicleType(AOBVehicalTypeID_3) + " : " + AOBVehicalQty_3 + "</font>";
                    }


                }
            }
            return valResult;
        }

        public static string GetStoreNamesWithDeliveryStatus(String DocumentTypeID, String DeliveryStatusID, Boolean IsReservationDelivery, String RefStoreWHID, String OBDNumber, String OBDTrackingID, String TenantID)
        {
            string TenantRootDir = "";
            string OutboundPath = "";
            string OBD_DeliveryNotePath = "";
            string OBD_PickandCheckSheetPath = "";
            string OBD_PODPath = "";

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();

            String resStores = "";
            String valResult = "";

            if (RefStoreWHID == "")
                return "";

            if (DocumentTypeID != "4" || (DocumentTypeID == "4" && IsReservationDelivery == true))
            {
                if (DeliveryStatusID == "4" || DeliveryStatusID == "11" || DeliveryStatusID == "7")
                {

                    if (RefStoreWHID.EndsWith(","))
                        RefStoreWHID = RefStoreWHID.Substring(0, RefStoreWHID.Length - 1);

                    char[] seps = new char[] { ',' };
                    String[] ArrStores = CommonLogic.FilterSpacesInArrElements(RefStoreWHID.Split(seps));

                    int WHID = DB.GetSqlN("select WarehouseID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and OB_RefWarehouse_DetailsID=" + RefStoreWHID + " and OutboundID=" + OBDTrackingID);

                    String SqlDelvDetails = "Select * from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrackingID + " AND OB_RefWarehouse_DetailsID =" + RefStoreWHID;

                    IDataReader rsDelvDetails = DB.GetRS(SqlDelvDetails);
                    String prmVerifiedLink = "";
                    String resStatus = "";

                    if (rsDelvDetails.Read())
                    {
                        valResult = "";

                        if (DB.RSFieldDateTime(rsDelvDetails, "DeliveryDate") == DateTime.MinValue)
                        {
                            valResult = "";
                            prmVerifiedLink = "Not Delivered.";
                        }

                        resStatus = "<font color='#dd2222'>" + GetWareHouseCode(WHID.ToString()) + "</font><br/>" + prmVerifiedLink + valResult;
                    }
                    else
                    {
                        resStores = "";
                    }

                    resStores += resStatus;
                    if (WHID.ToString() != ArrStores[ArrStores.Length - 1])
                    {
                        resStores += "<br/>";
                    }

                    rsDelvDetails.Close();



                }
            }

            return resStores;
        }



        public static string GetOBSearchStoreNamesWithDeliveryStatus(String DocumentTypeID, String DeliveryStatusID, Boolean IsReservationDelivery, String RefStoreWHID, String OBDNumber, String OBDTrackingID, String TenantID)
        {
            string TenantRootDir = "";
            string OutboundPath = "";
            string OBD_DeliveryNotePath = "";
            string OBD_PickandCheckSheetPath = "";
            string OBD_PODPath = "";


            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();

            String resStores = "";
            String valResult = "";

            if (RefStoreWHID == "")
                return "";

            if (RefStoreWHID.EndsWith(","))
                RefStoreWHID = RefStoreWHID.Substring(0, RefStoreWHID.Length - 1);

            char[] seps = new char[] { ',' };
            String[] ArrStores = CommonLogic.FilterSpacesInArrElements(RefStoreWHID.Split(seps));

            IDataReader rsWH = DB.GetRS("select WHCode,WarehouseID from GEN_Warehouse where IsActive=1 and IsDeleted=0 and WarehouseID IN (" + RefStoreWHID + ")");

            while (rsWH.Read())
            {
                //<!---------------Procedure Converting------------------>
                // int WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + DB.RSFieldInt(rsWH, "WarehouseID") + " and OutboundID=" + OBDTrackingID);

                int WHID = DB.GetSqlN("Exec [dbo].[USP_GetREF_WHID]  @WarehouseID = " + DB.RSFieldInt(rsWH, "WarehouseID") + ", @OutboundID = " + OBDTrackingID);

                //  String SqlDelvDetails = "Select * from OBD_OutboundTracking_Warehouse OBD_TW JOIN OBD_RefWarehouse_Details OBD_RW ON OBD_RW.OutboundID=OBD_TW.OutboundID  Where OBD_TW.OutboundID =" + OBDTrackingID + " AND OBD_TW.OB_RefWarehouse_DetailsID =" + WHID;
                String SqlDelvDetails = "[dbo].[USP_GetDeliveryDateForOBD] @OutboundID=" + OBDTrackingID + ", @REF_WHID = " + WHID;

                IDataReader rsDelvDetails = DB.GetRS(SqlDelvDetails);
                String prmVerifiedLink = "";
                String resStatus = "";

                if (rsDelvDetails.Read())
                {
                    valResult = "";

                    if (DB.RSFieldDateTime(rsDelvDetails, "DeliveryDate") == DateTime.MinValue)
                    {
                        valResult = "";
                        prmVerifiedLink = " <font color='#FF1122'> [  Not Delivered. ] </font>";
                    }
                    else
                    {

                        String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + OutboundPath + OBD_PODPath, OBDNumber + "_" + DB.RSFieldInt(rsDelvDetails, "WarehouseID"));

                        if (sFileName != "")
                        {
                            String Path = "../ViewImage.aspx?path=" + sFileName;

                            prmVerifiedLink += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + "<font color='#006666'> <nobr> [Delivered on : " + DB.RSFieldDateTime(rsDelvDetails, "DeliveryDate").ToString("dd-MMM-yyyy") + "  <img src=\"../Images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font>" + " </a>";
                        }
                        else
                        {
                            prmVerifiedLink = "<font color='#006666'> [ Delivered on :" + DB.RSFieldDateTime(rsDelvDetails, "DeliveryDate").ToString("dd-MMM-yyyy") + "  &nbsp;&nbsp;  <img src='../Images/redarrowright.gif' />   ]</font>";
                        }
                    }

                    resStatus = prmVerifiedLink;
                    //GetWareHouseCode(DB.RSFieldInt(rsWH, "WarehouseID").ToString()) + "<br/>" + prmVerifiedLink;
                }
                else
                {
                    resStores = "";
                }

                resStores += resStatus;
                if (DB.RSFieldInt(rsWH, "WarehouseID").ToString() != ArrStores[ArrStores.Length - 1])
                {
                    resStores += "<br/>";
                }

                rsDelvDetails.Close();

            }

            rsWH.Close();

            /*
                }
            }*/

            return resStores;
        }






        public static string GetUserName(String UserID)
        {
            if (UserID == "")
                return "";
            else
                return DB.GetSqlS("Select  ISNULL(FirstName,'') + ' '  + ISNULL(LastName,'')  AS S from GEN_User where UserID = " + UserID);
        }

        public static string GetUserNameArray(String UserIDArray, String Separator)
        {
            if (UserIDArray == "")
                return "";

            if (UserIDArray.EndsWith(","))
                UserIDArray = UserIDArray.Substring(0, UserIDArray.Length - 1);

            String sqlUserName = "Select FirstName from Users WHERE UserID IN(" + UserIDArray + ")";
            IDataReader rsUserNames = DB.GetRS(sqlUserName);
            String resUsers = "";

            while (rsUserNames.Read())
            {
                resUsers += rsUserNames["FirstName"].ToString() + Separator;
            }

            rsUserNames.Close();

            return resUsers;
        }


        public static string GetUserEmailID(String UserID)
        {
            if (UserID == "")
                return "";
            else
                return DB.GetSqlS("Select Email AS S from GEN_User where UserID = " + UserID);
        }

        public static string GetUserAltEmail1(String UserID)
        {
            if (UserID == "")
                return "";
            else
                return DB.GetSqlS("Select AlternateEmail1 AS S from Users where UserID = " + UserID);
        }

        public static string GetUserAltEmail2(String UserID)
        {
            if (UserID == "")
                return "";
            else
                return DB.GetSqlS("Select AlternateEmail2 AS S from Users where UserID = " + UserID);
        }

        public static string GetDepartmentNameArray(String DepartmentIDArray, String Separator)
        {
            if (DepartmentIDArray == "")
                return "";

            if (DepartmentIDArray.EndsWith(","))
                DepartmentIDArray = DepartmentIDArray.Substring(0, DepartmentIDArray.Length - 1);

            String sqlUserName = "Select Department from MMDepartment WHERE MMDepartmentID IN(" + DepartmentIDArray + ")";
            IDataReader rsUserNames = DB.GetRS(sqlUserName);
            String resUsers = "";

            while (rsUserNames.Read())
            {
                resUsers += rsUserNames["Department"].ToString() + Separator;
            }

            rsUserNames.Close();

            return resUsers;
        }



        public static string GetClearanceCompanyName(String CCID)
        {
            if (CCID == "")
                return "";
            else
                return DB.GetSqlS("Select ClearenceCompany AS S from ClearenceCompany where ClearenceCompanyID = " + CCID);

        }

        public static string GetFreightCompanyName(String FCID)
        {
            if (FCID == "")
                return "";
            else
                return DB.GetSqlS("Select FreightCompany AS S from FreightCompany where FreightCompanyID = " + FCID);
        }

        public static string GetCurrencyCode(String CurrencyID)
        {
            if (CurrencyID == "")
                return "";
            else
                return DB.GetSqlS("Select Code AS S from Currency where CurrencyID = " + CurrencyID);
        }

        public static string GetCC_FR_InvoiceLink(String StoreRefNumber, String RefID, String AttachmentType)
        {
            String retResult = "";
            if (RefID != "")
            {
                if (GetAttachmentFile(AttachmentType, StoreRefNumber) != "")
                {
                    retResult = "<a href=\"#\" onclick=\"popup('ViewImage.aspx?obdn=" + StoreRefNumber + "&type=" + AttachmentType + "');\" return false; class=\"GvLink\"><font color='#006666'>" + RefID + "  <img src=\"images/redarrowright.gif\" border=\"0\" /></font></a>";
                }
                else
                {
                    retResult = RefID;
                }
            }
            return retResult;
        }

        public static string GetFormatedText(object objText, object objChars)
        {
            //return GenFunctions.GetFormatedText(objText, objChars);
            string retString = "";
            if (objText.ToString().Length > int.Parse(objChars.ToString()))
            {
                retString = objText.ToString().Substring(0, int.Parse(objChars.ToString()));
                retString = retString.Trim();
                return retString;
            }
            else
            {
                retString = objText.ToString();
                retString = retString.Trim();
                return retString;
            }
        }

        public static string GetDocumentType(String DocTypeID)
        {
            String RetType = DB.GetSqlS("Select DocumentType AS S from GEN_DocumentType where DocumentTypeID = " + DocTypeID);
            return RetType;
        }

        public static string GetUoM(int UoMID)
        {
            String RetType = DB.GetSqlS("Select MMSKU AS S  from MMSKU  where MMSKUID = " + UoMID.ToString());
            return RetType;
        }

        public static string GetUoM(String UoMID)
        {
            String RetType = DB.GetSqlS("Select MMSKU AS S  from MMSKU  where MMSKUID = " + UoMID);
            return RetType;
        }

        public static string GDRStatus(String IsGDRApproved, String GDRTrackPendingUserID)
        {

            if (IsGDRApproved == "1")
                return "Approved";
            else
                return "Pending With" + GetUserName(GDRTrackPendingUserID);
        }

        public static String GetOBVehicalType(String OBVehicalTypeID)
        {
            return DB.GetSqlS("Select OBVehicalType AS S from OBVehicalType where OBVehicalTypeID = " + OBVehicalTypeID);
        }

        public static String GetPriorityAlert(int prmPriorityLevel, DateTime prmPrioriyTime)
        {
            String RetValue = "";
            TimeSpan span = new TimeSpan();
            if (prmPrioriyTime != null)
            {
                span = prmPrioriyTime.Subtract(DateTime.Now);
            }

            if (span.Days > 0)
            {
                RetValue = span.Days.ToString() + " day(s)";
            }
            else
            {
                if (span.Hours > 0 && span.Hours < 25)
                    RetValue = span.Hours.ToString() + " hour(s)";
            }


            return RetValue;
        }

        public static int RandomNumber(Int32 Maxnumber)
        {
            Random thisNum = new Random();
            return thisNum.Next(Maxnumber);
        }

        public static bool IsValidEmail(string email)
        {
            //regular expression pattern for valid email
            //addresses, allows for the following domains:
            //com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            //Regular expression object
            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;

            //make sure an email address was provided
            if (string.IsNullOrEmpty(email))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(email);
            }
            //return the value to the calling method
            return valid;
        }


        public static String GetChargesType(String ChargesTypeID)
        {

            switch (ChargesTypeID)
            {
                case "1":
                    return "Required";

                case "2":
                    return "Not required";


                case "3":
                    return "To be estimated";


                default:
                    return "";
            }
        }

        public static String GetConsignmentNoteType(string ConsignmentID)
        {

            try
            {

                if (ConsignmentID == "")
                    return "";
                else
                    return DB.GetSqlS("Select  ISNULL(ConsignmentNoteTypeCode,'')  AS S from INB_ConsignmentNoteType where ConsignmentNoteTypeID = " + ConsignmentID);

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public static String GetSupplierName(string SupplierID)
        {

            try
            {

                if (SupplierID == "")
                    return "";
                else
                    return DB.GetSqlS("Select  ISNULL(SupplierName,'')  AS S from MMT_Supplier where SupplierID = " + SupplierID);

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public static String GetProjectedVehicleDetails(string InboundID)
        {

            try
            {

                if (InboundID == "")
                    return "";
                else
                {
                    String value = DB.GetSqlS(" select GEN_E.EquipmentName  AS S  from INB_ProjectedEquipment INB_Pr left join GEN_Equipment GEN_E on GEN_E.EquipmentID=INB_Pr.EquipmentID where INB_Pr.InboundID=" + InboundID);

                    value += " :  " + DB.GetSqlN(" select INB_Pr.ProjectedValue  AS N  from INB_ProjectedEquipment INB_Pr left join GEN_Equipment GEN_E on GEN_E.EquipmentID=INB_Pr.EquipmentID where INB_Pr.InboundID=" + InboundID);


                    return value;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }


        #region ------------------- Export Gridview ---------------------------------------



        public static void ExportGridView(GridView gvResult, String FileNameStartsWith)
        {
            if (DateTime.Now > ExpDateString)
            {
                //throw new ArgumentException("Argument expired");
                throw new ArgumentException(EvalStatusString);
            }

            // PrepareGridViewForExport(gvResult);
            gvResult.AllowPaging = false;
            gvResult.AllowSorting = false;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileNameStartsWith + "_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + ".xls");
            HttpContext.Current.Response.ContentType = "application/vnd.xls";


            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);




            gvResult.RenderControl(htw);

            String excel = "";

            excel = Regex.Replace(htw.InnerWriter.ToString(), "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);

            //Response.Write(sw.ToString());
            HttpContext.Current.Response.Write(excel);

            // Response.Write(sw.ToString());
            HttpContext.Current.Response.End();

        }


        public static void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            HyperLink Hl = new HyperLink();

            string name = String.Empty;



            for (int i = 0; i < gv.Controls.Count; i++)
            {


                if (gv.Controls[i].GetType() == typeof(Literal))
                {

                    l.Text = (gv.Controls[i] as Literal).Text;

                    if (l.Text.IndexOf("<img") > 0)
                    {
                        //l.Replace(htw.InnerWriter.ToString(), "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);
                        //Regex.Replace(l.Text, "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);
                        l.Text = l.Text.Replace("<nobr> [ Verified  <img src=\"images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font></a><br/>", "");
                        //l.Text = l.Text.Replace("<img src='images/redarrowright.gif' border='0' />", "");
                        gv.Controls.AddAt(i, l);
                    }

                }



                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    l.Text = l.Text.Replace("<nobr> Change <img src='images/redarrowright.gif' border='0' /></nobr>", "");
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(TextBox))
                {
                    l.Text = (gv.Controls[i] as TextBox).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                    gv.Controls.Remove(gv.Controls[i]);
                    //gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(HyperLink))
                {
                    l.Text = (gv.Controls[i] as HyperLink).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(System.Web.UI.HtmlControls.HtmlImage))
                {
                    l.Text = (gv.Controls[i] as System.Web.UI.HtmlControls.HtmlImage).Src;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(ImageButton))
                {
                    l.Text = (gv.Controls[i] as System.Web.UI.WebControls.ImageButton).ImageUrl;

                    if (l.Text.Contains("/checked.gif"))
                        l.Text = "Yes";
                    else if (l.Text.Contains("/unchecked.gif"))
                        l.Text = "No";

                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }

                else if (gv.Controls[i].GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                {

                    l.Text = (gv.Controls[i] as System.Web.UI.HtmlControls.HtmlAnchor).InnerText;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }


                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }


            }

        }


        public static void PrepareGridViewForExport(GridView gv, Int16[] HiddenColArray)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            HyperLink Hl = new HyperLink();

            string name = String.Empty;


            gv.AllowPaging = false;
            gv.AllowSorting = false;




            if (HiddenColArray.Length != 0)
            {
                foreach (int colcount in HiddenColArray)
                {
                    gv.Columns[colcount].Visible = false;
                }
            }

            for (int i = 0; i < gv.Controls.Count; i++)
            {


                if (gv.Controls[i].GetType() == typeof(Literal))
                {

                    l.Text = (gv.Controls[i] as Literal).Text;

                    if (l.Text.IndexOf("<img") > 0)
                    {
                        //l.Replace(htw.InnerWriter.ToString(), "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);
                        //Regex.Replace(l.Text, "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);
                        l.Text = l.Text.Replace("<nobr> [ Verified  <img src=\"images/redarrowright.gif\" border=\"0\"  /> ]</nobr></font></a><br/>", "");
                        //l.Text = l.Text.Replace("<img src='images/redarrowright.gif' border='0' />", "");
                        gv.Controls.AddAt(i, l);
                    }

                }


                if (gv.Controls[i].GetType() == typeof(Label))
                {

                    switch ((gv.Controls[i] as Label).ID)
                    {
                        case "lblClearanceBillsSentDate":
                            if ((gv.Controls[i] as Label).Text != "")
                            {
                                l.Text += "<br/>Sent from store: " + (gv.Controls[i] as Label).Text;
                            }
                            break;

                        case "lblClearenceBillReceivedDate":
                            if ((gv.Controls[i] as Label).Text != "")
                            {
                                l.Text += "<br/>Rcvd By Accts.: " + (gv.Controls[i] as Label).Text;
                            }
                            break;

                        case "lblFreightBillsSentDate":
                            if ((gv.Controls[i] as Label).Text != "")
                            {
                                l.Text += "<br/>Sent from store: " + (gv.Controls[i] as Label).Text;
                            }
                            break;

                        case "FreightBillsReceivedDate":
                            if ((gv.Controls[i] as Label).Text != "")
                            {
                                l.Text += "<br/>Rcvd By Accts.: " + (gv.Controls[i] as Label).Text;
                            }
                            break;


                    }

                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }


                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    l.Text = l.Text.Replace("<nobr> Change <img src='images/redarrowright.gif' border='0' /></nobr>", "");
                    gv.Controls.AddAt(i, l);

                }
                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                    gv.Controls.Remove(gv.Controls[i]);
                    // gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(HyperLink))
                {
                    l.Text = (gv.Controls[i] as HyperLink).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(System.Web.UI.HtmlControls.HtmlImage))
                {
                    l.Text = (gv.Controls[i] as System.Web.UI.HtmlControls.HtmlImage).Src;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(ImageButton))
                {
                    l.Text = (gv.Controls[i] as System.Web.UI.HtmlControls.HtmlImage).Src;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }

                else if (gv.Controls[i].GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                {

                    l.Text = (gv.Controls[i] as System.Web.UI.HtmlControls.HtmlAnchor).InnerText;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }


            }

        }


        #endregion



        #region "Bar Code Label Printing"



        public static void SendPrintJob_Big_7p6x5(String MCode, String AltMCode, String OEMPartNo, String ItemDisc, String BatchNo, String SerialNo, String KitID, int KitChildrenCount, String KitParentMCode, decimal Qty, DateTime MfgDate, DateTime ExpDate, String PrinterType, String IPAddress22, String StrRefNo, String OBDNumber, String KitCode, String ReqNo, Boolean IsBoxLabelNeeded, Boolean IsQtyNeedtoPrint, String PrintQty, out String PrintStatus)
        {
            Neodynamic.SDK.Printing.ThermalLabel.LicenseOwner = "Advanced Technology Company-Standard Edition-Team License";
            Neodynamic.SDK.Printing.ThermalLabel.LicenseKey = "LF7WHCA6XQR5ZEQ7YGQ6U9L382FD8QZBDDFEBCUF2CD9MHUTWQRQ";

            int TabRowY = 18;
            int RowHeight = 4;

            int LeftColX = 4;
            int RightColX = 19;

            int fullrowWidth = 68;
            int halfrowWidth = 35;
            int LeftColWidth = 15;
            int RightColFullWidth = 55;

            int RowSpacing = 0;
            int vFontSize = 8;


            // Neodynamic.SDK.Printing.ThermalLabel.LicenseOwner = "Advanced Technology Company-Standard Edition-Team License";
            //Neodynamic.SDK.Printing.ThermalLabel.LicenseKey = "LF7WHCA6XQR5ZEQ7YGQ6U9L382FD8QZBDDFEBCUF2CD9MHUTWQRQ";



            //Define a ThermalLabel object and set unit to millimeter and label size
            Neodynamic.SDK.Printing.ThermalLabel tLabel = new Neodynamic.SDK.Printing.ThermalLabel(Neodynamic.SDK.Printing.UnitType.Mm, 76.2, 50.8);

            //Set the Verical Gap length between labels
            tLabel.GapLength = 0.2;


            // Define Logo and Company Text
            Neodynamic.SDK.Printing.ImageItem rtLogo = new Neodynamic.SDK.Printing.ImageItem(4, 5);

            //Production
            // rtLogo.SourceFile = @"E:\ATCDTrack\images\at_logo_barcode.jpg";

            //Development
            //rtLogo.SourceFile = @"D:\ATCDTrack\ATCDTrack\images\at_logo_barcode.jpg";


            //Development

            rtLogo.SourceFile = @"E:\FalconWMS_RT\Images\RTLogo_VectorJPG.jpg";
            //rtLogo.SourceFile = @"D:\FalconWMS_BranchRT\FalconWMS-BranchRT\FalconWMS\Images\Rossell80_barcode_bw50w.png";
            //rtLogo.SourceFile = @"D:\FalconWMS_BranchRT\FalconWMS-BranchRT\FalconWMS\Images\RTLogo_VectorJPG.jpg";
            // tLabel.Items.Add(rtLogo);


            rtLogo.Width = 11;
            rtLogo.LockAspectRatio = Neodynamic.SDK.Printing.LockAspectRatio.WidthBased;
            //Set monochrome conversion settings... 
            rtLogo.MonochromeSettings.DitherMethod = Neodynamic.SDK.Printing.DitherMethod.Threshold;
            //rtLogo.MonochromeSettings.Threshold = 75;
            rtLogo.MonochromeSettings.Threshold = 75;


            String PrintValue = "";

            //   tLabel.Items.Add(rtLogo);

            if (MCode.EndsWith("-D"))
            {
                MCode = MCode.Split('-')[0];
                PrintValue = "D";
            }
            else if (MCode.EndsWith("-P"))
            {
                //PrintValue = "PR";
                PrintValue = "P"; // As per modification done by swamy 05/11/2014 12:42 PM
            }





            //Define a BarcodeItem...
            Neodynamic.SDK.Printing.BarcodeItem bcItemMcode = new Neodynamic.SDK.Printing.BarcodeItem(LeftColX, 4, 45, 13, Neodynamic.SDK.Printing.BarcodeSymbology.Code128, MCode);//+ txtStoreRefNo.Text);
            //Set the value to encode i.e the user's product id
            bcItemMcode.Code = MCode;



            //Set barcode bars size in CM
            bcItemMcode.BarWidth = 0.25;
            bcItemMcode.BarHeight = 10;
            bcItemMcode.Font.Name = "Arial";
            bcItemMcode.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            bcItemMcode.Font.Size = 7;

            //border settings
            bcItemMcode.BorderThickness = new Neodynamic.SDK.Printing.FrameThickness(0);
            //center barcode inside its container
            bcItemMcode.BarcodeAlignment = Neodynamic.SDK.Printing.BarcodeAlignment.MiddleLeft;
            //disable checksum
            bcItemMcode.AddChecksum = false;
            //hide human readable text
            bcItemMcode.DisplayCode = false;
            tLabel.Items.Add(bcItemMcode);


            if (PrintValue != "")
            {
                // ROW -1 Cell settings...
                TextItem txtRowDamagedCol1 = new TextItem(65, 7, 8, 6, PrintValue);
                //font settings
                txtRowDamagedCol1.Font.Name = "Arial Narrow";
                txtRowDamagedCol1.Font.Bold = false;
                txtRowDamagedCol1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRowDamagedCol1.Font.Size = 15;
                //padding
                txtRowDamagedCol1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRowDamagedCol1.BorderThickness = new FrameThickness(0.2);
                txtRowDamagedCol1.TextAlignment = TextAlignment.Center;

                tLabel.Items.Add(txtRowDamagedCol1);
            }


            // ROW -1 Cell settings...
            TextItem txtRow1Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, " Part #");
            //font settings
            txtRow1Col1.Font.Name = "Arial Narrow";
            txtRow1Col1.Font.Bold = false;
            txtRow1Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow1Col1.Font.Size = vFontSize;
            //padding
            txtRow1Col1.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow1Col1.BorderThickness = new FrameThickness(0.1);
            txtRow1Col1.TextAlignment = TextAlignment.Left;

            TextItem txtRow1Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, " " + MCode); //MCode + AltMCode;
            //font settings
            txtRow1Col2.Font.Name = "Arial Narrow";
            txtRow1Col2.Font.Bold = false;
            txtRow1Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow1Col2.Font.Size = vFontSize;
            //padding
            txtRow1Col2.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow1Col2.BorderThickness = new FrameThickness(0.1);
            txtRow1Col2.TextAlignment = TextAlignment.Left;

            tLabel.Items.Add(txtRow1Col1);
            tLabel.Items.Add(txtRow1Col2);

            TabRowY += RowHeight + RowSpacing;


            // ROW -1 Cell settings...
            TextItem txtOEMRow1Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, " OEM Part #");
            //font settings
            txtOEMRow1Col1.Font.Name = "Arial Narrow";
            txtOEMRow1Col1.Font.Bold = false;
            txtOEMRow1Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtOEMRow1Col1.Font.Size = vFontSize;
            //padding
            txtOEMRow1Col1.TextPadding = new FrameThickness(0.1);
            //set border
            txtOEMRow1Col1.BorderThickness = new FrameThickness(0.1);
            txtOEMRow1Col1.TextAlignment = TextAlignment.Left;

            TextItem txtOEMRow1Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, " " + OEMPartNo); //OEMPartNo
            //font settings
            txtOEMRow1Col2.Font.Name = "Arial Narrow";
            txtOEMRow1Col2.Font.Bold = false;
            txtOEMRow1Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtOEMRow1Col2.Font.Size = vFontSize;
            //padding
            txtOEMRow1Col2.TextPadding = new FrameThickness(0.1);
            //set border
            txtOEMRow1Col2.BorderThickness = new FrameThickness(0.1);
            txtOEMRow1Col2.TextAlignment = TextAlignment.Left;

            tLabel.Items.Add(txtOEMRow1Col1);
            tLabel.Items.Add(txtOEMRow1Col2);

            TabRowY += RowHeight + RowSpacing;



            // ROW -2 Cell settings...

            TextItem txtRow2Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, " Desc.");
            //font settings
            txtRow2Col1.Font.Name = "Arial Narrow";
            txtRow2Col1.Font.Bold = false;
            txtRow2Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow2Col1.Font.Size = vFontSize;
            //padding
            txtRow2Col1.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow2Col1.BorderThickness = new FrameThickness(0.1);
            txtRow2Col1.TextAlignment = TextAlignment.Left;

            TextItem txtRow2Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, " " + ItemDisc); //ItemDisc;
            //font settings
            txtRow2Col2.Font.Name = "Arial Narrow";
            txtRow2Col2.Font.Bold = false;
            txtRow2Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow2Col2.Font.Size = vFontSize;
            //padding
            txtRow2Col2.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow2Col2.BorderThickness = new FrameThickness(0.1);
            txtRow2Col2.TextAlignment = TextAlignment.Left;
            txtRow2Col2.Text.ToLower();

            tLabel.Items.Add(txtRow2Col1);
            tLabel.Items.Add(txtRow2Col2);

            TabRowY += RowHeight + RowSpacing;

            // ROW -3 Cell settings...
            if (BatchNo != "")
            {
                TextItem txtRow3Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, " Batch/Lot #");
                //font settings
                txtRow3Col1.Font.Name = "Arial Narrow";
                txtRow3Col1.Font.Bold = false;
                txtRow3Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow3Col1.Font.Size = vFontSize;
                //padding
                txtRow3Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow3Col1.BorderThickness = new FrameThickness(0.1);
                txtRow3Col1.TextAlignment = TextAlignment.Left;

                TextItem txtRow3Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, " " + BatchNo); //Batch/Lot#;
                //font settings
                txtRow3Col2.Font.Name = "Arial Narrow";
                txtRow3Col2.Font.Bold = false;
                txtRow3Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow3Col2.Font.Size = vFontSize;
                //padding
                txtRow3Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow3Col2.BorderThickness = new FrameThickness(0.1);
                txtRow3Col2.TextAlignment = TextAlignment.Left;

                tLabel.Items.Add(txtRow3Col1);
                tLabel.Items.Add(txtRow3Col2);

                TabRowY += RowHeight + RowSpacing;
            }

            // ROW -4 Cell settings...
            if (SerialNo != "")
            {
                TextItem txtRow4Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, " Serial #"); //ItemDisc;
                //font settings
                txtRow4Col1.Font.Name = "Arial Narrow";
                txtRow4Col1.Font.Bold = false;
                txtRow4Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow4Col1.Font.Size = vFontSize;
                //padding
                txtRow4Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow4Col1.BorderThickness = new FrameThickness(0.1);
                txtRow4Col1.TextAlignment = TextAlignment.Left;

                TextItem txtRow4Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, " " + SerialNo); //ItemDisc;
                //font settings
                txtRow4Col2.Font.Name = "Arial Narrow";
                txtRow4Col2.Font.Bold = false;
                txtRow4Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow4Col2.Font.Size = vFontSize;
                //padding
                txtRow4Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow4Col2.BorderThickness = new FrameThickness(0.1);
                txtRow4Col2.TextAlignment = TextAlignment.Left;

                tLabel.Items.Add(txtRow4Col1);
                tLabel.Items.Add(txtRow4Col2);

                TabRowY += RowHeight + RowSpacing;
            }


            // ROW -5 Cell settings...
            if (KitID != "0")
            {
                TextItem txtRow5Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, " Kit ID");
                //font settings
                txtRow5Col1.Font.Name = "Arial Narrow";
                txtRow5Col1.Font.Bold = false;
                txtRow5Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow5Col1.Font.Size = vFontSize;
                //padding
                txtRow5Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow5Col1.BorderThickness = new FrameThickness(0.1);
                txtRow5Col1.TextAlignment = TextAlignment.Left;

                // Add the ParentMCode next to KitID ifthe ParentMCode != MCode  and ParentMCode !=""
                TextItem txtRow5Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, " " + KitID + (((KitParentMCode != "") && (MCode != KitParentMCode)) ? " / " + KitParentMCode + " [" + KitChildrenCount.ToString() + "]" : "")); // KitID
                //font settings
                txtRow5Col2.Font.Name = "Arial Narrow";
                txtRow5Col2.Font.Bold = false;
                txtRow5Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow5Col2.Font.Size = vFontSize;
                //padding
                txtRow5Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow5Col2.BorderThickness = new FrameThickness(0.1);
                txtRow5Col2.TextAlignment = TextAlignment.Left;

                tLabel.Items.Add(txtRow5Col1);
                tLabel.Items.Add(txtRow5Col2);

                TabRowY += RowHeight + RowSpacing;
            }

            // ROW -6 Cell settings...

            if (MfgDate != DateTime.MinValue || ExpDate != DateTime.MinValue)
            {
                //Col 1
                TextItem txtRow6Col1 = new TextItem(LeftColX, TabRowY, CommonLogic.IIF(ExpDate == DateTime.MinValue, fullrowWidth + 2, (fullrowWidth / 2) + 1), RowHeight, CommonLogic.IIF(MfgDate == DateTime.MinValue, " ", " Mfg.Date : " + MfgDate.ToString("dd MMM yyyy"))); //ItemDisc;
                //font settings
                txtRow6Col1.Font.Name = "Arial Narrow";
                txtRow6Col1.Font.Bold = false;
                txtRow6Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow6Col1.Font.Size = vFontSize;
                //padding
                txtRow6Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow6Col1.BorderThickness = new FrameThickness(0.1);
                txtRow6Col1.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow6Col1);


                if (ExpDate != DateTime.MinValue)
                {
                    //Col2
                    TextItem txtRow6Col2 = new TextItem(halfrowWidth + 4, TabRowY, halfrowWidth, RowHeight, CommonLogic.IIF(ExpDate == DateTime.MinValue, " ", " Exp.Date : " + ExpDate.ToString("dd MMM yyyy"))); //ItemDisc;
                    //font settings
                    txtRow6Col2.Font.Name = "Arial Narrow";
                    txtRow6Col2.Font.Bold = false;
                    txtRow6Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                    txtRow6Col2.Font.Size = vFontSize;
                    //padding
                    txtRow6Col2.TextPadding = new FrameThickness(0.1);
                    //set border
                    txtRow6Col2.BorderThickness = new FrameThickness(0.1);
                    txtRow6Col2.TextAlignment = TextAlignment.Left;
                    tLabel.Items.Add(txtRow6Col2);
                }


                TabRowY += RowHeight + RowSpacing;

            }

            //Row 7 --Call settings
            if (StrRefNo != "")
            {
                TextItem txtRow7Col1 = new TextItem(LeftColX, TabRowY, CommonLogic.IIF(ReqNo == "", fullrowWidth + 2, (fullrowWidth / 2) + 1), RowHeight, CommonLogic.IIF(StrRefNo == "", "", " StrRef # : " + StrRefNo.ToUpper())); //SotreRefNo
                //font settings
                txtRow7Col1.Font.Name = "Arial Narrow";
                txtRow7Col1.Font.Bold = false;
                txtRow7Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow7Col1.Font.Size = vFontSize;
                //padding
                txtRow7Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow7Col1.BorderThickness = new FrameThickness(0.1);
                txtRow7Col1.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow7Col1);

                if (ReqNo != "")
                {

                    TextItem txtRow7Col2 = new TextItem(halfrowWidth + 4, TabRowY, halfrowWidth, RowHeight, CommonLogic.IIF(ReqNo == "", "", " Req.# : " + ReqNo)); // ReqNo;
                    //font settings
                    txtRow7Col2.Font.Name = "Arial Narrow";
                    txtRow7Col2.Font.Bold = false;
                    txtRow7Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                    txtRow7Col2.Font.Size = vFontSize;
                    //padding
                    txtRow7Col2.TextPadding = new FrameThickness(0.1);
                    //set border
                    txtRow7Col2.BorderThickness = new FrameThickness(0.1);
                    txtRow7Col2.TextAlignment = TextAlignment.Left;
                    tLabel.Items.Add(txtRow7Col2);
                }

                TabRowY += RowHeight + RowSpacing;
            }


            //Row 8 --Cell settings
            if (OBDNumber != "")
            {
                TextItem txtRow8Col1 = new TextItem(LeftColX, TabRowY, CommonLogic.IIF(KitCode == "", fullrowWidth + 2, (fullrowWidth / 2) + 1), RowHeight, CommonLogic.IIF(OBDNumber == "", "", " OBD # : " + OBDNumber.ToUpper())); //OBDNumber
                //font settings
                txtRow8Col1.Font.Name = "Arial Narrow";
                txtRow8Col1.Font.Bold = false;
                txtRow8Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow8Col1.Font.Size = vFontSize;
                //padding
                txtRow8Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow8Col1.BorderThickness = new FrameThickness(0.1);
                txtRow8Col1.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow8Col1);

                if (KitCode != "")
                {
                    TextItem txtRow8Col2 = new TextItem(halfrowWidth + 4, TabRowY, halfrowWidth, RowHeight, CommonLogic.IIF(KitCode == "", "", " Kit Code : " + KitCode)); // Kit Code;
                    //font settings
                    txtRow8Col2.Font.Name = "Arial Narrow";
                    txtRow8Col2.Font.Bold = false;
                    txtRow8Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                    txtRow8Col2.Font.Size = vFontSize;
                    //padding
                    txtRow8Col2.TextPadding = new FrameThickness(0.1);
                    //set border
                    txtRow8Col2.BorderThickness = new FrameThickness(0.1);
                    txtRow8Col2.TextAlignment = TextAlignment.Left;
                    tLabel.Items.Add(txtRow8Col2);
                }

                TabRowY += RowHeight + RowSpacing;
            }

            //Row 9 --Cell settings
            if (IsQtyNeedtoPrint == true)
            {
                //+" Qty : " + Qty
                TextItem txtRow9Col1 = new TextItem(LeftColX, TabRowY, fullrowWidth + 2, RowHeight, CommonLogic.IIF(Qty == 0, "", " UoM/Qty. : " + AltMCode + (StrRefNo != "" ? "   Received Qty. :  " : "   Issued  Qty. : ") + PrintQty)); //Quantity
                //font settings
                txtRow9Col1.Font.Name = "Arial Narrow";
                txtRow9Col1.Font.Bold = false;
                txtRow9Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow9Col1.Font.Size = vFontSize;
                //padding
                txtRow9Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow9Col1.BorderThickness = new FrameThickness(0.1);
                txtRow9Col1.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow9Col1);

                /*
                TextItem txtRow9Col2 = new TextItem(halfrowWidth + 4, TabRowY, halfrowWidth, RowHeight, "");
                //font settings
                txtRow9Col2.Font.Name = "Arial Narrow";
                txtRow9Col2.Font.Bold = false;
                txtRow9Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow9Col2.Font.Size = vFontSize;
                //padding
                txtRow9Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow9Col2.BorderThickness = new FrameThickness(0.1);
                txtRow9Col2.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow9Col2);*/

                TabRowY += RowHeight + RowSpacing;
            }



            try
            {
                Neodynamic.SDK.Printing.PrinterSettings thisPrintSetting = new Neodynamic.SDK.Printing.PrinterSettings();
                //Set Thermal Printer resolution
                // thisPrintSetting.Dpi = 300;
                thisPrintSetting.Dpi = 300;

                //Set Thermal Printer language 
                thisPrintSetting.ProgrammingLanguage = Neodynamic.SDK.Printing.ProgrammingLanguage.ZPL;

                if (PrinterType == "USB")
                {
                    //Thermal Printer is connected through USB
                    thisPrintSetting.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.USB;
                    //Set Thermal Printer name 
                    thisPrintSetting.PrinterName = @"\ZDesigner";

                    // txtPrinter.Text.Trim();
                }
                else if (PrinterType == "IP")
                {
                    //Thermal Printer is connected through Network IP
                    thisPrintSetting.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.Network;
                    //Set Thermal Printer network info 

                    //pj.PrinterSettings.Communication.NetworkIPAddress = System.Net.IPAddress.Parse("172.20.109.68");

                    if (IPAddress22.Contains(".org"))
                    {

                        ////Using the Method
                        IPAddress ip = null;
                        //if (GetResolvedConnecionIPAddress("ardhprinter.dyndns.org", out ip))

                        if (GetResolvedConnecionIPAddress(IPAddress22, out ip))
                        {
                            IPAddress22 = ip.ToString();
                        }

                    }
                    //thisPrintSetting.Communication.CommunicationType = System.Net.IPAddress.Parse(IPAddress);
                    thisPrintSetting.Communication.NetworkIPAddress = System.Net.IPAddress.Parse(IPAddress22);
                    thisPrintSetting.Communication.NetworkPort = 9100;
                    thisPrintSetting.Communication.NetworkTimeout = 180;

                }



                //Create a PrintJob object
                Neodynamic.SDK.Printing.PrintJob pj = new Neodynamic.SDK.Printing.PrintJob(thisPrintSetting);

                if (Qty < 1)
                    Qty = 1;

                //Set number of copies...
                if (IsBoxLabelNeeded)
                {
                    pj.Copies = Convert.ToInt32(Qty + 1);
                }
                else
                {
                    pj.Copies = Convert.ToInt32(Qty);
                }

                //Save label to XML file
                // System.IO.File.WriteAllText(@"D:\temp\myLabel.xml", tLabel.GetXmlTemplate());

                // Print COmmand To Printer
                pj.Print(tLabel);

                // Print to PDF for Preview purpose
                // pj.ExportToPdf(tLabel, @"D:\temp\myLabel.pdf", 300);

                PrintStatus = "Success";
            }
            catch (SocketException ex)
            {
                if (ex.Message.StartsWith("A connection attempt failed"))
                    PrintStatus = "Printer is not available to print";
                else
                    PrintStatus = "Network Error";
            }
            catch (Exception ex)
            {


                PrintStatus = ex.HResult.ToString();
            }
        }


        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP)
        {
            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else if (throwIfMoreThanOneIP && addresses.Length > 1)
            {
                throw new ArgumentException(
                    "There is more that one IP address to the specified host.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], port); // Port gets validated here.
        }

        public static void SendPrintJob_Small_5x2p5(String MCode, String AltMCode, String ItemDisc, String BatchNo, String SerialNo, String KitID, int KitChildrenCount, String KitParentMCode, decimal Qty, DateTime MfgDate, DateTime ExpDate, String PrinterType, String IPAddress22, String StrRefNo, String ReqNo, Boolean IsBoxLabelNeeded, out String PrintStatus)
        {
            Neodynamic.SDK.Printing.ThermalLabel.LicenseOwner = "Advanced Technology Company-Standard Edition-Team License";
            Neodynamic.SDK.Printing.ThermalLabel.LicenseKey = "LF7WHCA6XQR5ZEQ7YGQ6U9L382FD8QZBDDFEBCUF2CD9MHUTWQRQ";


            //Define a ThermalLabel object and set unit to millimeter and label size
            Neodynamic.SDK.Printing.ThermalLabel tLabel = new Neodynamic.SDK.Printing.ThermalLabel(Neodynamic.SDK.Printing.UnitType.Mm, 50.2, 25.6);

            //Set the Verical Gap length between labels
            tLabel.GapLength = 0.2;



            int TabRowY = 11;
            int RowHeight = 3;

            int LeftColX = 3;
            int RightColX = 14;

            int fullrowWidth = 47;
            int halfrowWidth = 23;
            int LeftColWidth = 11;
            int RightColFullWidth = 36;
            int RowSpacing = 0;
            int vFontSize = 6;


            /*
            //Add the ATC Company Name
            TextItem txtHeader = new TextItem(12, 1, 50, 2, "Advanced Technology Company");
            //font settings
            txtHeader.Font.Name = "Arial";
            txtHeader.Font.Bold = true;
            txtHeader.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtHeader.Font.Size = vFontSize ;
            tLabel.Items.Add(txtHeader);
            */

            // Define Logo and Company Text
            Neodynamic.SDK.Printing.ImageItem atcLogo = new Neodynamic.SDK.Printing.ImageItem(2, 3);

            //Production
            atcLogo.SourceFile = @"E:\ATCDTrack\images\at_logo_barcode_small.jpg";

            //Development
            // atcLogo.SourceFile = @"D:\ATCDTrack\ATCDTrack\images\at_logo_barcode_small.jpg";

            tLabel.Items.Add(atcLogo);


            //Define a BarcodeItem...
            Neodynamic.SDK.Printing.BarcodeItem bcItemMcode = new Neodynamic.SDK.Printing.BarcodeItem(6, 2, 40, 8, Neodynamic.SDK.Printing.BarcodeSymbology.Code128, MCode);//+ txtStoreRefNo.Text);
            //Set the value to encode i.e the user's product id
            bcItemMcode.Code = MCode;// +txtStoreRefNo.Text;


            //Set barcode bars size in CM
            bcItemMcode.BarWidth = 0.25;
            bcItemMcode.BarHeight = 7;
            bcItemMcode.Font.Name = "Arial";
            bcItemMcode.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            bcItemMcode.Font.Size = 6;
            //border settings
            bcItemMcode.BorderThickness = new Neodynamic.SDK.Printing.FrameThickness(0);
            //center barcode inside its container
            bcItemMcode.BarcodeAlignment = Neodynamic.SDK.Printing.BarcodeAlignment.MiddleCenter;
            //disable checksum
            bcItemMcode.AddChecksum = false;
            //hide human readable text
            bcItemMcode.DisplayCode = false;
            tLabel.Items.Add(bcItemMcode);


            // ROW -1 Cell settings...
            TextItem txtRow1Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, "Part #");
            //font settings
            txtRow1Col1.Font.Name = "Arial Narrow";
            txtRow1Col1.Font.Bold = false;
            txtRow1Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow1Col1.Font.Size = vFontSize;
            //padding
            txtRow1Col1.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow1Col1.BorderThickness = new FrameThickness(0.1);
            txtRow1Col1.TextAlignment = TextAlignment.Left;

            TextItem txtRow1Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, MCode + CommonLogic.IIF(AltMCode != "", "  ," + AltMCode, "")); //MCode + AltMCode;
            //font settings
            txtRow1Col2.Font.Name = "Arial Narrow";
            txtRow1Col2.Font.Bold = false;
            txtRow1Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow1Col2.Font.Size = vFontSize;
            //padding
            txtRow1Col2.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow1Col2.BorderThickness = new FrameThickness(0.1);
            txtRow1Col2.TextAlignment = TextAlignment.Left;

            tLabel.Items.Add(txtRow1Col1);
            tLabel.Items.Add(txtRow1Col2);

            TabRowY += RowHeight + RowSpacing;


            // ROW -2 Cell settings...

            TextItem txtRow2Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, "Description");
            //font settings
            txtRow2Col1.Font.Name = "Arial Narrow";
            txtRow2Col1.Font.Bold = false;
            txtRow2Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow2Col1.Font.Size = vFontSize;
            //padding
            txtRow2Col1.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow2Col1.BorderThickness = new FrameThickness(0.1);
            txtRow2Col1.TextAlignment = TextAlignment.Left;

            TextItem txtRow2Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, ItemDisc); //ItemDisc;
            //font settings
            txtRow2Col2.Font.Name = "Arial Narrow";
            txtRow2Col2.Font.Bold = false;
            txtRow2Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtRow2Col2.Font.Size = vFontSize;
            //padding
            txtRow2Col2.TextPadding = new FrameThickness(0.1);
            //set border
            txtRow2Col2.BorderThickness = new FrameThickness(0.1);
            txtRow2Col2.TextAlignment = TextAlignment.Left;

            tLabel.Items.Add(txtRow2Col1);
            tLabel.Items.Add(txtRow2Col2);

            TabRowY += RowHeight + RowSpacing;



            // ROW -4 Cell settings... Batch NO/ SerialNo
            if (BatchNo != "" || SerialNo != "")
            {
                TextItem txtRow4Col1 = new TextItem(LeftColX, TabRowY, halfrowWidth, RowHeight, (BatchNo != "" ? "Batch # " + BatchNo : " ")); //Batch No;
                //font settings
                txtRow4Col1.Font.Name = "Arial Narrow";
                txtRow4Col1.Font.Bold = false;
                txtRow4Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow4Col1.Font.Size = vFontSize;
                //padding
                txtRow4Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow4Col1.BorderThickness = new FrameThickness(0.1);
                txtRow4Col1.TextAlignment = TextAlignment.Left;

                TextItem txtRow4Col2 = new TextItem(halfrowWidth + 3, TabRowY, halfrowWidth + 1, RowHeight, (SerialNo != "" ? "Serial # " + SerialNo : " ")); //SerialNo;
                //font settings
                txtRow4Col2.Font.Name = "Arial Narrow";
                txtRow4Col2.Font.Bold = false;
                txtRow4Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow4Col2.Font.Size = vFontSize;
                //padding
                txtRow4Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow4Col2.BorderThickness = new FrameThickness(0.1);
                txtRow4Col2.TextAlignment = TextAlignment.Left;

                tLabel.Items.Add(txtRow4Col1);
                tLabel.Items.Add(txtRow4Col2);

                TabRowY += RowHeight + RowSpacing;
            }




            // ROW -5 Cell settings...
            if (MfgDate != DateTime.MinValue || ExpDate != DateTime.MinValue)
            {
                TextItem txtRow6Col1 = new TextItem(LeftColX, TabRowY, halfrowWidth, RowHeight, CommonLogic.IIF(MfgDate == DateTime.MinValue, " ", "Mfg.Date : " + MfgDate.ToString("dd MMM yyyy"))); //Mfg Date;
                //font settings
                txtRow6Col1.Font.Name = "Arial Narrow";
                txtRow6Col1.Font.Bold = false;
                txtRow6Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow6Col1.Font.Size = vFontSize;
                //padding
                txtRow6Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow6Col1.BorderThickness = new FrameThickness(0.1);
                txtRow6Col1.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow6Col1);


                TextItem txtRow6Col2 = new TextItem(halfrowWidth + 3, TabRowY, halfrowWidth + 1, RowHeight, CommonLogic.IIF(ExpDate == DateTime.MinValue, " ", "Exp.Date : " + ExpDate.ToString("dd MMM yyyy"))); //ItemDisc;
                //font settings
                txtRow6Col2.Font.Name = "Arial Narrow";
                txtRow6Col2.Font.Bold = false;
                txtRow6Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow6Col2.Font.Size = vFontSize;
                //padding
                txtRow6Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow6Col2.BorderThickness = new FrameThickness(0.1);
                txtRow6Col2.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow6Col2);


                TabRowY += RowHeight + RowSpacing;
            }

            //Row 7 --Call settings
            if (StrRefNo != "")
            {
                TextItem txtRow7Col1 = new TextItem(LeftColX, TabRowY, halfrowWidth, RowHeight, CommonLogic.IIF(StrRefNo == "", " ", "StrRef. # " + StrRefNo)); //SotreRefNo;
                //font settings
                txtRow7Col1.Font.Name = "Arial Narrow";
                txtRow7Col1.Font.Bold = false;
                txtRow7Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow7Col1.Font.Size = vFontSize;
                //padding
                txtRow7Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow7Col1.BorderThickness = new FrameThickness(0.1);
                txtRow7Col1.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow7Col1);

                TextItem txtRow7Col2 = new TextItem(halfrowWidth + 3, TabRowY, halfrowWidth + 1, RowHeight, CommonLogic.IIF(ReqNo == "", " ", "Req.# " + ReqNo)); //SotreRefNo/ ReqNo;
                //font settings
                txtRow7Col2.Font.Name = "Arial Narrow";
                txtRow7Col2.Font.Bold = false;
                txtRow7Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow7Col2.Font.Size = vFontSize;
                //padding
                txtRow7Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow7Col2.BorderThickness = new FrameThickness(0.1);
                txtRow7Col2.TextAlignment = TextAlignment.Left;
                tLabel.Items.Add(txtRow7Col2);

                TabRowY += RowHeight + RowSpacing;
            }


            // ROW -3 Cell settings...
            if (KitID != "0")
            {
                TextItem txtRow5Col1 = new TextItem(LeftColX, TabRowY, LeftColWidth, RowHeight, "Kit ID");
                //font settings
                txtRow5Col1.Font.Name = "Arial Narrow";
                txtRow5Col1.Font.Bold = false;
                txtRow5Col1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow5Col1.Font.Size = vFontSize;
                //padding
                txtRow5Col1.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow5Col1.BorderThickness = new FrameThickness(0.1);
                txtRow5Col1.TextAlignment = TextAlignment.Left;

                //TextItem txtRow5Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, KitID); // KitID
                TextItem txtRow5Col2 = new TextItem(RightColX, TabRowY, RightColFullWidth, RowHeight, KitID + (((KitParentMCode != "") && (MCode != KitParentMCode)) ? " /" + KitParentMCode + " [" + KitChildrenCount.ToString() + "]" : " ")); // KitID
                //font settings
                txtRow5Col2.Font.Name = "Arial Narrow";
                txtRow5Col2.Font.Bold = false;
                txtRow5Col2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
                txtRow5Col2.Font.Size = vFontSize;
                //padding
                txtRow5Col2.TextPadding = new FrameThickness(0.1);
                //set border
                txtRow5Col2.BorderThickness = new FrameThickness(0.1);
                txtRow5Col2.TextAlignment = TextAlignment.Left;

                tLabel.Items.Add(txtRow5Col1);
                tLabel.Items.Add(txtRow5Col2);


            }
            try
            {


                Neodynamic.SDK.Printing.PrinterSettings thisPrintSetting = new Neodynamic.SDK.Printing.PrinterSettings();
                //Set Thermal Printer resolution
                thisPrintSetting.Dpi = 203;
                //Set Thermal Printer language 
                thisPrintSetting.ProgrammingLanguage = Neodynamic.SDK.Printing.ProgrammingLanguage.ZPL;



                if (PrinterType == "USB")
                {
                    //Thermal Printer is connected through USB
                    thisPrintSetting.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.USB;
                    //Set Thermal Printer name 
                    //pj.PrinterSettings.PrinterName = txtBatchLotNo.Text; //"\\MParkerPC\ZDesigner";
                    thisPrintSetting.PrinterName = @"\\MParkerPC\ZDesigner";

                    // txtPrinter.Text.Trim();
                }
                else if (PrinterType == "IP")
                {
                    //Thermal Printer is connected through Network IP
                    thisPrintSetting.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.Network;
                    //Set Thermal Printer network info 

                    //pj.PrinterSettings.Communication.NetworkIPAddress = System.Net.IPAddress.Parse("172.20.109.68");
                    if (IPAddress22.Contains(".org"))
                    {

                        ////Using the Method
                        IPAddress ip = null;
                        if (GetResolvedConnecionIPAddress(IPAddress22, out ip))
                        {
                            IPAddress22 = ip.Address.ToString();
                        }

                    }

                    thisPrintSetting.Communication.NetworkIPAddress = System.Net.IPAddress.Parse(IPAddress22);
                    // thisPrintSetting.Communication.NetworkIPAddress = System.Net.IPAddress.Parse("10.31.42.190");
                    thisPrintSetting.Communication.NetworkPort = 9100;
                }

                //Create a PrintJob object
                Neodynamic.SDK.Printing.PrintJob pj = new Neodynamic.SDK.Printing.PrintJob(thisPrintSetting);

                //Set number of copies...
                if (IsBoxLabelNeeded)
                {
                    pj.Copies = Convert.ToInt32(Qty + 1);
                }
                else
                {
                    pj.Copies = Convert.ToInt32(Qty);
                }


                //Save label to XML file
                // System.IO.File.WriteAllText(@"D:\temp\myLabel.xml", tLabel.GetXmlTemplate());

                // Print COmmand To Printer
                pj.Print(tLabel);

                // Print to PDF for Prview purpose
                // pj.ExportToPdf(tLabel, @"D:\temp\small_myLabel3.pdf", 300);



                PrintStatus = "Success";
            }
            catch (Exception ex)
            {

                PrintStatus = "Error Printing :" + ex.ToString();
            }


        }


        public static bool GetResolvedConnecionIPAddress(string serverNameOrURL, out IPAddress resolvedIPAddress)
        {
            bool isResolved = false;
            IPHostEntry hostEntry = null;
            IPAddress resolvIP = null;
            try
            {
                if (!IPAddress.TryParse(serverNameOrURL, out resolvIP))
                {
                    hostEntry = Dns.GetHostEntry(serverNameOrURL);

                    if (hostEntry != null && hostEntry.AddressList != null
                                 && hostEntry.AddressList.Length > 0)
                    {
                        if (hostEntry.AddressList.Length == 1)
                        {
                            resolvIP = hostEntry.AddressList[0];
                            isResolved = true;
                        }
                        else
                        {
                            foreach (IPAddress var in hostEntry.AddressList)
                            {
                                if (var.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    resolvIP = var;
                                    isResolved = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    isResolved = true;
                }
            }
            catch (Exception ex)
            {
                isResolved = false;
                resolvIP = null;
            }
            finally
            {
                resolvedIPAddress = resolvIP;
            }

            return isResolved;
        }


        public static void SendPrintJob_Location_2x1p5(String Location, DataTable dt, String IPAddress, out String PrintStatus)
        {
            Neodynamic.SDK.Printing.ThermalLabel.LicenseOwner = "Advanced Technology Company-Standard Edition-Team License";
            Neodynamic.SDK.Printing.ThermalLabel.LicenseKey = "LF7WHCA6XQR5ZEQ7YGQ6U9L382FD8QZBDDFEBCUF2CD9MHUTWQRQ";


            String PrinterType = "IP";

            DataTable dtBCode = new DataTable();

            dtBCode.Columns.Add("BCode");

            dtBCode.Rows.Add("S101A03");
            dtBCode.Rows.Add("S101A04");
            dtBCode.Rows.Add("S101A05");
            dtBCode.Rows.Add("S101A06");


            //Define a ThermalLabel object and set unit to millimeter and label size
            Neodynamic.SDK.Printing.ThermalLabel tLabel = new Neodynamic.SDK.Printing.ThermalLabel(Neodynamic.SDK.Printing.UnitType.Mm, 50.8, 25.1);

            //Set the Verical Gap length between labels
            tLabel.GapLength = 3;
            tLabel.LabelsPerRow = 2;

            //Set the horiz gap between labels
            tLabel.LabelsHorizontalGapLength = 0.1;

            // tLabel.DataSource = dtBCode;
            tLabel.DataSource = dt;





            /*
            //Add the ATC Company Name
            TextItem txtHeader = new TextItem(12, 1, 50, 2, "Advanced Technology Company");
            //font settings
            txtHeader.Font.Name = "Arial";
            txtHeader.Font.Bold = true;
            txtHeader.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            txtHeader.Font.Size = vFontSize ;
            tLabel.Items.Add(txtHeader);
            */

            // Define Logo and Company Text
            Neodynamic.SDK.Printing.ImageItem rtLogo = new Neodynamic.SDK.Printing.ImageItem(2, 3);

            //Production
            // rtLogo.SourceFile = @"E:\ATCDTrack\images\at_logo_barcode_small.jpg";

            //Development
            //rtLogo.SourceFile = @"D:\FalconWMS-BranchRT\FalconWMS\Images\Rossell100_barcode.jpg";
            //tLabel.Items.Add(rtLogo);


            //Define a BarcodeItem...
            Neodynamic.SDK.Printing.BarcodeItem bcItemMcode = new Neodynamic.SDK.Printing.BarcodeItem(0.5, 5, 45, 20, Neodynamic.SDK.Printing.BarcodeSymbology.Code128, "");//+ txtStoreRefNo.Text);
            //Set the value to encode i.e the user's product id
            // bcItemMcode.Code = Location;// +txtStoreRefNo.Text;
            bcItemMcode.DataField = "EmployeeCode";


            //  bcItemMcode.CounterStep = 1;
            //      bcItemMcode.CounterUseLeadingZeros = true;

            //Set barcode bars size in MM
            //Set barcode bars size in CM
            bcItemMcode.BarWidth = 0.35;
            bcItemMcode.BarHeight = 12;

            //in MM
            //bcItemMcode.BarWidth = 0.05;
            //bcItemMcode.BarHeight = 0.7;

            bcItemMcode.Font.Name = "Arial";
            bcItemMcode.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point;
            bcItemMcode.Font.Size = 13;
            //border settings
            //bcItemMcode.BorderThickness = new Neodynamic.SDK.Printing.FrameThickness(0);
            //center barcode inside its container
            bcItemMcode.BarcodeAlignment = Neodynamic.SDK.Printing.BarcodeAlignment.MiddleCenter;
            //disable checksum
            //do not set a quiet zone
            bcItemMcode.QuietZone = new FrameThickness(0);

            //bcItemMcode.AddChecksum = false;
            //hide human readable text
            bcItemMcode.DisplayCode = true;
            tLabel.Items.Add(bcItemMcode);


            try
            {
                using (PrintJob pj = new PrintJob())
                {


                    Neodynamic.SDK.Printing.PrinterSettings thisPrintSetting = new Neodynamic.SDK.Printing.PrinterSettings();
                    //Set Thermal Printer resolution

                    //Thermal Printer is connected through USB
                    thisPrintSetting.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.Network;
                    //thisPrintSetting.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.USB;

                    thisPrintSetting.Dpi = 300;

                    //Set Thermal Printer language 
                    thisPrintSetting.ProgrammingLanguage = Neodynamic.SDK.Printing.ProgrammingLanguage.ZPL;


                    //Set Thermal Printer name 
                    //pj.PrinterSettings.PrinterName = txtBatchLotNo.Text; //"\\MParkerPC\ZDesigner";
                    //thisPrintSetting.PrinterName = @"\\Inventrax6\ZDesigner";
                    thisPrintSetting.PrinterName = "ZDesigner";

                    thisPrintSetting.Communication.NetworkIPAddress = System.Net.IPAddress.Parse(IPAddress);
                    thisPrintSetting.Communication.NetworkPort = 9100;



                    //pj.Copies = 4;


                    //Save label to XML file
                    // System.IO.File.WriteAllText(@"D:\temp\myLabel.xml", tLabel.GetXmlTemplate());
                    // Print COmmand To Printer
                    pj.PrinterSettings = thisPrintSetting;
                    pj.Print(tLabel);

                    // pj.ExportToPdf(tLabel, @"D:\temp\small_myLabel.pdf", 300);

                    PrintStatus = "Success";

                    // Print to PDF for Prview purpose


                }


            }
            catch (SocketException ex)
            {
                if (ex.Message.StartsWith("A connection attempt failed"))
                {
                    PrintStatus = "Printer is not available to print";
                    return;
                }
                else
                {
                    PrintStatus = "Network Error";
                    return;
                }
            }
            catch (Exception ex)
            {

                PrintStatus = ex.HResult.ToString();

                return;
            }


        }


        /*
        public static void ExportToPDF(Neodynamic.SDK.Printing.ThermalLabel _currentThermalLabel, Neodynamic.SDK.Printing.PrintJob pj)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
           // sfd.Filter = "Adobe PDF|*.pdf";
           
                //create a PrintJob object
                    pj.ThermalLabel = _currentThermalLabel; // set the ThermalLabel object

                    pj.ExportToPdf("test.pdf", 203); //export to pdf
                    System.Diagnostics.Process.Start("test.pdf");
        
           
        }
        */
        #endregion


        public static int GetChildrenCount(String vKitID)
        {

            if (vKitID == "")
                return 0;

            if (vKitID == "0")
                return 0;

            int result = 0;
            result = DB.GetSqlN("Select COUNT(KitPlannerID) as N  from KitPlannerDetail  Where KitPlannerID =" + vKitID);
            return result;
        }

        public static void LoadStockType(DropDownList ddlStockType, String selVal)
        {

            ddlStockType.Items.Clear();

            ddlStockType.Items.Add(new ListItem("Select StockType", "0"));
            IDataReader rsStockType = DB.GetRS("Select StockTypeID, StockType from MMStockType Where IsActive =1");

            while (rsStockType.Read())
            {
                ddlStockType.Items.Add(new ListItem(rsStockType["StockType"].ToString(), rsStockType["StockTypeID"].ToString()));

            }

            rsStockType.Close();
            ddlStockType.SelectedValue = selVal;

        }

        public static void LoadPlant(DropDownList ddlPlant, String selVal)
        {

            ddlPlant.Items.Clear();

            ddlPlant.Items.Add(new ListItem("Select Plant", "0"));
            IDataReader rsStockType = DB.GetRS("Select MMPlantID, Plant from MMPlant Where IsActive =1");

            while (rsStockType.Read())
            {
                ddlPlant.Items.Add(new ListItem(rsStockType["Plant"].ToString(), rsStockType["MMPlantID"].ToString()));

            }

            rsStockType.Close();
            ddlPlant.SelectedValue = selVal;

        }

        public static void LoadMMGroup(DropDownList ddlMMGroup, String SelectedValue)
        {
            ddlMMGroup.Items.Clear();
            ddlMMGroup.Items.Add(new ListItem("Sel. MMGroup", "0"));

            IDataReader rsUoM = DB.GetRS("Select MMGroupID, MaterialGroup from MMGroup Where IsActive =1 ORDER BY MaterialGroup");

            while (rsUoM.Read())
            {
                ddlMMGroup.Items.Add(new ListItem(DB.RSField(rsUoM, "MaterialGroup"), DB.RSFieldInt(rsUoM, "MMGroupID").ToString()));
            }
            rsUoM.Close();

            if (SelectedValue != "")
                ddlMMGroup.SelectedValue = SelectedValue;
        }

        public static void LoadUoMList(DropDownList ddlUoM, String SelectedValue)
        {
            ddlUoM.Items.Clear();
            ddlUoM.Items.Add(new ListItem("Sel. UoM", "0"));

            IDataReader rsUoM = DB.GetRS("Select MMSKUID, MMSKU from MMSKU Where IsActive =1 ORDER BY MMSKU");

            while (rsUoM.Read())
            {
                ddlUoM.Items.Add(new ListItem(DB.RSField(rsUoM, "MMSKU"), DB.RSFieldInt(rsUoM, "MMSKUID").ToString()));
            }
            rsUoM.Close(); if (SelectedValue != "")
                ddlUoM.SelectedValue = SelectedValue;
        }

        public static void LoadUoMByMCode(String MCode, DropDownList ddlUoM, String SelectedValue)
        {
            ddlUoM.Items.Clear();
            //ddlUoM.Items.Add(new ListItem("Sel. UoM", "0"));

            IDataReader rsUoM = DB.GetRS("Select UoMID, UoM from dbo.fnGetUoMList(" + DB.SQuote(MCode) + ")");

            ddlUoM.DataSource = rsUoM;
            ddlUoM.DataTextField = "UoM";
            ddlUoM.DataValueField = "UoMID";
            ddlUoM.DataBind();

            if (SelectedValue != "")
                ddlUoM.SelectedValue = SelectedValue;
        }

        public static void LoadUsers(DropDownList ddlUsers, String DefaultText, String SelectedValue)
        {
            ddlUsers.Items.Clear();
            ddlUsers.Items.Add(new ListItem(DefaultText, "0"));

            IDataReader rsUSers = DB.GetRS("Select FirstName, UserId from gen_User Where IsActive=1 ORDER BY FirstName");

            while (rsUSers.Read())
            {
                ddlUsers.Items.Add(new ListItem(DB.RSField(rsUSers, "FirstName"), DB.RSFieldInt(rsUSers, "UserID").ToString()));
            }

            rsUSers.Close();
            if (SelectedValue != "")
                ddlUsers.SelectedValue = SelectedValue;
        }

        public static void LoadCurrency(DropDownList ddlCurrency, String DefaultText, String SelectedValue)
        {
            ddlCurrency.Items.Clear();
            ddlCurrency.Items.Add(new ListItem(DefaultText, "0"));

            IDataReader rsUSers = DB.GetRS("Select CurrencyID, Code from Currency Where IsActive=1 ORDER BY Code");

            while (rsUSers.Read())
            {
                ddlCurrency.Items.Add(new ListItem(DB.RSField(rsUSers, "Code"), DB.RSFieldInt(rsUSers, "CurrencyID").ToString()));
            }

            rsUSers.Close();
            if (SelectedValue != "")
                ddlCurrency.SelectedValue = SelectedValue;
        }
        public static void LoadYear(DropDownList ddlYear, int startyear, int endyear, String SelectedValue)
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Add(new ListItem("YYYY", "0"));
            for (int count = startyear; count <= endyear; count++)
            {
                ddlYear.Items.Add(new ListItem(count.ToString(), count.ToString()));
            }

            if (SelectedValue != "")
                ddlYear.SelectedValue = SelectedValue;
        }

        public static void LoadDay(DropDownList ddlDay, String SelectedValue)
        {
            ddlDay.Items.Clear();
            ddlDay.Items.Add(new ListItem("DD", "0"));

            for (int count = 1; count <= 31; count++)
            {
                ddlDay.Items.Add(new ListItem(count.ToString(), count.ToString()));
            }

            if (SelectedValue != "")
                ddlDay.SelectedValue = SelectedValue;

        }

        public static void LoadMonth(DropDownList ddlMonth, String SelectedValue)
        {
            ddlMonth.Items.Clear();
            ddlMonth.Items.Add(new ListItem("MMM", "0"));

            for (int count = 1; count <= 12; count++)
            {
                ddlMonth.Items.Add(new ListItem(GetMonthName(count, true), count.ToString()));
            }

            if (SelectedValue != "")
                ddlMonth.SelectedValue = SelectedValue;

        }

        private static string GetMonthName(int month, bool abbrev)
        {
            DateTime date = new DateTime(2000, month, 1);
            if (abbrev) return date.ToString("MMM");
            return date.ToString("MMMM");
        }

        public static bool IsDate(Object obj)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static String AppSettings(String SettingName)
        {


            String resultValue = DB.GetSqlS("Select Value as S from ApplicationSettings Where SettingName=" + DB.SQuote(SettingName));
            return resultValue;

        }

        public static bool IsValidLocation(String Location)
        {
            if (Location.Length == 0)
                return false;

            int prmSite = 0;
            if (!Int32.TryParse(Location.Substring(0, 2), out prmSite))
            {
                return false;
            }
            if (!Int32.TryParse(Location.Substring(6, 2), out prmSite))
            {
                return false;
            }


            return true;
        }

        public static string[] GetRolesAllowed(String strRolesAllowed)
        {
            string[] resRolesAllowed;
            ArrayList rolesAllowed = new ArrayList();
            if (strRolesAllowed != "")
            {
                IDataReader rsRolesAllowed;
                if (strRolesAllowed.ToLower() == "all")
                {
                    rsRolesAllowed = DB.GetRS("Select UserRoleID from GEN_UserRole Where IsActive=1 AND IsDeleted=0");
                }
                else
                {
                    rsRolesAllowed = DB.GetRS("Select UserRoleID from GEN_User_UserRole Where IsActive=1 AND UserRoleID IN(Select value from dbo.udf_Split(" + DB.SQuote(strRolesAllowed) + ",','))");
                }

                while (rsRolesAllowed.Read())
                {
                    rolesAllowed.Add(DB.RSFieldInt(rsRolesAllowed, "UserRoleID").ToString());
                }
                rsRolesAllowed.Close();

            }

            resRolesAllowed = (string[])rolesAllowed.ToArray(typeof(string));
            return resRolesAllowed;
        }

        public static string GetRolesAllowedForthisPage(String PageName)
        {
            // String resRoles = DB.GetSqlS("Select UserRoleIDs as S from GEN_MenuLinks Where MenuText='" + PageName + "'");
            String resRoles = DB.GetSqlS("SELECT STUFF((SELECT distinct ', ' + Convert(nvarchar(200),GUML.UserRoleID) FROM GEN_MenuLinks GML JOIN GEN_UserRole_MenuLinks GUML ON GUML.MenuID=GML.MenuID Where MenuText = '" + PageName + "' FOR XML PATH('')),1, 2, '') AS S");

            return resRoles;
        }


        public static bool HasThisUserType(string[] AllowedUserTypeIDs, String CurrentUserTypeIDs)
        {
            if (DateTime.Now > ExpDateString)
            {
                //throw new ArgumentException("Argument expired");
                throw new ArgumentException(EvalStatusString);
            }

            char[] seps = new char[] { ',' };
            String[] ArrUserTypeIDs = CurrentUserTypeIDs.Split(seps);


            foreach (String vUserTypeID in ArrUserTypeIDs)
            {
                if (AllowedUserTypeIDs.Contains(vUserTypeID))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasThisWarehouse(string[] ReferedStoreIDs, String CurrentWarehouseID)
        {


            char[] seps = new char[] { ',' };
            String[] ArrWHIDs = CurrentWarehouseID.Split(seps);


            foreach (String vUserTypeID in ArrWHIDs)
            {
                if (ReferedStoreIDs.Contains(vUserTypeID))
                {
                    return true;
                }
            }

            return false;
        }

        public static void LoadStores(DropDownList ddlWH, String StoreIDValues, int InOutID, Boolean IsAllRequired)
        {
            string selectMMList;
            ddlWH.Items.Clear();


            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

            if (IsAllRequired)
                ddlWH.Items.Add(new ListItem("All", "0"));
            else
                ddlWH.Items.Add(new ListItem("Warehouse", "0"));

            selectMMList = "EXEC [sp_GetWarehouseData] @WarehouseIDs=" + DB.SQuote(StoreIDValues) + ",@InOutID=" + InOutID.ToString();

            selectMMList = selectMMList + ",@AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString() + ",@UserID_New=" + cp.UserID.ToString();

            //IDataReader rsWH = DB.GetRS("EXEC [sp_GetWarehouseData] @WarehouseIDs=" + DB.SQuote(StoreIDValues) + ",@InOutID=" + InOutID.ToString());

            IDataReader rsWH = DB.GetRS(selectMMList);

            String StoreVal = "";
            while (rsWH.Read())
            {
                if (rsWH["Location"].ToString() != "")
                {
                    //StoreVal = rsWH["WHCode"].ToString() + "[" + rsWH["Location"].ToString() + "]";
                    StoreVal = rsWH["WHCode"].ToString();
                }
                else
                {
                    StoreVal = rsWH["WHCode"].ToString();
                }

                ddlWH.Items.Add(new ListItem(StoreVal, rsWH["WareHouseID"].ToString()));
            }
            rsWH.Close();

            if (StoreIDValues != "")
                ddlWH.SelectedIndex = 1;


        }


        public static string[] CommonStores(string[] Array1, string[] Array2)
        {
            string[] commonElements = Array1.Intersect(Array2).ToArray();
            //return String.Join(",", commonElements);
            return commonElements;
        }

        public static String GenerateBarCode(String MCode)
        {
            String resReturn = "";

            if (MCode != "")
            {
                resReturn = "New Bar --" + MCode;
            }
            return resReturn;
        }

        public static String GetKitParentCode(String vKitID)
        {
            if (vKitID != "")
            {
                String resReturn = DB.GetSqlS("Select  MM.MCode as S  from MMT_KitPlanner KP Left JOIN MMT_MaterialMaster MM ON KP.ParentMaterialMasterID =MM.MaterialMasterID WHERE KP.KitPlannerID =" + vKitID);
                return resReturn;
            }
            else
            {
                return "";
            }
        }

        public static String GetUserIP()
        {

            String IPAddress = CommonLogic.ServerVariables("HTTP_X_FORWARDED_FOR");
            if (IPAddress == String.Empty)
                IPAddress = CommonLogic.ServerVariables("REMOTE_ADDR");
            else
                IPAddress = CommonLogic.ServerVariables("REMOTE_HOST");

            return IPAddress;
        }

        public static string[] FilterSpacesInArrElements(string[] ArrWithSpaces)
        {
            ArrayList ArrWithOutSpaces = new ArrayList();

            foreach (String vStoreId in ArrWithSpaces)
            {
                ArrWithOutSpaces.Add(vStoreId.Trim());
            }
            string[] AssStoreIDs = ArrWithOutSpaces.ToArray(typeof(string)) as string[];
            return AssStoreIDs;

        }


        public static void LoadCycleClass(DropDownList ddlCC, String SelectedValue)
        {
            ddlCC.Items.Clear();
            //ddlCC.Items.Add(new ListItem("Sel. CC", "0"));

            IDataReader rsCC = DB.GetRS("Select CycleClassID, CycleClass from QCC_CycleClass Where IsActive=1");

            ddlCC.DataSource = rsCC;
            ddlCC.DataTextField = "CycleClass";
            ddlCC.DataValueField = "CycleClassID";
            ddlCC.DataBind();

            ddlCC.Items.Insert(0, new ListItem("Sel. Cycle Class", "0"));


            if (SelectedValue != "")
                ddlCC.SelectedValue = SelectedValue;

            //rsCC.Close();
        }

        public static void LoadDepartment(DropDownList ddlDiv)
        {

            ddlDiv.Items.Clear();

            ddlDiv.Items.Add(new ListItem("All", "0"));
            IDataReader rsDiv = DB.GetRS("select DepartmentID,Department from GEN_Department where IsActive=1 and IsDeleted=0");

            while (rsDiv.Read())
            {
                ddlDiv.Items.Add(new ListItem(rsDiv["Department"].ToString(), rsDiv["DepartmentID"].ToString()));

            }
            rsDiv.Close();
        }

        public static void LoadLocationCodes(DropDownList ddlLocCodes)
        {

            ddlLocCodes.Items.Clear();
            ddlLocCodes.Items.Add(new ListItem("All", "0"));
            IDataReader rsLocCodes = DB.GetRS("Select LocationCode,LocationCodeID from LocationCode Where IsActive =1");

            while (rsLocCodes.Read())
            {
                ddlLocCodes.Items.Add(new ListItem(DB.RSField(rsLocCodes, "LocationCode"), DB.RSFieldInt(rsLocCodes, "LocationCodeID").ToString("D2")));
            }
            rsLocCodes.Close();


        }

        public static void LoadAisles(DropDownList ddlAisles)
        {

            ddlAisles.Items.Clear();
            ddlAisles.Items.Add(new ListItem("All", "0"));

            for (int vchar = 65; vchar <= 90; vchar++)
            {
                ddlAisles.Items.Add(new ListItem(Convert.ToChar(vchar).ToString(), Convert.ToChar(vchar).ToString()));
            }



        }

        public static void LoadBays(DropDownList ddlBays)
        {

            ddlBays.Items.Clear();
            ddlBays.Items.Add(new ListItem("All", "0"));

            for (int vnum = 1; vnum <= 30; vnum++)
            {
                ddlBays.Items.Add(new ListItem(vnum.ToString("D2"), vnum.ToString("D2")));
            }
        }

        public static void LoadBeams(DropDownList ddlBeams)
        {

            ddlBeams.Items.Clear();
            ddlBeams.Items.Add(new ListItem("All", "0"));

            for (int vchar = 65; vchar <= 90; vchar++)
            {
                ddlBeams.Items.Add(new ListItem(Convert.ToChar(vchar).ToString(), Convert.ToChar(vchar).ToString()));
            }
        }

        public static void LoadLocations(DropDownList ddlLocations)
        {

            ddlLocations.Items.Clear();
            ddlLocations.Items.Add(new ListItem("All", "0"));

            for (int vnum = 1; vnum <= 20; vnum++)
            {
                ddlLocations.Items.Add(new ListItem(vnum.ToString("D2"), vnum.ToString("D2")));
            }
        }


        public static DataTable ImportDataFromExcel(string FilePath, string Extension)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    break;
                case ".xlsx": //Excel 07
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    break;
            }
            conStr = String.Format(conStr, FilePath, 1);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            try
            {
                
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dtExcelRecords = new DataTable();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dtExcelRecords);
                return dtExcelRecords;

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                connExcel.Close();
            }
            

           

            /*
            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            GridView1.DataSource = dt;
            GridView1.DataBind();
            */

        }

        public static Boolean HasSpecialCharacter(String stringToMatch)
        {
            return !Regex.IsMatch(stringToMatch, @"^[a-z A-Z 0-9 \s]+$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.CultureInvariant);
        }

        public static String ConvertDataTableToXML(DataTable tableToExport)
        {
            StringBuilder formattedXML = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, string.Empty, tableToExport.TableName, null);
            DataColumnCollection dtColumns = tableToExport.Columns;

            foreach (DataRow dataItem in tableToExport.Rows)
            {
                XmlElement element = doc.CreateElement("datarow");
                foreach (DataColumn thisColumn in dtColumns)
                {
                    object value = dataItem[thisColumn];
                    XmlElement tmp = doc.CreateElement(thisColumn.ColumnName);
                    if (value != null)
                    {

                        tmp.InnerXml = (HasSpecialCharacter(value.ToString()) ? @"<![CDATA[" + value.ToString() + "]]>" : value.ToString());
                    }
                    else
                    {
                        tmp.InnerXml = string.Empty;
                    }

                    element.AppendChild(tmp);
                }

                node.AppendChild(element);
            }

            doc.AppendChild(node);

            return doc.InnerXml;
        }

        public static T FromXml<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return returnedXmlClass;
        }

        //Creates an object from an XML string.
        public static object FromXml(string Xml, System.Type ObjType)
        {

            XmlSerializer ser;
            ser = new XmlSerializer(ObjType);
            StringReader stringReader;
            stringReader = new StringReader(Xml);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            return obj;

        }

        public static String ToOrdinal(int i, string format)
        {
            String suffix = "th";
            switch (i % 100)
            {
                case 11:
                case 12:
                case 13:
                    //deliberately empty
                    break;
                default:
                    switch (i % 10)
                    {
                        case 1:
                            suffix = "st";
                            break;
                        case 2:
                            suffix = "nd";
                            break;
                        case 3:
                            suffix = "rd";
                            break;
                    }
                    break;
            }
            return i.ToString(format) + suffix;
        }


        public static void LoadEquipment(DropDownList ddlEquipment, String DefaultText, String SelectedValue, int TenantID)
        {
            ddlEquipment.Items.Clear();
            ddlEquipment.Items.Add(new ListItem(DefaultText, "0"));

            IDataReader rsEquipment = DB.GetRS("Select EquipmentName, EquipmentID from GEN_Equipment Where IsActive=1 and IsDeleted=0 AND TenantID = case when  " + TenantID.ToString() + "=0  then TenantID else " + TenantID.ToString() + " end ORDER BY EquipmentName");

            while (rsEquipment.Read())
            {
                ddlEquipment.Items.Add(new ListItem(DB.RSField(rsEquipment, "EquipmentName"), DB.RSFieldInt(rsEquipment, "EquipmentID").ToString()));
            }

            rsEquipment.Close();
            if (SelectedValue != "")
                ddlEquipment.SelectedValue = SelectedValue;
        }


        public static String GetStoreRefNo(String InboundTrackingID)
        {
            return DB.GetSqlS("Select StoreRefNo as S from INB_Inbound Where  IsActive=1 and IsDeleted=0 and InboundID=" + InboundTrackingID);
        }

        public static String GetOBDNo(String OBDTrackingID)
        {
            OBDTrackingID = OBDTrackingID != "" ? OBDTrackingID : "0";
            return DB.GetSqlS("Select OBDNumber as S from OBD_Outbound Where IsActive=1 and IsDeleted=0 and OutboundID=" + OBDTrackingID);
        }


        public static String GetDateTimeFormatWithOutNullTime(String DateTimeString)
        {

            String EndRes = "";

            if (DateTimeString != "")
            {

                DateTime dtConvdate;
                try
                {
                    dtConvdate = DateTime.ParseExact(DateTimeString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    dtConvdate = DateTime.MinValue;
                }

                if (dtConvdate != DateTime.MinValue)
                {
                    if (dtConvdate.ToString("hh:mm tt") == "12:00 AM")
                        EndRes = dtConvdate.ToString(@"dd\/MM\/yy");
                    else
                        EndRes = dtConvdate.ToString(@"dd\/MM\/yy hh\:mm tt");

                }


            }

            return EndRes;

        }



        public static bool UploadFile(String PathName, FileUpload FileControl, String FileName)
        {

            bool status = false;

            if (FileControl.HasFile)
            {
                try
                {
                    var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);

                    var _Directory = new DirectoryInfo(_Path);

                    if (_Directory.Exists == false)
                    {
                        _Directory.Create();
                    }

                    var file = Path.Combine(_Path, FileName);

                    if (File.Exists(file))
                        File.Delete(file);

                    FileControl.SaveAs(file);

                    status = true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return status;
        }


        public static bool UploadMultipleFiles(String PathName, HttpPostedFile FileControl, String FileName)
        {

            bool status = false;

            try
            {
                var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);

                var _Directory = new DirectoryInfo(_Path);

                if (_Directory.Exists == false)
                {
                    _Directory.Create();
                }

                var file = Path.Combine(_Path, FileName);

                if (File.Exists(file))
                    File.Delete(file);

                FileControl.SaveAs(file);

                status = true;

            }
            catch (Exception ex)
            {
                return false;
            }


            return status;
        }

        public static bool UploadFile(String PathName, Byte[] FileControl, String WCFURL)
        {

            bool status = false;


            try
            {
                MemoryStream ms = new MemoryStream(FileControl);

                //var _Path = System.Web.HttpContext.Current.Server.MapPath(WCFURL);
                //var _Path =Uri. WCFURL;
                Uri uri = new Uri(WCFURL);
                //var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);
                var _Path = uri.ToString();
                var _Directory = new DirectoryInfo(_Path.ToString());

                if (_Directory.Exists == false)
                {
                    _Directory.Create();
                }
                // instance a filestream pointing to the
                // storage folder, use the original file name
                // to name the resulting file
                FileStream fs = new FileStream
                    (_Path, FileMode.Create);

                // write the memory stream containing the original
                // file as a byte array to the filestream
                ms.WriteTo(fs);

                // clean up
                ms.Close();
                fs.Close();
                fs.Dispose();

                status = true;

            }
            catch (Exception ex)
            {
                return false;
            }


            return status;
        }



        public static bool _DeleteAttatchment(String PathName, String FileName)
        {

            bool status = false;

            try
            {
                var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);

                var _Directory = new DirectoryInfo(_Path);

                if (_Directory.Exists)
                {
                    var file = Path.Combine(_Path, FileName);

                    if (File.Exists(file))
                        File.Delete(file);

                    status = true;
                }


            }
            catch (Exception ex)
            {
                return false;
            }


            return status;
        }




        public static String _GetAttatchmentFile(String PathName, String FileName)
        {
            try
            {

                var _Path = System.Web.HttpContext.Current.Server.MapPath("~\\" + PathName);

                String[] DirectoryFile = { "" };

                DirectoryFile = Directory.GetFiles(_Path);

                string _FilePath = "";
                string RelativePath = "";

                foreach (string vFileName in DirectoryFile)
                {
                    if (vFileName.Equals(_Path + FileName + Path.GetExtension(vFileName)))
                    {
                        _FilePath = vFileName;

                        RelativePath = PathName + FileName + Path.GetExtension(vFileName);
                        break;
                    }
                }

                return RelativePath;

            }
            catch (Exception ex)
            {
                return "";
            }

        }


        public static String[] _GetMultipleAttatchmentFiles(String PathName)
        {
            try
            {

                var _Path = System.Web.HttpContext.Current.Server.MapPath("~\\" + PathName);

                /*
                String[] DirectoryFile = { "" };

                DirectoryFile = Directory.GetFiles(_Path);


                String[] RelativePath = Directory.GetFiles(_Path);

                int i = 0;
                foreach (string vFileName in DirectoryFile)
                {
                    RelativePath[i] = vFileName;//_Path + vFileName + Path.GetExtension(vFileName);
                    i++;
                }*/

                String[] DirectoryFile = { "" };

                DirectoryFile = Directory.GetFiles(_Path);

                string _FilePath = "";
                string RelativePath = "";

                foreach (string vFileName in DirectoryFile)
                {


                    RelativePath = vFileName.Substring(vFileName.IndexOf("TenantContent"), vFileName.Length - vFileName.IndexOf("TenantContent"));
                }



                return DirectoryFile;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        // Following function will reset all the Passwords in  the User Table
        public static void ResetPasswordswithKey(int TenantID, String Key)
        {
            IDataReader rsUser = DB.GetRS("Select * from GEN_User WHERE TenantID=" + TenantID.ToString());
            StringBuilder sbUser = new StringBuilder();


            while (rsUser.Read())
            {
                // sbUser.Append()
                try
                {
                    DB.ExecuteSQL(" UPDATE GEN_USER SET EnPassword=" + DB.SQuote(Encrypt.EncryptData("inventraxsuperpwd", DB.RSField(rsUser, "Password"))) + " Where UserID = " + DB.RSFieldInt(rsUser, "UserID").ToString() + " AND TenantID=" + TenantID.ToString());
                    //lblLoginStatus.Text += Encrypt.DecryptData("inventraxsuperpwd", DB.RSField(rsUser, "EnPassword")) + "<br/>";
                }
                catch (Exception ex)
                {

                    break;
                }
            }

            rsUser.Close();
        }


        //Following function will return the System Configuration value for a given Parameter and TenantID

        public static string GetConfigValue(int TenantID, String ConfigParm)
        {
            return DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue] @TenantID=" + TenantID.ToString() + ",@SysConfigKey=" + DB.SQuote(ConfigParm));
        }


        public static int GetUserID(String UserName)
        {
            return DB.GetSqlN("Select UserID AS N from GEN_User where FirstName='" + UserName + "' AND IsActive=1 AND IsDeleted=0");
        }


        public static int GetInboundID(String StoreRefNo)
        {
            return DB.GetSqlN("Select InboundID AS N from INB_Inbound where StoreRefNo='" + StoreRefNo + "' AND IsActive=1 AND IsDeleted=0");
        }

        public static int GetOutboundID(String OBDNumber)
        {
            return DB.GetSqlN("Select OutboundID AS N from OBD_Outbound where OBDNumber='" + OBDNumber + "' AND IsActive=1 AND IsDeleted=0");
        }

        public static int GetMMID(String MCode)
        {
            return DB.GetSqlN("Select MaterialMasterID AS N from MMT_MaterialMaster where MCode='" + MCode + "' AND  IsActive=1 AND IsDeleted=0");
        }

        public static bool GoodINStatus(int InboundID)
        {
            bool status = false;

            if (DB.GetSqlN("Declare  @GoodsINStatus int; EXEC [sp_INV_CheckGoodsINStatus] @InboundID=" + InboundID + ",@Status=@GoodsINStatus out; select @GoodsINStatus as N;") != 0)
            {
                status = true;
            }

            return status;

        }

        public static bool GoodOutStatus(int OutboundID)
        {
            bool status = false;

            if (DB.GetSqlN("Declare  @GoodsOutStatus int; EXEC [sp_INV_CheckGoodsOutStatus] @OutboundID=" + OutboundID + ", @Status=@GoodsOutStatus out; select @GoodsOutStatus as N;") != 0)
            {
                status = true;
            }

            return status;

        }


        public static int GetMFGLocationID(String LocationTypeID, String WorkCenterID)
        {
            int LocID = 0;

            try
            {
                LocID = DB.GetSqlN(" select isnull(LOC.MfgLocationID,0) as N from MFG_Location LOC JOIN MFG_LocationType LOCT ON LOCT.LocationTypeID=LOC.LocationTypeID AND LOCT.IsDeleted=0 AND LOCT.IsActive=1 join MFG_WorkCenter WC on LOC.WorkCenterGroupID=WC.WorkCenterGroupID AND WC.IsDeleted=0 AND WC.IsActive=1 join MFG_WorkCenterGroup WCG ON WCG.WorkCenterGroupID=WC.WorkCenterGroupID AND WCG.IsDeleted=0 AND WCG.IsActive=1 WHERE WC.WorkCenterID=" + WorkCenterID + " AND LOCT.LocationTypeID=" + LocationTypeID + " AND LOC.IsDeleted=0 AND LOC.IsActive=1");
            }
            catch (Exception ex)
            {
            }

            return LocID;

        }



        public static int GetScrapCodeID(String ScrapCode)
        {
            int ScrapCodeID = 0;

            try
            {
                ScrapCodeID = DB.GetSqlN("select ScrapCodeID AS N  from MFG_ScrapCode where IsActive=1 AND IsDeleted=0 and ScrapName='" + ScrapCode + "'");
            }
            catch (Exception ex)
            {

            }

            return ScrapCodeID;

        }

        public static int GetPOInvoiceLineItemsCount(int POHeaderID, int SupplierInvoiceID, int TenantID)
        {
            int LineItemsCount = 0;

            try
            {

                IDataReader dr = DB.GetRS("EXEC [dbo].[sp_INB_GetPOInvoiceLineItemsCount] @POHeaderID=" + POHeaderID + ",@TenantID=" + TenantID + ",@SupplierInvoiceID=" + SupplierInvoiceID);

                dr.Read();

                LineItemsCount = DB.RSFieldInt(dr, "LineItemsCount");

                dr.Close();

            }
            catch (Exception ex)
            {

            }

            return LineItemsCount;

        }


        // Expot to Excel Function Beg

        public static void ExporttoExcel(GridView gvResult, String FileNameStartsWith, String[] hiddencolumns)
        {

            if (DateTime.Now > ExpDateString)
            {
                throw new ArgumentException(EvalStatusString);
            }

            if (gvResult.Rows.Count != 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileNameStartsWith + string.Format("{0:dd-MM-yyyy_hh-mm-ss-tt}", DateTime.Now) + ".xls");
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                try
                {

                    gvResult.AllowPaging = false;
                    gvResult.AllowSorting = false;
                    gvResult.FooterRow.Visible = false;
                    //  Hide  Columns Code
                    int hiddencolumnid;
                    //gvResult.FooterRow.Visible = false;
                    //gvResult.HeaderRow.Visible = false;

                    foreach (String hiddencolumn in hiddencolumns)
                    {
                        hiddencolumnid = 0;
                        for (int i = 0; i < gvResult.Columns.Count; i++)
                        {
                            if (gvResult.Rows.Count > i)
                            {
                                GridViewRow row = gvResult.Rows[i];
                                //Apply text style to each Row
                                row.Cells[i].Attributes.Add("style", "textmode");
                            }                           
                            if (gvResult.Columns[i].HeaderText == hiddencolumn)
                            {
                                if (gvResult.Columns[i].HeaderText == "")
                                {
                                    gvResult.Columns[i].HeaderText = "*";
                                }
                                hiddencolumnid = i;
                                break;
                            }
                        }
                        gvResult.Columns[hiddencolumnid].Visible = false;
                    }

                    gvResult.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvResult.HeaderRow.Cells.Count; i++)
                    {
                        gvResult.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
                    }
                    int j = 1;
                    foreach (GridViewRow gvrow in gvResult.Rows)
                    {
                        gvrow.BackColor = System.Drawing.Color.White;
                        foreach (TableCell cell in gvrow.Cells)
                        {
                            cell.Attributes.CssStyle["text-align"] = "center";
                            // cell.Style.Add("mso-style-parent", "style0");
                            cell.Style.Add("mso-number-format", "\\@");
                        }
                        if (j <= gvResult.Rows.Count)
                        {
                            if (j % 2 != 0)
                            {
                                for (int k = 0; k < gvrow.Cells.Count; k++)
                                {
                                    gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");

                                }
                            }
                        }
                        j++;
                    }

                    gvResult.RenderControl(htw);
                    String excel = "";
                    //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    excel = Regex.Replace(htw.InnerWriter.ToString(), "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);
                    //  HttpContext.Current.Response.Write(style);
                    HttpContext.Current.Response.Write(excel.ToString());
                    HttpContext.Current.Response.End();
                    //Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        //End the Export to Excel

        public static void ExporttoExcel1(GridView gvResult, String FileNameStartsWith, String[] hiddencolumns)
        {

            if (DateTime.Now > ExpDateString)
            {
                throw new ArgumentException(EvalStatusString);
            }

            if (gvResult.Rows.Count != 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + FileNameStartsWith + string.Format("{0:dd-MM-yyyy_hh-mm-ss-tt}", DateTime.Now) + ".xls");
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                try
                {

                    gvResult.AllowPaging = false;
                    gvResult.AllowSorting = false;
                    gvResult.FooterRow.Visible = false;
                    //  Hide  Columns Code
                    int hiddencolumnid;
                    //gvResult.FooterRow.Visible = false;
                    //gvResult.HeaderRow.Visible = false;

                    foreach (String hiddencolumn in hiddencolumns)
                    {
                        hiddencolumnid = 0;
                        for (int i = 0; i < gvResult.Columns.Count; i++)
                        {
                           // GridViewRow row = gvResult.Rows[i];
                            //Apply text style to each Row
                            //row.Cells[i].Attributes.Add("style", "textmode");
                            if (gvResult.Columns[i].HeaderText == hiddencolumn)
                            {
                                if (gvResult.Columns[i].HeaderText == "")
                                {
                                    gvResult.Columns[i].HeaderText = "*";
                                }
                                hiddencolumnid = i;
                                break;
                            }
                        }
                        gvResult.Columns[hiddencolumnid].Visible = false;
                    }

                    gvResult.HeaderRow.Style.Add("background-color", "#FFFFFF");

                    for (int i = 0; i < gvResult.HeaderRow.Cells.Count; i++)
                    {
                        gvResult.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
                    }
                    int j = 1;
                    foreach (GridViewRow gvrow in gvResult.Rows)
                    {
                        gvrow.BackColor = System.Drawing.Color.White;
                        foreach (TableCell cell in gvrow.Cells)
                        {
                            cell.Attributes.CssStyle["text-align"] = "center";
                            // cell.Style.Add("mso-style-parent", "style0");
                            cell.Style.Add("mso-number-format", "\\@");
                        }
                        if (j <= gvResult.Rows.Count)
                        {
                            if (j % 2 != 0)
                            {
                                for (int k = 0; k < gvrow.Cells.Count; k++)
                                {
                                    gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");

                                }
                            }
                        }
                        j++;
                    }

                    gvResult.RenderControl(htw);
                    String excel = "";
                    //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    excel = Regex.Replace(htw.InnerWriter.ToString(), "(<a[^>]*>)|(</a>)|(<img[^>]*>)", " ", RegexOptions.IgnoreCase);
                    //  HttpContext.Current.Response.Write(style);
                    HttpContext.Current.Response.Write(excel.ToString());
                    HttpContext.Current.Response.End();
                    //Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        public static Int32 GetReportNameID(String ReportName)
        {

            try
            {

                return DB.GetSqlN("select ReportNameID AS N from Mail_ReportName where ReportName='" + ReportName + "'");
            }
            catch (Exception ex)
            {

                return 0;
            }

        }


        public static void LoadPrinters(DropDownList ddlPrinters)
        {
            try
            {

                ddlPrinters.Items.Clear();

                IDataReader drPrinters = DB.GetRS("select CR.ClientResourceName,CR.DeviceIP from GEN_ClientResource CR JOIN  GEN_DeviceModel DM ON DM.DeviceModelID=CR.DeviceModelID AND DM.IsActive=1 AND DM.IsDeleted=0 JOIN GEN_DeviceType DT ON DT.DeviceTypeID=DM.DeviceTypeID AND DT.IsActive=1 AND DT.IsDeleted=0 where CR.IsActive=1 AND CR.IsDeleted=0 AND DM.DeviceTypeID=3");

                ddlPrinters.Items.Add(new ListItem("Printer", "0"));
                while (drPrinters.Read())
                {
                    ddlPrinters.Items.Add(new ListItem(DB.RSField(drPrinters, "ClientResourceName"), DB.RSField(drPrinters, "DeviceIP")));
                }

                drPrinters.Close();

            }
            catch (Exception ex)
            {

            }
        }

        public static void LoadLabelSizes(DropDownList ddlLabels)
        {
            try
            {

                ddlLabels.Items.Clear();

                string query = "select LabelType,TenantBarcodeTypeID,convert(varchar(50),[Length])+' * '+convert(varchar(50),[Width])+' ('+convert(varchar(100),DPI)+')' LB,Length,Width,DPI from TPL_Tenant_BarcodeType where IsActive=1";


                DataSet ds = DB.GetDS(query, false);
                ddlLabels.Items.Add(new ListItem("Select Label", "0"));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ddlLabels.Items.Add(new ListItem(row["LabelType"].ToString() + "  " + row["LB"].ToString(), row["TenantBarcodeTypeID"].ToString()));
                }


            }
            catch (Exception ex)
            {

            }
        }

        public static int GETDPI(string Printer)
        {
            int DPI = 0;
            try
            {
                if (Printer.ToString() != null)
                {
                    string query = "select [Dpi] as N from GEN_ClientResource where DeviceIP=" + DB.SQuote(Printer);
                    DPI = DB.GetSqlN(query);
                }

            }
            catch (Exception ex)
            {

            }
            return DPI;
        }


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

            String reportTime = Date[1] + "/" + Date[0] + "/" + Date[2] + " " + time[0] + ":" + time[1] + " " + CommonLogic.IIF(Convert.ToInt32(time[0]) >= 12, "PM", "AM");


            return reportTime;
        }
        #region CretingLog File
        public static void CreateLog(string module, string entityType, string entityName, string description, string URL)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("SFTLog.xml");
                XmlNode node = doc.SelectSingleNode("Table");
                AppendNode(module, entityType, entityName, description, node, doc);
                doc.Save("SFTLog.xml");
            }
            catch (System.IO.FileNotFoundException)
            {
                XmlTextWriter writer = new XmlTextWriter("SFTLog.xml", System.Text.Encoding.UTF8);
                writer.WriteStartDocument(false);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("Table");
                createNode(module, entityType, entityName, description, writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

        }
        private static void AppendNode(string module, string entityType, string entityName, string description, XmlNode node, XmlDocument doc)
        {
            XmlElement rootNode = doc.CreateElement("Log");
            XmlElement dateNode = doc.CreateElement("Date");
            dateNode.InnerText = (DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            rootNode.AppendChild(dateNode);

            XmlElement moduleNode = doc.CreateElement("Module");
            moduleNode.InnerText = module;
            rootNode.AppendChild(moduleNode);

            XmlElement entityTypeNode = doc.CreateElement("Entity_Type");
            entityTypeNode.InnerText = entityType;
            rootNode.AppendChild(entityTypeNode);

            XmlElement entityNameNode = doc.CreateElement("EntityName");
            entityNameNode.InnerText = entityName;
            rootNode.AppendChild(entityNameNode);

            XmlElement descriptionNode = doc.CreateElement("Description");
            descriptionNode.InnerText = description;
            rootNode.AppendChild(descriptionNode);


            node.AppendChild(rootNode);
        }


        private static void createNode(string module, string entityType, string entityName, string description, XmlTextWriter writer)
        {

            writer.WriteStartElement("Log");
            writer.WriteStartElement("Date");
            writer.WriteString(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            writer.WriteEndElement();


            writer.WriteStartElement("Module");
            writer.WriteString(module);
            writer.WriteEndElement();

            writer.WriteStartElement("Entity_Type");
            writer.WriteString(entityType);
            writer.WriteEndElement();

            writer.WriteStartElement("EntityName");
            writer.WriteString(entityName);
            writer.WriteEndElement();

            writer.WriteStartElement("Description");
            writer.WriteString(description);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        #endregion



        public static void createErrorNode(string cpID, string ErrorPageName, string source, string ErrorReason, string StackTrace)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/WMSErrorList.xml");

                if (!File.Exists(path))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode rootNode = xmlDoc.CreateElement("Errors");
                    xmlDoc.AppendChild(rootNode);
                    xmlDoc.Save(path);
                }

                XmlDocument xmlErrorRecords = new XmlDocument();
                xmlErrorRecords.Load(path);


                XmlElement ErrorRecord = xmlErrorRecords.CreateElement("Error");

                XmlElement Err_Count = xmlErrorRecords.CreateElement("ErrorCount");
                Err_Count.InnerText = "E" + ErrorCount.ToString();

                XmlElement UserID = xmlErrorRecords.CreateElement("UserID");
                UserID.InnerText = cpID;

                XmlElement E_PageName = xmlErrorRecords.CreateElement("E_PageName");
                E_PageName.InnerText = ErrorPageName;

                XmlElement E_Source = xmlErrorRecords.CreateElement("E_Source");
                E_Source.InnerText = source;

                XmlElement E_Reason = xmlErrorRecords.CreateElement("E_Reason");
                E_Reason.InnerText = ErrorReason;

                XmlElement E_ST = xmlErrorRecords.CreateElement("E_ST");
                E_ST.InnerText = StackTrace;

                XmlElement E_Time = xmlErrorRecords.CreateElement("E_Time");
                E_Time.InnerText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                ErrorRecord.AppendChild(Err_Count);
                ErrorRecord.AppendChild(UserID);
                ErrorRecord.AppendChild(E_PageName);
                ErrorRecord.AppendChild(E_Source);
                ErrorRecord.AppendChild(E_Reason);
                ErrorRecord.AppendChild(E_Time);
                ErrorRecord.AppendChild(E_ST);


                xmlErrorRecords.DocumentElement.InsertBefore(ErrorRecord, xmlErrorRecords.DocumentElement.FirstChild);

                xmlErrorRecords.Save(path);

                ErrorCount++;

            }
            catch (Exception e)
            {

            }

        }

        public static void DeleteErrorNode(string ErrorCount)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/WMSErrorList.xml");

                DataSet ds = new DataSet();
                if (File.Exists(path))
                {

                    ds.ReadXml(path);
                    DataTable firstDT = ds.Tables[0];

                    ds.Tables[0].AsEnumerable().Where(r => r.Field<string>("ErrorCount") == ErrorCount).ToList().ForEach(row => row.Delete());

                    ds.WriteXml(path);
                }
            }
            catch (Exception ex) { }

        }





        #region -------------------------3PL Billing -----------------------------

        public static void LoadTenants(DropDownList ddlTenants)
        {
            try
            {
                ddlTenants.Items.Clear();

                ddlTenants.Items.Add(new ListItem("Select Tenant", "-1"));

                IDataReader rsWH = DB.GetRS("select TenantName,TenantID from TPL_Tenant where IsDeleted=0 AND IsActive=1");

                while (rsWH.Read())
                {
                    ddlTenants.Items.Add(new ListItem(rsWH["TenantName"].ToString(), rsWH["TenantID"].ToString()));
                }
                rsWH.Close();
            }
            catch (Exception ex)
            {
                //resetError("Error while loading stores", true);

            }
        }

        public static void LoadAccount(DropDownList ddlAccount)
        {
            try
            {
                ddlAccount.Items.Clear();

                ddlAccount.Items.Add(new ListItem("Select Account", "-1"));

                IDataReader rsWH = DB.GetRS("select Account,AccountID from GEN_Account where IsDeleted=0 AND IsActive=1");

                while (rsWH.Read())
                {
                    ddlAccount.Items.Add(new ListItem(rsWH["Account"].ToString(), rsWH["AccountID"].ToString()));
                }
                rsWH.Close();
            }
            catch (Exception ex)
            {
                //resetError("Error while loading stores", true);

            }
        }

        public static void LoadUserType(DropDownList ddlUserType)
        {
            try
            {
                ddlUserType.Items.Clear();

                ddlUserType.Items.Add(new ListItem("Select User Type", "-1"));

                IDataReader rsWH = DB.GetRS("select UserType,UserTypeID from GEN_UserType where IsDeleted=0 AND IsActive=1");

                while (rsWH.Read())
                {
                    ddlUserType.Items.Add(new ListItem(rsWH["UserType"].ToString(), rsWH["UserTypeID"].ToString()));
                }
                rsWH.Close();
            }
            catch (Exception ex)
            {
                //resetError("Error while loading stores", true);

            }
        }

        public static bool CheckSuperAdmin(TextBox txtTenant, CustomPrincipal cp, HiddenField hifTenantID)
        {
            hifTenantID.Value = cp.TenantID.ToString();
            if (cp.TenantID != 0)
            {
                txtTenant.Text = cp.TenantName;
                return false;
            }
            return true;
        }

        #endregion -------------------------3PL Billing-----------------------------

        //For Printing Label Using ZPL Added by Prasanna on 21st august 2017

        public static void PrintLabel(TracklineMLabel thisMlabel)
        {

            ZPL zplString = new ZPL();

            int xaxis = 20;
            int yaxis = 20;
            //Barcode

            zplString.addBarcode(xaxis, yaxis, thisMlabel.MCode, 80, BarcodeType.CODE128, false, 2);
            //Draw box
            zplString.drawBox(15, 120, 550, 330, 1);
            //Line
            zplString.drawLine(150, 120, 1, 330, 1);

            yaxis = yaxis + 80;


            //Part #
            yaxis = yaxis + 30;
            zplString.drawText(xaxis, yaxis, 30, 30, "Part#");
            xaxis = xaxis + 150;
            zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.MCode);


            zplString.drawLine(15, yaxis + 40, 530, 1, 1);
            //OEM#

            yaxis = yaxis + 50;
            xaxis = 20;
            zplString.drawText(xaxis, yaxis, 30, 30, "OEM Part#");
            xaxis = xaxis + 150;
            zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.OEMPartNo);

            zplString.drawLine(15, yaxis + 40, 530, 1, 1);
            // DESC
            yaxis = yaxis + 45;
            xaxis = 20;
            zplString.drawText(xaxis, yaxis, 30, 30, "Desc.");
            xaxis = xaxis + 150;
            zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.Description);
            zplString.drawLine(15, yaxis + 40, 530, 1, 1);
            if (thisMlabel.KitPlannerID != 0)
            {
                zplString.drawLine(350, 360, 1, 40, 1);
                xaxis = 350;
                zplString.drawText(xaxis + 20, yaxis, 30, 30, "KIT:");
                xaxis = xaxis + 60;
                zplString.drawText(xaxis + 40, yaxis, 30, 30, thisMlabel.KitPlannerID.ToString());

            }
            if (thisMlabel.BatchNo != "")
            {
                yaxis = yaxis + 45;
                xaxis = 20;
                zplString.drawText(xaxis, yaxis, 30, 30, "Batch/Lot#");
                xaxis = xaxis + 150;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.BatchNo);
                zplString.drawLine(15, yaxis + 40, 530, 1, 1);
            }
            if (thisMlabel.SerialNo != "")
            {
                yaxis = yaxis + 45;
                xaxis = 20;
                zplString.drawText(xaxis, yaxis, 30, 30, "Serial#");
                xaxis = xaxis + 150;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.SerialNo);
                zplString.drawLine(15, yaxis + 40, 530, 1, 1);
            }
            //if (thisMlabel.MfgDate!= DateTime.MinValue)
            //{
            //    if (thisMlabel.MfgDate.ToString() != "")
            //    {
            //        yaxis = yaxis + 45;
            //        xaxis = 20;
            //        zplString.drawText(xaxis, yaxis, 30, 30, "Mfg.Date");
            //        xaxis = xaxis + 150;
            //        zplString.drawText(xaxis, yaxis, 30, 30, CommonLogic.IIF(thisMlabel.MfgDate == DateTime.MinValue, " ", "" + thisMlabel.MfgDate.ToString("dd MMM yyyy")));

            //    }
            //}
            //if(thisMlabel.ExpDate != DateTime.MinValue)
            //{
            //    if (thisMlabel.ExpDate.ToString() != "")
            //    {

            //        xaxis = 350;
            //        zplString.drawText(xaxis, yaxis, 30, 30, "Exp.");
            //        zplString.drawText(xaxis, yaxis+20, 30, 30, "Date:");
            //        xaxis = xaxis + 60;
            //        zplString.drawText(xaxis, yaxis, 30, 30, CommonLogic.IIF(thisMlabel.ExpDate == DateTime.MinValue, " ", "" + thisMlabel.ExpDate.ToString("dd MMM yyyy")));
            //        zplString.drawLine(340, 360, 1, 42, 1);
            //    }
            //}
            //store ref.
            if (thisMlabel.StrRefNo != "")
            {
                yaxis = yaxis + 45;
                xaxis = 20;
                zplString.drawText(xaxis, yaxis, 30, 30, "Store ref#");
                xaxis = xaxis + 150;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.StrRefNo);
            }
            if (thisMlabel.ReqNo != "")
            {

                xaxis = 350;
                zplString.drawText(xaxis, yaxis, 30, 30, "M.R.P:Rs.");
                xaxis = xaxis + 130;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.ReqNo);
                zplString.drawLine(340, yaxis, 1, 42, 1);
            }


            if (thisMlabel.OBDNumber != "")
            {
                yaxis = yaxis + 45;
                xaxis = 20;
                zplString.drawText(xaxis, yaxis, 30, 30, "OBD #");
                xaxis = xaxis + 150;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.OBDNumber);
                zplString.drawLine(15, yaxis + 40, 530, 1, 1);
            }

            if (thisMlabel.Location != "")
            {
                zplString.drawLine(350, 360, 1, 40, 1);
                xaxis = 20;
                zplString.drawText(xaxis, yaxis + 50, 30, 30, "SLOC:");
                xaxis = xaxis + 60;
                zplString.drawText(xaxis + 100, yaxis + 50, 30, 30, thisMlabel.Location);

            }
            if (thisMlabel.IsQtyNeedToPrint == true)
            {

                xaxis = 20;
                yaxis = yaxis + 45;
                zplString.drawText(xaxis, yaxis, 30, 30, "UoM/Qty.");
                xaxis = xaxis + 150;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.AltMCode);
                zplString.drawLine(300, 405, 1, 42, 1);
                xaxis = 300;

                zplString.drawText(xaxis + 20, yaxis, 30, 30, "Issued Qty.:");
                xaxis = xaxis + 180;
                zplString.drawText(xaxis, yaxis, 30, 30, thisMlabel.PrintQty);

            }


            string zpl = zplString.getZPLString();
            //zplString.printUsingIP(thisMlabel.PrinterIP, 9100, 1, 0);



            //


        }

        //[DllImport("alglib.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern string SetTraversePath(string[] LocationNodes, string[] ZNodes, string[] Fl, int Count);
        //public string TraversePath(string[] LocationNodes, string[] ZNodes, string[] Fl, int Count)
        //{
        //    string res = SetTraversePath(LocationNodes, ZNodes, Fl, Count);
        //    return res;

        //}
        [DllImport("alglib.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string SetTraversePath(string[] LocationNodes, string[] ZNodes, string[] Fl, int Count);
        public static string LocationStr(string Loctions, string VLPDID, string OutboundId)
        {
            int Cnt = 0;
            string result = "";
            try
            {
                string[] locationCodinates = Loctions.ToString().Split(',');
                string fs = null;
                for (int x = 0; x < locationCodinates.Count(); x++)
                {
                    fs = fs + "'" + locationCodinates[x] + "',";
                }
                fs = fs.Remove(fs.Length - 1);
                string[] loc = new string[3000], xs = new string[3000], ys = new string[3000], zs = new string[3000], fl = new string[3000];
                string query = "select * from  INV_Location where Location in (" + fs + ") order by FloorLevelIdentity asc";
                DataSet ds = DB.GetDS(query, false);
                Cnt = ds.Tables[0].Rows.Count;
                for (int axies = 0; axies < Cnt; axies++)
                {
                    loc[axies] = Convert.ToString(ds.Tables[0].Rows[axies][1]);
                    zs[axies] = Convert.ToString(ds.Tables[0].Rows[axies][20]);
                    fl[axies] = Convert.ToString(ds.Tables[0].Rows[axies][23]);
                }
                string res = SetTraversePath(loc, zs, fl, Cnt);

                DB.ExecuteSQL("EXEC GenerateDeliveryPickingSeq @VLPDID = " + VLPDID + ", @OutboundID=" + OutboundId + ", @LocationString = " + DB.SQuote(res) + "");
                result = "success";
            }
            catch (Exception ex)
            {
                result = "Error while gerenate picking list and Error is :" + ex.Message + "";
            }


            // finallocationstrng = res;
            return result;

        }

        //================== Added by MD.Prasad ===================//
        //Decrypt Password //
        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        //Encript Password //
        public static string EncryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        //================== Added by MD.Prasad ===================//




        public static string Formateddate(string inputdate)
        {
            inputdate = inputdate.Replace('-', '/');

            inputdate = inputdate.Split(' ')[0].ToString();
            string[] str = inputdate.Split('/');


            if (Convert.ToInt32(str[1]) > 12)
            {
                DateTime exdate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                inputdate = exdate.ToString("MM/dd/yyyy");
            }

            else
            {
                DateTime exdate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[1]), Convert.ToInt32(str[0]));
                inputdate = exdate.ToString("MM/dd/yyyy");
            }

            return inputdate;
        }

        // added by lalitha on 08/02/2019 //


        // added by lalitha on 26/02/2019 for download the excel //
        public static void ExportExcelData(DataTable dtSrc, string fileName, List<int> notReqiredCoulumnIndex)
        {

            try
            {

                string strOperationNumber = string.Empty;
                ExcelPackage objExcelPackage = new ExcelPackage();



                string sheetName = fileName;

                //Create the worksheet    
                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(sheetName);
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                //Rows
                //Adding Headers

                for (int index = 0; index < dtSrc.Rows.Count; index++)
                {
                    //Columns
                    for (int colindex = 0; colindex < dtSrc.Columns.Count; colindex++)
                    {
                        // Checking for row index and wring header data
                        if (index == 0)
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                            {

                                objWorksheet.Cells[index + 1, colindex + 1].RichText.Text = dtSrc.Columns[colindex].Caption;
                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }

                        }
                        else
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                            {

                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }
                        }

                    }
                }
                objWorksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 12));
                objWorksheet.Cells.AutoFitColumns();
                //Format the header    
                using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }



                try
                {
                    string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                    objExcelPackage.SaveAs(new System.IO.FileInfo(strUploadPath + "\\" + fileName + ".xlsx"));
                    // System.Diagnostics.Process.Start("Data.xlsx");

                }
                catch (Exception ex)
                {


                }

            }

            catch (Exception ex)
            {


                string Query = " INSERT INTO GEN_ErrorLog(Module, Message) ";
                Query += "SELECT 'CCDETAILS EXPORT','" + ex.InnerException + "'";
                DB.ExecuteSQL(Query);
                throw ex;

            }

        }

        // added by lalitha on 26/02/2019 for download the excel //


        //================== Added by M.D.Prasad ====================//

        public static void ExportExcelDataForReports(DataTable dtSrc, string fileName, List<int> notReqiredCoulumnIndex, string PageName)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            try
            {

                string strOperationNumber = string.Empty;
                ExcelPackage objExcelPackage = new ExcelPackage();


                int rowIndex = 0;
                int colIndex = 0;
                int PixelTop = 88;
                int PixelLeft = 129;
                int Height = 38;
                int Width = 150;
                string sheetName = fileName;

                //Create the worksheet    
                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(sheetName);
                //System.Drawing.Image img = System.Drawing.Image.FromFile(@"E:\logo.png");
                string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
                // string filePath = SafeMapPath("~/Images/inventrax.jpg");
                string filePath = SafeMapPath("~/TPL/AccountLogos/"+ logo);
                objWorksheet.Cells[1, 1, 2, 2].Merge = true;
                System.Drawing.Image img = System.Drawing.Image.FromFile(filePath);
                ExcelPicture pic = objWorksheet.Drawings.AddPicture("Sample", img);
                pic.SetPosition(rowIndex, 0, colIndex, 0);
                pic.SetSize(Width, Height);
                //First Border Box  
                //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge
                int val = dtSrc.Columns.Count;
                using (ExcelRange Rng = objWorksheet.Cells[1, 3, 2, val])
                {
                    Rng.Value = PageName;//Text Color & Background Color
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Navy);
                }

                objWorksheet.Cells["A3"].LoadFromDataTable(dtSrc, true);


                for (int index = 2; index < dtSrc.Rows.Count; index++)
                {

                    //Columns
                    for (int colindex = 0; colindex < dtSrc.Columns.Count; colindex++)
                    {

                        // Checking for row index and wring header data
                        if (index == 2)
                        {


                            if (notReqiredCoulumnIndex.IndexOf(colindex) == 1)
                            {

                                objWorksheet.Cells[index + 1, colindex + 1].RichText.Text = dtSrc.Columns[colindex].Caption;
                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }

                        }
                        else
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == 1)
                            {

                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }
                        }

                    }
                }
                objWorksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 12));
                objWorksheet.Cells.AutoFitColumns();
                //Format the header    
                using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }



                try
                {
                    string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                    objExcelPackage.SaveAs(new System.IO.FileInfo(strUploadPath + "\\" + fileName + ".xlsx"));
                    // System.Diagnostics.Process.Start("Data.xlsx");

                }
                catch (Exception ex)
                {

                }

            }

            catch (Exception ex)
            {
                string Query = " INSERT INTO GEN_ErrorLog(Module, Message) ";
                Query += "SELECT 'CCDETAILS EXPORT','" + ex.InnerException + "'";
                DB.ExecuteSQL(Query);
                throw ex;
            }

        }

        public static void ExportCyclecountDetailsData(DataTable dtSrc, string fileName, List<int> notReqiredCoulumnIndex)
        {

            try
            {

                string strOperationNumber = string.Empty;
                ExcelPackage objExcelPackage = new ExcelPackage();



                string sheetName = fileName;

                //Create the worksheet    
                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(sheetName);
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                //Rows
                //Adding Headers

                for (int index = 0; index < dtSrc.Rows.Count; index++)
                {
                    //Columns
                    for (int colindex = 0; colindex < dtSrc.Columns.Count; colindex++)
                    {
                        // Checking for row index and wring header data
                        if (index == 0)
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                            {

                                objWorksheet.Cells[index + 1, colindex + 1].RichText.Text = dtSrc.Columns[colindex].Caption;
                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }

                        }
                        else
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                            {

                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }
                        }

                    }
                }
                objWorksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 12));
                objWorksheet.Cells.AutoFitColumns();
                //Format the header    
                using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }



                try
                {
                    string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                    objExcelPackage.SaveAs(new System.IO.FileInfo(strUploadPath + "\\" + fileName + ".xlsx"));
                    // System.Diagnostics.Process.Start("Data.xlsx");

                }
                catch (Exception ex)
                {


                }

            }

            catch (Exception ex)
            {


                string Query = " INSERT INTO GEN_ErrorLog(Module, Message) ";
                Query += "SELECT 'CCDETAILS EXPORT','" + ex.InnerException + "'";
                DB.ExecuteSQL(Query);
                throw ex;

            }

        }

        public static void ExportLoadSheetInfo(DataSet ds, string fileName, List<int> notReqiredCoulumnIndex)
        {

            try
            {

                string strOperationNumber = string.Empty;
                ExcelPackage objExcelPackage = new ExcelPackage();

                foreach (DataTable dtSrc in ds.Tables)
                {

                    string sheetName = fileName;

                    //Create the worksheet    
                    ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                    //Rows
                    //Adding Headers


                    for (int index = 0; index < dtSrc.Rows.Count; index++)
                    {
                        //Columns
                        for (int colindex = 0; colindex < dtSrc.Columns.Count; colindex++)
                        {
                            // Checking for row index and wring header data
                            if (index == 0)
                            {

                                if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                                {

                                    objWorksheet.Cells[index + 1, colindex + 1].RichText.Text = dtSrc.Columns[colindex].Caption;
                                    objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                                }

                            }
                            else
                            {

                                if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                                {

                                    objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                                }
                            }

                        }
                    }
                    objWorksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 12));
                    objWorksheet.Cells.AutoFitColumns();
                    //Format the header    
                    using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                    {
                        objRange.Style.Font.Bold = true;
                        objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }


                }





                try
                {
                    string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                    objExcelPackage.SaveAs(new System.IO.FileInfo(strUploadPath + "\\" + fileName + ".xlsx"));
                    // System.Diagnostics.Process.Start("Data.xlsx");

                }
                catch (Exception ex)
                {


                }

            }

            catch (Exception ex)
            {


                string Query = " INSERT INTO GEN_ErrorLog(Module, Message) ";
                Query += "SELECT 'CCDETAILS EXPORT','" + ex.InnerException + "'";
                DB.ExecuteSQL(Query);
                throw ex;

            }

        }

    }
}
