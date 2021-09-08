using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Printing;
//using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Collections;




namespace MRLWMSC21Common
{
    public class PrinterHelper
    {
        public PrinterHelper()
        {
        }

            private static string enq = Convert.ToChar(5).ToString();
            private static string esc = Convert.ToChar(27).ToString();
            private static string nul = Convert.ToChar(0).ToString();
            private static string rs = Convert.ToChar(30).ToString();
            private static string lf = Convert.ToChar(10).ToString();
            private static string cr = Convert.ToChar(13).ToString();

            // Structure and API declarions:
            [StructLayout(LayoutKind.Sequential , CharSet = CharSet.Ansi)]
            public class DOCINFOA
            {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
            }
            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);
            [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool ClosePrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);
            [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool EndDocPrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool StartPagePrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool EndPagePrinter(IntPtr hPrinter);
            [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);
            // SendBytesToPrinter()
            // When the function is given a printer name and an unmanaged array
            // of bytes, the function sends those bytes to the print queue.
            // Returns true on success, false on failure.
            public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
            {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.
            di.pDocName = "IVRCL Document Developed by M sateesh kumar";
            di.pDataType = "RAW";
            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
            // Start a document.
            if (StartDocPrinter(hPrinter, 1, di))
            {
            // Start a page.
            if (StartPagePrinter(hPrinter))
            {
            // Write your bytes.
            bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
            EndPagePrinter(hPrinter);
            }
            EndDocPrinter(hPrinter);
            }
            ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
            dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
            }
            public static bool SendFileToPrinter(string szPrinterName, string szFileName)
            {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;
            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);


            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // MessageBox.Show("L'impression est dclenche maintenant!");

            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
            }

            public static bool SendStringToPrinter(string szPrinterName, string szString)
            {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);


            // Send the converted ANSI string to the printer.
            bool res = SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return res;
            }

            private String OpenCashDrawer()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)112);
            sequence.Append((char)0);
            sequence.Append((char)25);
            sequence.Append((char)250);


            return sequence.ToString();
            }


            private String InitializePrinter()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)64);


            return sequence.ToString();
            }
            private static String PrintCenterText()
            {
            //The printer will continue printing at center
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)97);
            sequence.Append((char)1);


            return sequence.ToString();
            }
            private static String PrintLeftText()
            {
            //The printer will continue printing at left
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)97);
            sequence.Append((char)0);


            return sequence.ToString();
            }
            private static String CutPaper()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)105);
            sequence.Append((char)0);
            sequence.Append((char)25);


            return sequence.ToString();
            }


            private static String LineFeed(int pNumLines)
            {
            StringBuilder sequence = new StringBuilder();
            //sequence.Append((char)27);
            //sequence.Append((char)100);
            // sequence.Append((char)pNumLines);
            //

            //return sequence.ToString();
            for (int i = 0; i < pNumLines; i++)
            sequence.Append(lf + cr);
            return sequence.ToString();
            }
            private static String Boldon()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append(esc + "E" + nul);
            return sequence.ToString();
            }
            private static String Boldoff()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append(nul + esc + "F" + nul);
            return sequence.ToString();
            }
            private static String Italicon()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append(esc + "4" + nul);
            return sequence.ToString();
            }
            private static String Italicooff()
            {
            StringBuilder sequence = new StringBuilder();
            sequence.Append(nul + esc + "5" + nul);
            return sequence.ToString();
            }
            private static string newline()
            {
            return lf + cr;
            }
            private static String Compress()
            {
            return esc + "x0" + Convert.ToChar(15).ToString()+nul;
            }
            private static string Eject()
            {
            return esc + "EMR";
            }
            private static string nextpage()
            {
            return Convert.ToChar(12).ToString();
            }
            private static string padc(ArrayList strs, int length)
            {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strs)
            {
            sb.Append(str.PadLeft((length / 2)+str.Length/2,' ').PadRight(length,' '));
            sb.Append(newline());
            }
            return sb.ToString();
            }
            private static string Reset()
            {
            return esc + Convert.ToChar(64).ToString() + nul;
            }
            private static string Drawline(int length)
            {
            string str = "";
            for (int i = 0; i < length; i++)
            str += '-';
            return str + newline();
            }


            public static string SendtoPrint(DataSet tableDS, DataSet schemaDS, ArrayList Headers, ArrayList Footer, string PrinterName, int fromPage, int toPage)
            {
            if (string.IsNullOrEmpty(PrinterName))
            {
            return "Specify Printername";
            }
            if ((tableDS == null) || (tableDS.Tables[0].Rows.Count == 0))
            {
            return "No data to Print";
            }
            if (tableDS.Tables[0].Columns.Count != schemaDS.Tables[0].Rows.Count)
            {
            return "Please check the schema table values";
            }
            // List<SqlParameter> parms = new List<SqlParameter>();
            // parms.Add(new SqlParameter("@TableName", tableDS.Tables[0].TableName));
            // string str = "";
            // foreach (DataColumn dc in tableDS.Tables[0].Columns)
            // {
            // str += dc.ColumnName + ',';
            // }
            // str = str.Remove(str.Length - 1, 1);
            // parms.Add(new SqlParameter("@Columns", str));

            // DataSet schemaDS = new DataSet();
            // DataAccessLayer.DBConnection objDBConnection = new OAIIS.DataAccessLayer.DBConnection();
            // schemaDS = objDBConnection.DBExecute("SP_GBL_TableSchema", CommandType.StoredProcedure, parms.ToArray(), null);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(schemaDS);
            //-------------------------
            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbFooter = new StringBuilder();
            StringBuilder sbCols = new System.Text.StringBuilder();
            StringBuilder sbRows = new System.Text.StringBuilder();
            // Object[] columns;
            sbHeader.Append(PrinterHelper.padc(Headers, 136));
            sbFooter.Append(PrinterHelper.Drawline(136) + PrinterHelper.padc(Footer, 136));
            sbCols.Append(PrinterHelper.Drawline(136));
            //foreach (DataColumn dc in tableDS.Tables[0].Columns)
            //{
            //    int.TryParse(schemaDS.Tables[0].Rows[dc.Ordinal][2].ToString(), out int padlengh);
            //    sbCols.Append(dc.ColumnName.PadRight(padlengh, ' '));
            //sbCols.Append(' ');
            //}
            if (sbCols.Length > 0)
            sbCols.Remove(sbCols.Length - 1, 1).Append(PrinterHelper.newline()); // Write the Data Rows
            sbCols.Append(PrinterHelper.Drawline(136));
            sbRows.Append(PrinterHelper.Boldon() + sbHeader.ToString() + sbCols.ToString() + PrinterHelper.Boldoff());
            int printedlinescount = Headers.Count + (sbCols.ToString().Length / 136) + 1;
            int pageno = 1;
            bool newpageflag = false;
            int rowlinecount = 0;
            //foreach (DataRow dr in schemaDS.Tables[0].Rows)
            //{
            //    int.TryParse(dr[2].ToString(), out int temp);
            //    rowlinecount += temp;

            //}
            rowlinecount = rowlinecount / 136 + 1;
            foreach (DataRow dr in tableDS.Tables[0].Rows)
            {
            if ((fromPage != 0) && (pageno > toPage))
            break;
            if (newpageflag)
            {
            newpageflag = false;
            pageno++;
            if ((pageno == 0) || (pageno <= toPage))
            {
            sbRows.Append(PrinterHelper.Boldon() + sbFooter.ToString() + PrinterHelper.Boldoff());
            sbRows.Append(PrinterHelper.nextpage());
            sbRows.Append(PrinterHelper.Reset());
            sbRows.Append(PrinterHelper.Boldon() + sbHeader.ToString() + sbCols.ToString() + PrinterHelper.Boldoff());
            printedlinescount = Headers.Count + (sbCols.ToString().Length / 130) + 1;
            }
            }
            if ((fromPage == 0) || ((pageno >= fromPage) && (pageno <= toPage)))
            {
            //for (int i = 0; i < dr.ItemArray.Length; i++)
            //{
            //            int.TryParse(schemaDS.Tables[0].Rows[i][2].ToString(), out int padlengh);
            //            string strdatatype = Convert.ToString(schemaDS.Tables[0].Rows[i][1]);
            //switch (strdatatype)
            //{
            //case "datetime":
            ////sbRows.Append(((DateTime)dr[i]).ToString("dd/MM/yyyy hh:mm:ss tt").PadRight(padlengh, ' '));
            ////break;
            //case "date":
            //sbRows.Append(((DateTime)dr[i]).ToString("dd/MM/yyyy").PadRight(padlengh, ' '));
            //break;
            //case "time":
            //sbRows.Append(((DateTime)dr[i]).ToString("hh:mm:ss tt").PadRight(padlengh, ' '));
            //break;
            //case "int":
            //case "tinyint":
            //case "smallint":
            //case "numaric":
            //case "decimal":
            //sbRows.Append(dr[i].ToString().PadLeft(padlengh, ' '));
            //break;
            //default:
            //sbRows.Append(dr[i].ToString().PadRight(padlengh, ' '));
            //break;
            //}

            //sbRows.Append(' ');
            //}
            if (sbRows.Length > 0)
            sbRows.Remove(sbRows.Length - 1, 1).Append(PrinterHelper.newline());
            //printedlinescount += 1;
            printedlinescount += rowlinecount;//= (sbRows.Length / 136) + 2;

            if (printedlinescount > 64)
            {
            newpageflag = true;
            //pageno++;
            //if ((pageno ==0) ||(pageno <=toPage))
            //{
            //sbRows.Append(PrinterHelper.Boldon() + sbFooter.ToString() + PrinterHelper.Boldoff());
            //sbRows.Append(PrinterHelper.nextpage());
            //sbRows.Append(PrinterHelper.Reset());
            //sbRows.Append(PrinterHelper.Boldon() + sbHeader.ToString() + sbCols.ToString() + PrinterHelper.Boldoff());
            //printedlinescount = Headers.Count + (sbCols.ToString().Length / 136) + 1;
            //}
            }
            }
            }
            sbRows.Append(PrinterHelper.Boldon() + sbFooter.ToString() + PrinterHelper.Boldoff());
            sbRows.Append(PrinterHelper.nextpage());
            bool result = PrinterHelper.SendStringToPrinter(PrinterName, PrinterHelper.Reset() + PrinterHelper.Compress() + sbRows.ToString());
            if (result)
            return "Done Please collect the papers";
            else
            return "Error in Printing";
            //PrinterHelper.SendStringToPrinter(@"\\moiz\EPSON LQ-300+II ESC/P2", ((char)27)+"x0"+((char)15) +sbCols.ToString() + "\r\n" + sbRows.ToString());

            }

        }

           
}
