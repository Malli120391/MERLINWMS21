using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.DataAccess
{
    public  class BaseDAL
    {
        private string _sConnectionString;
        private int _LoggedInUserID = 1;

        private SqlConnection _oSQLConnection;

        private string _ClassCode = "WMSCore_DA_001_";

        public int LoggedInUserID { get => _LoggedInUserID; set => _LoggedInUserID = value; }

        private BaseDAL()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.BaseDAL);

            _LoggedInUserID = 1;
            _sConnectionString = FrameworkUtilities.ReadApplicationKey("dbConnectionString");
            if (_sConnectionString == null)
                
                _sConnectionString = "data source=192.168.1.20;initial catalog=VIP_Samadhan;user id=SL_AppUser;password=SL@369!;persist security info=True;packet size=4096;";

            _oSQLConnection = new SqlConnection(_sConnectionString);
            _oSQLConnection.Open();
        }


        public BaseDAL(int LoggedInUserID, string ConnectionString)
        {
            _LoggedInUserID = LoggedInUserID;
            _sConnectionString = ConnectionString;
            if (_sConnectionString == null || _sConnectionString.Equals(string.Empty))
                _sConnectionString = "data source=192.168.1.20;initial catalog=VIP_Samadhan;user id=SL_AppUser;password=SL@369!;persist security info=True;packet size=4096;";

            _oSQLConnection = new SqlConnection(_sConnectionString);
            
        }


        ~BaseDAL()
        {
            // Desctructor / Finalizer to de-allocate the memory for the Data Members.

            //_oSQLConnection.Close();

            _sConnectionString = null;
            _oSQLConnection = null;
        }

        /// <summary>
        /// Executes the Prepared Statement and returns the Dataset.
        /// </summary>
        /// <param name="sPreparedStatement"> Prepared statement string. </param>
        /// <param name="lSQLParameter"> Parameters of the Prepared Statement. </param>
        /// <returns>Data Set</returns>
        public DataSet FillDataSet(string sPreparedStatement, List<SqlParameter> lSQLParameter)
        {
            string _SQLStatement = string.Empty;
            StringBuilder _sbQuery = new StringBuilder();

            try
            {

                _oSQLConnection.Open();

                SqlCommand oSQLCommand = new SqlCommand(sPreparedStatement, _oSQLConnection);
                
                DataSet _dsResults = new DataSet();

                if (lSQLParameter == null)
                    lSQLParameter = new List<SqlParameter>();

                bool IsExecuteStatement = sPreparedStatement.Contains("EXEC");

                foreach (SqlParameter Param in lSQLParameter)
                {
                    if (IsExecuteStatement)
                    {
                        bool _IsQuoteRequired = false;

                        if (Param.SqlDbType.Equals(SqlDbType.NVarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Char) || Param.SqlDbType.Equals(SqlDbType.Date) || Param.SqlDbType.Equals(SqlDbType.DateTime) || Param.SqlDbType.Equals(SqlDbType.DateTime2) || Param.SqlDbType.Equals(SqlDbType.SmallDateTime) || Param.SqlDbType.Equals(SqlDbType.Text) || Param.SqlDbType.Equals(SqlDbType.UniqueIdentifier) || Param.SqlDbType.Equals(SqlDbType.VarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Variant))
                            _IsQuoteRequired = true;

                        _sbQuery.Append((_sbQuery.Length.Equals(0) ? "" : ",") + " " + Param.ParameterName + " = " + (_IsQuoteRequired ? "'" : "") + Param.Value + (_IsQuoteRequired ? "'" : ""));

                    }
                    else
                    {
                        SqlParameter _TempSQLParameter = new SqlParameter(Param.ParameterName, Param.SqlDbType, Param.Size);
                        _TempSQLParameter.Value = Param.Value;

                        oSQLCommand.Parameters.Add(_TempSQLParameter);
                    }
                }

                if (IsExecuteStatement)
                    _SQLStatement = sPreparedStatement + " " + _sbQuery.ToString();
                else
                    _SQLStatement = sPreparedStatement;

                oSQLCommand.CommandText = _SQLStatement;

                oSQLCommand.Prepare();

                SqlDataAdapter _oDAPreparedStatement = new SqlDataAdapter(oSQLCommand);
                
                _oDAPreparedStatement.Fill(_dsResults);

                _oSQLConnection.Close();
                return _dsResults;
            }
            catch(Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("sPreparedStatement", sPreparedStatement);
                // oExcpData.AddInputs("lSQLParameter", lSQLParameter);

                oExcpData.AddInputs("SQL STATEMENT", sPreparedStatement + " " + _sbQuery.ToString());

                foreach(SqlParameter _Param in lSQLParameter)
                {
                    oExcpData.AddInputs(_Param.ParameterName, _Param.Value);
                }

                ExceptionHandling.LogException(ex, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
            finally
            {
                _oSQLConnection.Close();
            }
        }


        /// <summary>
        /// Executes the Prepared Statement and returns the Dataset.
        /// </summary>
        /// <param name="sPreparedStatement"> Prepared statement string. </param>
        /// <returns>Data Set</returns>
        public DataSet FillDataSet(string sPreparedStatement)
        {
            try
            {
                _oSQLConnection.Open();

                SqlCommand oSQLCommand = new SqlCommand(sPreparedStatement, _oSQLConnection);

                DataSet _dsResults = new DataSet();

                oSQLCommand.Prepare();

                SqlDataAdapter _oDAPreparedStatement = new SqlDataAdapter(oSQLCommand);

                _oDAPreparedStatement.Fill(_dsResults);

                _oSQLConnection.Close();
                return _dsResults;
            }
            catch (Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("sPreparedStatement", sPreparedStatement);

                ExceptionHandling.LogException(ex, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
            finally
            {
                _oSQLConnection.Close();
            }
        }


        /// <summary>
        /// Executes the Prepared Statement and returns the Non Query Integer Output.
        /// </summary>
        /// <param name="sPreparedStatement"> Prepared statement string. </param>
        /// <param name="lSQLParameter"> Parameters of the Prepared Statement. </param>
        /// <returns>Data Set</returns>
        public int ExecuteNonQuery(string sPreparedStatement, List<SqlParameter> lSQLParameter)
        {
            string _SQLStatement = string.Empty;

            try
            {

                _oSQLConnection.Open();

                SqlCommand oSQLCommand = new SqlCommand(sPreparedStatement, _oSQLConnection);
                StringBuilder _sbQuery = new StringBuilder();

                DataSet _dsResults = new DataSet();

                bool IsExecuteStatement = sPreparedStatement.Contains("EXEC");

                foreach (SqlParameter Param in lSQLParameter)
                {
                    if (IsExecuteStatement)
                    {
                        bool _IsQuoteRequired = false;

                        if (Param.SqlDbType.Equals(SqlDbType.NVarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Char) || Param.SqlDbType.Equals(SqlDbType.Date) || Param.SqlDbType.Equals(SqlDbType.DateTime) || Param.SqlDbType.Equals(SqlDbType.DateTime2) || Param.SqlDbType.Equals(SqlDbType.SmallDateTime) || Param.SqlDbType.Equals(SqlDbType.Text) || Param.SqlDbType.Equals(SqlDbType.UniqueIdentifier) || Param.SqlDbType.Equals(SqlDbType.VarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Variant))
                            _IsQuoteRequired = true;

                        _sbQuery.Append((_sbQuery.Length.Equals(0) ? "" : ",") + " " + Param.ParameterName + " = " + (_IsQuoteRequired ? "'" : "") + Param.Value + (_IsQuoteRequired ? "'" : ""));

                    }
                    else
                    {
                        SqlParameter _TempSQLParameter = new SqlParameter(Param.ParameterName, Param.SqlDbType, Param.Size);
                        _TempSQLParameter.Value = Param.Value;

                        oSQLCommand.Parameters.Add(_TempSQLParameter);
                    }
                }
                if (IsExecuteStatement)
                   _SQLStatement = sPreparedStatement + " " + _sbQuery.ToString();
                else
                    _SQLStatement = sPreparedStatement;

                oSQLCommand.CommandText = _SQLStatement;

                //foreach (SqlParameter Param in lSQLParameter)
                //{
                //    SqlParameter _oParameter = new SqlParameter(Param.ParameterName, Param.SqlDbType, Param.Size);
                //    _oParameter.Value = Param.Value;

                //    oSQLCommand.Parameters.Add(_oParameter);
                //}
                oSQLCommand.Prepare();

                int _iData = oSQLCommand.ExecuteNonQuery();

                _oSQLConnection.Close();
                return _iData;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("sPreparedStatement", sPreparedStatement);
                oExcpData.AddInputs("EXEC STATEMENT", _SQLStatement);


                foreach (SqlParameter _Param in lSQLParameter)
                {
                    oExcpData.AddInputs(_Param.ParameterName, _Param.Value);
                }

                ExceptionHandling.LogException(ex, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };

            }
            finally
            {
                _oSQLConnection.Close();
            }
        }
          

        public int ExecuteNonQuery(string sPreparedStatement)
        {
            string _SQLStatement = string.Empty;
            try
            {

                _oSQLConnection.Open();

                List<SqlParameter> lSQLParameter = new List<SqlParameter>();
                SqlCommand oSQLCommand = new SqlCommand(sPreparedStatement, _oSQLConnection);
                StringBuilder _sbQuery = new StringBuilder();

                DataSet _dsResults = new DataSet();

                bool IsExecuteStatement = sPreparedStatement.Contains("EXEC");

                foreach (SqlParameter Param in lSQLParameter)
                {
                    if (IsExecuteStatement)
                    {
                        bool _IsQuoteRequired = false;

                        if (Param.SqlDbType.Equals(SqlDbType.NVarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Char) || Param.SqlDbType.Equals(SqlDbType.Date) || Param.SqlDbType.Equals(SqlDbType.DateTime) || Param.SqlDbType.Equals(SqlDbType.DateTime2) || Param.SqlDbType.Equals(SqlDbType.SmallDateTime) || Param.SqlDbType.Equals(SqlDbType.Text) || Param.SqlDbType.Equals(SqlDbType.UniqueIdentifier) || Param.SqlDbType.Equals(SqlDbType.VarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Variant))
                            _IsQuoteRequired = true;

                        _sbQuery.Append((_sbQuery.Length.Equals(0) ? "" : ",") + " " + Param.ParameterName + " = " + (_IsQuoteRequired ? "'" : "") + Param.Value + (_IsQuoteRequired ? "'" : ""));

                    }
                    else
                    {
                        SqlParameter _TempSQLParameter = new SqlParameter(Param.ParameterName, Param.SqlDbType, Param.Size);
                        _TempSQLParameter.Value = Param.Value;

                        oSQLCommand.Parameters.Add(_TempSQLParameter);
                    }
                }

                if (IsExecuteStatement)
                    _SQLStatement = sPreparedStatement + " " + _sbQuery.ToString();
                else
                    _SQLStatement = sPreparedStatement;

                oSQLCommand.CommandText = _SQLStatement;

                oSQLCommand.Prepare();

                int _iData = oSQLCommand.ExecuteNonQuery();

                _oSQLConnection.Close();
                return _iData;
            }
            catch (Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("sPreparedStatement", sPreparedStatement);
                oExcpData.AddInputs("EXEC STATEMENT", _SQLStatement);
             
                ExceptionHandling.LogException(ex, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };

            }
            finally
            {
                _oSQLConnection.Close();
            }
        }


        /// <summary>
        /// Executes the Prepared Statement and returns the Scalar Output as a String.
        /// </summary>
        /// <param name="PreparedStatement"> Prepared statement string. </param>
        /// <param name="SQLParameters"> Parameters of the Prepared Statement. </param>
        /// <returns>Data Set</returns>
        public string ExecuteScalar(string PreparedStatement, List<SqlParameter> SQLParameters)
        {
            string _SQLStatement = string.Empty;
            try
            {

                _oSQLConnection.Open();

                SqlCommand oSQLCommand = new SqlCommand(PreparedStatement, _oSQLConnection);
                StringBuilder _sbQuery = new StringBuilder();

                DataSet _dsResults = new DataSet();


                bool IsExecuteStatement = PreparedStatement.Contains("EXEC");

                foreach (SqlParameter Param in SQLParameters)
                {
                    if (IsExecuteStatement)
                    {
                        bool _IsQuoteRequired = false;

                        if (Param.SqlDbType.Equals(SqlDbType.NVarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Char) || Param.SqlDbType.Equals(SqlDbType.Date) || Param.SqlDbType.Equals(SqlDbType.DateTime) || Param.SqlDbType.Equals(SqlDbType.DateTime2) || Param.SqlDbType.Equals(SqlDbType.SmallDateTime) || Param.SqlDbType.Equals(SqlDbType.Text) || Param.SqlDbType.Equals(SqlDbType.UniqueIdentifier) || Param.SqlDbType.Equals(SqlDbType.VarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Variant))
                            _IsQuoteRequired = true;

                        _sbQuery.Append((_sbQuery.Length.Equals(0) ? "" : ",") + " " + Param.ParameterName + " = " + (_IsQuoteRequired ? "'" : "") + Param.Value + (_IsQuoteRequired ? "'" : ""));

                    }
                    else
                    {
                        SqlParameter _TempSQLParameter = new SqlParameter(Param.ParameterName, Param.SqlDbType, Param.Size);
                        _TempSQLParameter.Value = Param.Value;

                        oSQLCommand.Parameters.Add(_TempSQLParameter);
                    }
                }


                if (IsExecuteStatement)
                    _SQLStatement = PreparedStatement + " " + _sbQuery.ToString();
                else
                    _SQLStatement = PreparedStatement;

                oSQLCommand.CommandText = _SQLStatement;

                oSQLCommand.Prepare();

                string _sData= (string)oSQLCommand.ExecuteScalar();

                _oSQLConnection.Close();
                return _sData;
            }
            catch (Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("sPreparedStatement", PreparedStatement);
                oExcpData.AddInputs("EXEC STATEMENT", _SQLStatement);


                foreach (SqlParameter _Param in SQLParameters)
                {
                    oExcpData.AddInputs(_Param.ParameterName, _Param.Value);
                }

                ExceptionHandling.LogException(ex, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };

            }
            finally
            {
                _oSQLConnection.Close();
            }
        }

        public string ExecuteScalar(string PreparedStatement)
        {
            string _SQLStatement = string.Empty;
            try
            {

                _oSQLConnection.Open();

                List<SqlParameter> SQLParameters = new List<SqlParameter>();
                StringBuilder _sbQuery = new StringBuilder();
                SqlCommand oSQLCommand = new SqlCommand(PreparedStatement, _oSQLConnection);

                DataSet _dsResults = new DataSet();

                bool IsExecuteStatement = PreparedStatement.Contains("EXEC");

                foreach (SqlParameter Param in SQLParameters)
                {
                    if (IsExecuteStatement)
                    {
                        bool _IsQuoteRequired = false;

                        if (Param.SqlDbType.Equals(SqlDbType.NVarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Char) || Param.SqlDbType.Equals(SqlDbType.Date) || Param.SqlDbType.Equals(SqlDbType.DateTime) || Param.SqlDbType.Equals(SqlDbType.DateTime2) || Param.SqlDbType.Equals(SqlDbType.SmallDateTime) || Param.SqlDbType.Equals(SqlDbType.Text) || Param.SqlDbType.Equals(SqlDbType.UniqueIdentifier) || Param.SqlDbType.Equals(SqlDbType.VarChar) || Param.SqlDbType.Equals(SqlDbType.Xml) || Param.SqlDbType.Equals(SqlDbType.Variant))
                            _IsQuoteRequired = true;

                        _sbQuery.Append((_sbQuery.Length.Equals(0) ? "" : ",") + " " + Param.ParameterName + " = " + (_IsQuoteRequired ? "'" : "") + Param.Value + (_IsQuoteRequired ? "'" : ""));

                    }
                    else
                    {
                        SqlParameter _TempSQLParameter = new SqlParameter(Param.ParameterName, Param.SqlDbType, Param.Size);
                        _TempSQLParameter.Value = Param.Value;

                        oSQLCommand.Parameters.Add(_TempSQLParameter);
                    }
                }


                if (IsExecuteStatement)
                    _SQLStatement = PreparedStatement + " " + _sbQuery.ToString();
                else
                    _SQLStatement = PreparedStatement;

                oSQLCommand.CommandText = _SQLStatement;

                oSQLCommand.Prepare();

                decimal _sData = (decimal)oSQLCommand.ExecuteScalar();

                _oSQLConnection.Close();
                return _sData.ToString();
            }
            catch (Exception ex)
            {

                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("sPreparedStatement", PreparedStatement);
                oExcpData.AddInputs("EXEC STATEMENT", _SQLStatement);

                ExceptionHandling.LogException(ex, _ClassCode + "005", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };

            }
            finally
            {
                _oSQLConnection.Close();
            }
        }

        public String SQuote(String s)
        {
            return "N'" + s.Replace("'", "''") + "'";
        }


    }
}
