using System;
using MRLWMSC21Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using System.Net.Mail;


namespace MRLWMSC21Core.Library
{
    public static class ExceptionHandling
    {

        public static void LogException(Exception ex, string MethodCode)
        {
            LogExceptionToDB(ex, MethodCode, null);
            FireExceptionEMail(ex, MethodCode, null);
        }

        public static void LogException(Exception ex, string MethodCode, ExceptionData ExceptionData)
        {
            LogExceptionToDB(ex, MethodCode, ExceptionData);
            FireExceptionEMail(ex, MethodCode, ExceptionData);
        }

        private static void LogExceptionToDB(Exception ex, string MethodCode, ExceptionData ExceptionData)
        {

            try
            {
                if (ex != null && MethodCode != null && ExceptionData != null)
                {
                    string _sSQL = "EXEC [dbo].[sp_API_LogApplicationExceptions]";

                    List<SqlParameter> lParams = new List<SqlParameter>();

                    SqlParameter oMethodCode = new SqlParameter("@MethodCode", SqlDbType.NVarChar, 50);
                    oMethodCode.Value = MethodCode;

                    SqlParameter oWMSExceptionCode = new SqlParameter("@WMSExceptionCode", SqlDbType.NVarChar, 50);
                    oWMSExceptionCode.Value = string.Empty;

                    SqlParameter oWMSExceptionMessage = new SqlParameter("@WMSExceptionMessage", SqlDbType.NVarChar, 8000);
                    oWMSExceptionMessage.Value = string.Empty;

                    SqlParameter oExcpInnerException = new SqlParameter("@ExcpInnerException", SqlDbType.NVarChar, 8000);
                    oExcpInnerException.Value = (ex.InnerException == null ? string.Empty : ex.InnerException.ToString());

                    SqlParameter oExcpMessage = new SqlParameter("@ExcpMessage", SqlDbType.NVarChar, 8000);
                    oExcpMessage.Value = ex.Message == null ? string.Empty : ex.Message.Replace("'", "\"").ToString();

                    SqlParameter oExcpStackTrace = new SqlParameter("@ExcpStackTrace", SqlDbType.NVarChar, 50);
                    oExcpStackTrace.Value = ex.StackTrace == null ? string.Empty : ex.StackTrace.Replace("'", "\"").ToString();


                    SqlParameter oExceptionData = new SqlParameter("@ExceptionData", SqlDbType.NVarChar, 8000);
                    oExceptionData.Value = PrepareExceptionDataString(MethodCode, ExceptionData);


                    lParams.Add(oMethodCode);
                    lParams.Add(oWMSExceptionCode);
                    lParams.Add(oWMSExceptionMessage);
                    lParams.Add(oExcpInnerException);
                    lParams.Add(oExcpStackTrace);
                    lParams.Add(oExcpMessage);
                    lParams.Add(oExceptionData);

                    string _sConnectionString = string.Empty;

                    SqlConnection _oSQLConnection;

                    if (FrameworkUtilities.ReadApplicationKey("dbConnectionString") != null)
                        _sConnectionString = FrameworkUtilities.ReadApplicationKey("dbConnectionString");
                    if (_sConnectionString.Length > 25)
                    {
                        _oSQLConnection = new SqlConnection(_sConnectionString);
                        _oSQLConnection.Open();

                        DataSet _dsResults = new DataSet();
                        StringBuilder _sbQuery = new StringBuilder();

                        foreach (SqlParameter Param in lParams)
                        {
                            bool _IsQuoteRequired = false;

                            if (Param.SqlDbType.Equals(SqlDbType.NVarChar) || Param.SqlDbType.Equals(SqlDbType.Char) || Param.SqlDbType.Equals(SqlDbType.Date) || Param.SqlDbType.Equals(SqlDbType.DateTime) || Param.SqlDbType.Equals(SqlDbType.DateTime2) || Param.SqlDbType.Equals(SqlDbType.SmallDateTime) || Param.SqlDbType.Equals(SqlDbType.Text) || Param.SqlDbType.Equals(SqlDbType.UniqueIdentifier) || Param.SqlDbType.Equals(SqlDbType.VarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Variant))
                                _IsQuoteRequired = true;

                            _sbQuery.Append((_sbQuery.Length.Equals(0) ? " " : ",") + " " + Param.ParameterName + " = " + (_IsQuoteRequired ? "'" : "") + Param.Value + (_IsQuoteRequired ? "'" : ""));

                        }
                        // oSQLCommand.Prepare();
                        SqlCommand oSQLCommand = new SqlCommand(_sSQL + _sbQuery.ToString(), _oSQLConnection);

                        int _iData = oSQLCommand.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();

                FireExceptionEMail(excp, "EXCEPTION HANDLING-DB LOGGING", oExcpData);
            }

        }


        private static string PrepareExceptionDataString(string MethodCode, ExceptionData excpData)
        {
            if (excpData.MethodInputs != null && MethodCode != null)
            {
                StringBuilder _sbExcptionData = new StringBuilder();

                _sbExcptionData.Append("<ExceptionData>");

                foreach (KeyValuePair<string, object> Pair in excpData.MethodInputs)
                {
                    _sbExcptionData.Append("<Data>");

                    string json = JsonConvert.SerializeObject(Pair.Value);

                    _sbExcptionData.Append("<MethodCode>" + MethodCode + "</MethodCode>");
                    _sbExcptionData.Append("<Parameter>" + Pair.Key.ToString() + "</Parameter>");
                    _sbExcptionData.Append("<InputData>" + json.Replace("'", "\"") + "</InputData>");

                    _sbExcptionData.Append("</Data>");
                }

                _sbExcptionData.Append("</ExceptionData>");

                return _sbExcptionData.ToString();
            }
            else return string.Empty;
        }


        private static void FireExceptionEMail(Exception ex, string MethodCode, ExceptionData excpData)
        {
            MailUtility mailUtility = new MailUtility();
            mailUtility.SendExceptionEmail(ex, MethodCode, excpData);

            ////MailMessage mail = new MailMessage("adityag@inventrax.com", "adityag@inventrax.com");


            //// Command line argument must the the SMTP host.
            //SmtpClient client = new SmtpClient();
            //client.Port = 25;
            //client.Host = "smtpout.asia.secureserver.net";
            //client.EnableSsl = false;
            //client.Timeout = 50000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential("adityag@inventrax.com", "AdityaSrinivasB");

            //MailMessage mm = new MailMessage("adityag@inventrax.com", "adityag@inventrax.com", "An Exception has Occoured on : ", "EXCPTION");
            //mm.To.Add(new MailAddress("prasanna.chaganti@inventrax.com"));
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            //client.Send(mm);

            ////string your_id = "adityag@inventrax.com";
            ////string your_password = "AdityaSrinivasB";
            ////try
            ////{
            ////    SmtpClient client = new SmtpClient
            ////    {
            ////        Host = "smtpout.asia.secureserver.net",
            ////        Port = 80,
            ////        EnableSsl = false,
            ////        DeliveryMethod = SmtpDeliveryMethod.Network,
            ////        Credentials = new System.Net.NetworkCredential(your_id, your_password),
            ////        Timeout = 10000,
            ////    };
            ////    MailMessage mm = new MailMessage(your_id, "adityag@inventrax.com, prasanna.chaganti@inventrax.com", "subject", "body");
            ////    client.Send(mm);
            ////    Console.WriteLine("Email Sent");
            ////}
            ////catch (Exception e)
            ////{
            ////    Console.WriteLine("Could not end email\n\n" + e.ToString());
            ////}

        }

        public enum ExcpConstants_API_DataLayer
        {
            BaseDAL = 1,        // WMSCore_DAL_0001_
            UserDAL,            // WMSCore_DAL_0002_
            MasterDAL,          // WMSCore_DAL_0003_
            PalletDAL,          // WMSCore_DAL_0004_
            LocationDAL,        // WMSCore_DAL_0005_
            CycleCountDAL,      // WMSCore_DAL_0006_
            DeNestingDAL,       // WMSCore_DAL_0007_
            HouseKeepingDAL,    // WMSCore_DAL_0008_
            InboundDAL,         // WMSCore_DAL_0009_
            InventoryDAL,       // WMSCore_DAL_0010_
            OutboundDAL,         // WMSCore_DAL_0011_
            ScanningDAL  ,       // WMSCore_DAL_0012_
            IntegrationDAL
        }

        public enum ExcpConstants_API_BusinessLayer
        {
            BaseBL = 1,         // WMSCore_BL_0001_
            GenericBL,          // WMSCore_BL_0002_
            MasterBL,           // WMSCore_BL_0003_
            LoginBL,            // WMSCore_BL_0004_
            LocationBL,         // WMSCore_BL_0005_
            InventoryBL,        // WMSCore_BL_0006_
            InboundBL,          // WMSCore_BL_0007_
            DeNestingBL,        // WMSCore_BL_0008_
            CycleCountBL,       // WMSCore_BL_0009_
            HouseKeepingBL,     // WMSCore_BL_0010_
            OutboundBL  ,        // WMSCore_BL_0011_
            ScanningBL,        // WMSCore_BL_0012_
            IntegrationBL

        }

        public enum ExcpConstants_API_Enpoint
        {
            BaseController = 1,         // WMSCore_CTRLR_0001_
            InboundController,          // WMSCore_CTRLR_0002_
            OutboundController,         // WMSCore_CTRLR_0003_
            CycleCountController,       // WMSCore_CTRLR_0004_
            DeNestingController,        // WMSCore_CTRLR_0005_
            ExceptionController,        // WMSCore_CTRLR_0006_
            MRLWMSC21CoreController,    // WMSCore_CTRLR_0007_
            HomeController,             // WMSCore_CTRLR_0008_
            HouseKeepingController,     // WMSCore_CTRLR_0009_
            InternalTransferController, // WMSCore_CTRLR_0010_
            LoginController,            // WMSCore_CTRLR_0011_
            ValuesController,            // WMSCore_CTRLR_0012_
            PODController
        }

        public static string GetClassExceptionCode(ExcpConstants_API_BusinessLayer ClassName)
        {
            string _sBaseCode = "WMSCore_BL_";
            string _sCode = ClassName.GetHashCode().ToString();

            _sCode = (_sCode.Length.Equals(1) ? "000" : (_sCode.Length.Equals(2) ? "00" : (_sCode.Length.Equals(3) ? "0" : ""))) + _sCode + "_";

            return _sBaseCode + _sCode;
        }
        
        public static string GetClassExceptionCode(ExcpConstants_API_DataLayer ClassName)
        {
            string _sBaseCode = "WMSCore_DAL_";
            string _sCode = ClassName.GetHashCode().ToString();

            _sCode = (_sCode.Length.Equals(1) ? "000" : (_sCode.Length.Equals(2) ? "00" : (_sCode.Length.Equals(3) ? "0" : ""))) + _sCode + "_";

            return _sBaseCode + _sCode;
        }
        
        public static string GetClassExceptionCode(ExcpConstants_API_Enpoint ClassName)
        {
            string _sBaseCode = "WMSCore_CTRLR_";
            string _sCode = ClassName.GetHashCode().ToString();

            _sCode = (_sCode.Length.Equals(1) ? "000" : (_sCode.Length.Equals(2) ? "00" : (_sCode.Length.Equals(3) ? "0" : ""))) + _sCode + "_";

            return _sBaseCode + _sCode;
        }

    }

    public class ExceptionData
    {
        private Dictionary<string, object> _MethodInputs;

        public Dictionary<string, object> MethodInputs { get => _MethodInputs; set => _MethodInputs = value; }

        public ExceptionData()
        {
            _MethodInputs = new Dictionary<string, object>();
        }

        ~ExceptionData()
        {
            _MethodInputs = null;// new Dictionary<string, object>();
        }

        public void AddInputs(string ParamName, object ParamValue)
        {
            _MethodInputs.Add(ParamName, ParamValue);
        }
    }

    public class WMSExceptionMessage : Exception
    {
        private string _WMSMessage;

        private string _WMSExceptionCode;

        private bool _ShowAsError;

        private bool _ShowAsWarning;

        private bool _ShowAsSuccess;

        private bool _ShowAsCriticalError;

        private bool _ShowUserConfirmDialogue;

        public string WMSMessage { get => _WMSMessage; set => _WMSMessage = value; }
        public string WMSExceptionCode { get => _WMSExceptionCode; set => _WMSExceptionCode = value; }
        public bool ShowAsError { get => _ShowAsError; set => _ShowAsError = value; }
        public bool ShowAsWarning { get => _ShowAsWarning; set => _ShowAsWarning = value; }
        public bool ShowAsSuccess { get => _ShowAsSuccess; set => _ShowAsSuccess = value; }
        public bool ShowAsCriticalError { get => _ShowAsCriticalError; set => _ShowAsCriticalError = value; }
        public bool ShowUserConfirmDialogue { get => _ShowUserConfirmDialogue; set => _ShowUserConfirmDialogue = value; }


    }
    
}
