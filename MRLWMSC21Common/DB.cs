using System;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace MRLWMSC21Common
{
    /// <summary>
    /// Summary description for DB.
    /// </summary>
    public sealed class DB
    {

        static CultureInfo USCulture = new CultureInfo("en-US");
        static CultureInfo SqlServerCulture = new CultureInfo(CommonLogic.Application("DBSQLServerLocaleSetting"));
        static private DateTime ExpDateString = new DateTime(2022, 12, 30);
        static private String EvalStatusString = "InventoryModule_Expired";

        public DB()
        {
            //Check the Expiry date of this software


        } 

        static private String _activeDBConn = SetDBConn();
        static private String _NoLockString = "N/A";

        static private String SetDBConn()
        {
            String s = CommonLogic.Application("DBConn");
            return s;
        }

        static public String GetDBConn()
        {

            //Check the Expiry date of this software
            if (DateTime.Now > ExpDateString)
            {
                //throw new ArgumentException("Argument expired");
                throw new ArgumentException(EvalStatusString);

            }

            return _activeDBConn;

        }

        static public String GetNoLock()
        {
            if (_NoLockString == "N/A")
            {
                // must set it the first time:
                _NoLockString = String.Empty;
                if (CommonLogic.ApplicationBool("UseSQLNoLock") || CommonLogic.ApplicationBool("SQLNoLock"))
                {
                    _NoLockString = " with (NOLOCK) ";
                }
            }
            return _NoLockString;
        }

        public static String SQuote(String s)
        {
            try
            {

                return "N'" + s.Replace("'", "''") + "'";

            }
            catch (Exception)
            {

                return "''";
            }
        }

        public static String SQuoteNotUnicode(String s)
        {
            return "'" + s.Replace("'", "''") + "'";
        }

        public static String DateQuote(String s)
        {
            return "'" + s.Replace("'", "''") + "'";
        }

        public static String DateQuote(DateTime dt)
        {
            return DateQuote(Localization.ToDBDateTimeString(dt));
        }

        static public IDataReader GetRS(String Sql)
        {
            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SQL=" + Sql + "<br/>\n");
            }
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
            SqlCommand cmd = new SqlCommand(Sql, dbconn);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }



        static public IDataReader SPGetRS(String procCall)
        {
            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SP=" + procCall + "<br/>\n");
            }
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
#pragma warning disable SG0026 // Potential SQL injection with MsSQL Data Provider
            SqlCommand cmd = new SqlCommand(procCall, dbconn)
            {
                CommandType = CommandType.StoredProcedure
            };
#pragma warning restore SG0026 // Potential SQL injection with MsSQL Data Provider
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        static public void ExecuteSQL(String Sql)
        {

            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SQL=" + Sql + "<br/>\n");
            }
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
#pragma warning disable SG0026 // Potential SQL injection with MsSQL Data Provider
            SqlCommand cmd = new SqlCommand(Sql, dbconn);
            cmd.CommandTimeout = 600;
#pragma warning restore SG0026 // Potential SQL injection with MsSQL Data Provider
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
                throw (ex);
            }
        }

        static public void ExecuteStoredProcedure(String ProcName, SqlParameter[] SPParmCollection)
        {

            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SP=" + ProcName + "<br/>\n");
            }
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
#pragma warning disable SG0026 // Potential SQL injection with MsSQL Data Provider
            SqlCommand cmd = new SqlCommand(ProcName, dbconn)
            {
                CommandType = CommandType.StoredProcedure
            };
#pragma warning restore SG0026 // Potential SQL injection with MsSQL Data Provider

            foreach (SqlParameter spParm in SPParmCollection)
            {
                cmd.Parameters.AddWithValue(spParm.ParameterName, spParm.Value);
                if (spParm.ParameterName == "@MatAssigned")
                {
                    spParm.SqlDbType = SqlDbType.Structured;
                }
            }




            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
                throw (ex);
            }
        }


        // NOTE FOR DB ACCESSOR FUNCTIONS: AdminSite try/catch block is needed until
        // we convert to the new admin page styles. Our "old" db accessors handled empty
        // recordset conditions, so we need to preserve that for the admin site to add 
        // new products/categories/etc...
        //
        // We do not use try/catch on the store site for speed

        // ----------------------------------------------------------------
        //
        // SIMPLE ROW FIELD ROUTINES
        //
        // ----------------------------------------------------------------

        public static String RowField(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return String.Empty;
                    }
                    return Convert.ToString(row[fieldname]);
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return String.Empty;
                }
                return Convert.ToString(row[fieldname]);
            }
        }

        public static String RowFieldByLocale(DataRow row, String fieldname, String LocaleSetting)
        {
            String tmpS = String.Empty;
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        tmpS = String.Empty;
                    }
                    else
                    {
                        tmpS = Convert.ToString(row[fieldname]);
                    }
                }
                catch
                {
                    tmpS = String.Empty;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    tmpS = String.Empty;
                }
                else
                {
                    tmpS = Convert.ToString(row[fieldname]);
                }
            }
#if PRO
			return tmpS;
#else
            return XmlCommon.GetLocaleEntry(tmpS, LocaleSetting, true);
#endif
        }

        // uses xpath spec to look into the field value and return a node innertext
        public static String RowFieldByXPath(DataRow row, String fieldname, String XPath)
        {
            String tmpS = String.Empty;
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        tmpS = String.Empty;
                    }
                    else
                    {
                        tmpS = Convert.ToString(row[fieldname]);
                    }
                }
                catch
                {
                    tmpS = String.Empty;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    tmpS = String.Empty;
                }
                else
                {
                    tmpS = Convert.ToString(row[fieldname]);
                }
            }
            return XmlCommon.GetXPathEntry(tmpS, XPath);
        }

        public static bool RowFieldBool(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return false;
                    }
                    String s = row[fieldname].ToString().ToUpperInvariant();
                    return (s == "TRUE" || s == "YES" || s == "1");
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return false;
                }
                String s = row[fieldname].ToString().ToUpperInvariant();
                return (s == "TRUE" || s == "YES" || s == "1");
            }
        }

        public static Byte RowFieldByte(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return 0;
                    }
                    return Convert.ToByte(row[fieldname]);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return 0;
                }
                return Convert.ToByte(row[fieldname]);
            }
        }

        public static String RowFieldGUID(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return String.Empty;
                    }
                    return Convert.ToString(row[fieldname]);
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return String.Empty;
                }
                return Convert.ToString(row[fieldname]);
            }
        }

        public static int RowFieldInt(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(row[fieldname]);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return 0;
                }
                return Convert.ToInt32(row[fieldname]);
            }
        }

        public static long RowFieldLong(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return 0;
                    }
                    return Convert.ToInt64(row[fieldname]);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return 0;
                }
                return Convert.ToInt64(row[fieldname]);
            }
        }

        public static Single RowFieldSingle(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return 0.0F;
                    }
                    return Convert.ToSingle(row[fieldname]);
                }
                catch
                {
                    return 0.0F;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return 0.0F;
                }
                return Convert.ToSingle(row[fieldname]);
            }
        }

        public static Double RowFieldDouble(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return 0.0F;
                    }
                    return Convert.ToDouble(row[fieldname]);
                }
                catch
                {
                    return 0.0F;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return 0.0F;
                }
                return Convert.ToDouble(row[fieldname]);
            }
        }

        public static Decimal RowFieldDecimal(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return System.Decimal.Zero;
                    }
                    return Convert.ToDecimal(row[fieldname]);
                }
                catch
                {
                    return System.Decimal.Zero;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return System.Decimal.Zero;
                }
                return Convert.ToDecimal(row[fieldname]);
            }
        }


        public static DateTime RowFieldDateTime(DataRow row, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    if (Convert.IsDBNull(row[fieldname]))
                    {
                        return System.DateTime.MinValue;
                    }
                    return Convert.ToDateTime(row[fieldname], SqlServerCulture);
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            else
            {
                if (Convert.IsDBNull(row[fieldname]))
                {
                    return System.DateTime.MinValue;
                }
                return Convert.ToDateTime(row[fieldname], SqlServerCulture);
            }
        }

        // ----------------------------------------------------------------
        //
        // SIMPLE RS FIELD ROUTINES
        //
        // ----------------------------------------------------------------

        public static String RSField(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return String.Empty;
                    }
                    return rs.GetString(idx);
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return String.Empty;
                }
                return rs.GetString(idx);
            }
        }

        public static String RSFieldByLocale(IDataReader rs, String fieldname, String LocaleSetting)
        {
            String tmpS = String.Empty;
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        tmpS = String.Empty;
                    }
                    else
                    {
                        tmpS = rs.GetString(idx);
                    }
                }
                catch (Exception ex)
                {
                    tmpS = String.Empty;

                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    tmpS = String.Empty;
                }
                else
                {
                    tmpS = rs.GetString(idx);
                }
            }
#if PRO
			return tmpS;
#else
            return XmlCommon.GetLocaleEntry(tmpS, LocaleSetting, true);
#endif
        } 

        // uses xpath spec to look into the field value and return a node innertext
        public static String RSFieldByXPath(IDataReader rs, String fieldname, String XPath)
        {
            String tmpS = String.Empty;
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        tmpS = String.Empty;
                    }
                    else
                    {
                        tmpS = rs.GetString(idx);
                    }
                }
                catch
                {
                    tmpS = String.Empty;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    tmpS = String.Empty;
                }
                else
                {
                    tmpS = rs.GetString(idx);
                }
            }
            return XmlCommon.GetXPathEntry(tmpS, XPath);
        }

        public static bool RSFieldBool(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return false;
                    }
                    String s = rs[fieldname].ToString().ToUpperInvariant();
                    return (s == "TRUE" || s == "YES" || s == "1");
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return false;
                }
                String s = rs[fieldname].ToString().ToUpperInvariant();
                return (s == "TRUE" || s == "YES" || s == "1");
            }
        }

        public static Guid RSFieldGUID(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return Guid.Empty;
                    }
                    return rs.GetGuid(idx);
                }
                catch
                {
                    return Guid.Empty;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return Guid.Empty;
                }
                return rs.GetGuid(idx);
            }
        }

        public static Byte RSFieldByte(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return 0;
                    }
                    return rs.GetByte(idx);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return 0;
                }
                return rs.GetByte(idx);
            }
        }

        public static int RSFieldInt(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return 0;
                    }
                    return rs.GetInt32(idx);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return 0;
                }
                return rs.GetInt32(idx);
            }
        }

        public static int RSFieldTinyInt(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return 0;
                    }
                    return Localization.ParseNativeInt(rs[idx].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return 0;
                }
                return Localization.ParseNativeInt(rs[idx].ToString());
            }
        }

        public static long RSFieldLong(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return 0;
                    }
                    return rs.GetInt64(idx);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return 0;
                }
                return rs.GetInt64(idx);
            }
        }

        public static Single RSFieldSingle(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return 0.0F;
                    }
                    return (Single)rs.GetDouble(idx); // SQL server seems to fail the GetFloat calls, so we have to do this
                }
                catch
                {
                    return 0.0F;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return 0.0F;
                }
                return (Single)rs.GetDouble(idx); // SQL server seems to fail the GetFloat calls, so we have to do this
            }
        }

        public static Double RSFieldDouble(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return 0.0F;
                    }
                    return rs.GetDouble(idx);
                }
                catch
                {
                    return 0.0F;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return 0.0F;
                }
                return rs.GetDouble(idx);
            }
        }

        public static Decimal RSFieldDecimal(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return System.Decimal.Zero;
                    }
                    return rs.GetDecimal(idx);
                }
                catch
                {
                    return System.Decimal.Zero;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return System.Decimal.Zero;
                }
                return rs.GetDecimal(idx);
            }
        }

        public static DateTime RSFieldDateTime(IDataReader rs, String fieldname)
        {
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return System.DateTime.MinValue; // ---This line is commented and following line is added since SQL 2005 accepts values from 1/1/1753 - 12/31/9999
                                                         //The above line returns  01/01/0001 
                                                         // return DateTime.Parse("1/1/1753");
                    }
                    return Convert.ToDateTime(rs[idx], SqlServerCulture);

                    //return rs.GetDateTime(idx);
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return System.DateTime.MinValue; // ---This line is commented and following line is added since SQL 2005 accepts values from 1/1/1753 - 12/31/9999
                    //The above line returns  01/01/0001 
                    //return DateTime.Parse("1/1/1753");
                }
                return Convert.ToDateTime(rs[idx], SqlServerCulture);
                //return rs.GetDateTime(idx);
            }
        }

        public static TimeSpan RSFieldTime(IDataReader rs, String fieldname)
        {
            TimeSpan span;
            if (AppLogic.IsAdminSite)
            {
                try
                {
                    int idx = rs.GetOrdinal(fieldname);
                    if (rs.IsDBNull(idx))
                    {
                        return System.TimeSpan.MinValue; // ---This line is commented and following line is added since SQL 2005 accepts values from 1/1/1753 - 12/31/9999
                        //The above line returns  01/01/0001 
                        // return DateTime.Parse("1/1/1753");
                    }

                    if (System.TimeSpan.TryParse(rs[idx].ToString(), out span))
                        return span;
                    else
                        return System.TimeSpan.MinValue;


                    //return rs.GetDateTime(idx);
                }
                catch
                {
                    return System.TimeSpan.MinValue;
                }
            }
            else
            {
                int idx = rs.GetOrdinal(fieldname);
                if (rs.IsDBNull(idx))
                {
                    return System.TimeSpan.MinValue; // ---This line is commented and following line is added since SQL 2005 accepts values from 1/1/1753 - 12/31/9999
                    //The above line returns  01/01/0001 
                    //return DateTime.Parse("1/1/1753");
                }


                if (System.TimeSpan.TryParse(rs[idx].ToString(), out span))
                    return span;
                else
                    return System.TimeSpan.MinValue;
                //return rs.GetDateTime(idx);
            }
        }

        public static DataSet GetTable(String tablename, String orderBy, String cacheName, bool useCache)
        {
            if (useCache)
            {
                DataSet cacheds = (DataSet)HttpContext.Current.Cache.Get(cacheName);
                if (cacheds != null)
                {
                    return cacheds;
                }
            }
            DataSet ds = new DataSet();
            String Sql = String.Empty;
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
            Sql = "select * from " + tablename + " order by " + orderBy;
            SqlDataAdapter da = new SqlDataAdapter(Sql, dbconn);
            da.Fill(ds, tablename);
            dbconn.Close();
            if (useCache)
            {
                HttpContext.Current.Cache.Insert(cacheName, ds);
            }
            return ds;
        }

        /// <summary>
        /// Incrementally adds tables results to a dataset
        /// </summary>
        /// <param name="ds">Dataset to add the table to</param>
        /// <param name="tableName">Name of the table to be created in the DataSet</param>
        /// <param name="sqlQuery">Query to retrieve the data for the table.</param>
        static public int FillDataSet(DataSet ds, string tableName, string sqlQuery)
        {
            int n = 0;
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, dbconn);
            n = da.Fill(ds, tableName);
            dbconn.Close();
            return n;
        }

        public static DataSet GetDS(String Sql, String cacheName, bool useCache, System.DateTime expiresAt)
        {
            if (useCache)
            {
                DataSet cacheds = (DataSet)HttpContext.Current.Cache.Get(cacheName);
                if (cacheds != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("SQL=[From Cache]" + Sql + "<br/>");
                    }
                    return cacheds;
                }
            }
            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SQL=" + Sql + "<br/>");
            }
            DataSet ds = new DataSet();
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };


            dbconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(Sql, dbconn);
            da.SelectCommand.CommandTimeout = 90; // Will allow the Data set to be filled in 90 Secs
            da.Fill(ds, "Table");
            dbconn.Close();
            if (useCache)
            {
                HttpContext.Current.Cache.Insert(cacheName, ds, null, expiresAt, TimeSpan.Zero);
            }
            return ds;
        }

        public static DataSet GetDS(String Sql, String cacheName, bool useCache)
        {
            return GetDS(Sql, cacheName, useCache, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()));
        }

        public static DataSet GetDS(String Sql, bool useCache, System.DateTime expiresAt)
        {
            return GetDS(Sql, "DataSet: " + Sql.ToUpperInvariant(), useCache, expiresAt);
        }

        public static DataSet GetDS(String Sql, bool useCache)
        {
            return GetDS(Sql, useCache, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()));
        }

        public static String GetNewGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        static public int GetSqlN(String Sql)
        {
            int N = 0;
            IDataReader rs;
            rs = DB.GetRS(Sql);
            if (rs.Read())
            {
                N = DB.RSFieldInt(rs, "N");
            }
            rs.Close();
            return N;
        }

        static public Single GetSqlNSingle(String Sql)
        {
            Single N = 0.0F;
            IDataReader rs = DB.GetRS(Sql);
            if (rs.Read())
            {
                N = DB.RSFieldSingle(rs, "N");
            }
            rs.Close();
            return N;
        }

        static public decimal GetSqlNDecimal(String Sql)
        {
            decimal N = System.Decimal.Zero;
            IDataReader rs = DB.GetRS(Sql);
            if (rs.Read())
            {
                N = DB.RSFieldDecimal(rs, "N");
            }
            rs.Close();
            return N;
        }

        static public int GetSqlTinyInt(String Sql)
        {
            int TI = 0;
            IDataReader rs = DB.GetRS(Sql);
            if (rs.Read())
            {
                TI = DB.RSFieldTinyInt(rs, "TI");
            }
            rs.Close();
            return TI;
        }

        static public void ExecuteLongTimeSQL(String Sql, int TimeoutSecs)
        {
            ExecuteLongTimeSQL(Sql, DB.GetDBConn(), TimeoutSecs);
        }

        static public String GetSqlS(String Sql)
        {
            String S = String.Empty;
            IDataReader rs = DB.GetRS(Sql);
            if (rs.Read())
            {
                S = DB.RSFieldByLocale(rs, "S", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
                if (S.Equals(DBNull.Value))
                {
                    S = String.Empty;
                }
            }
            rs.Close();
            return S;
        }

        static public String GetSqlSAllLocales(String Sql)
        {
            String S = String.Empty;
            IDataReader rs = DB.GetRS(Sql);
            if (rs.Read())
            {
                S = DB.RSField(rs, "S");
                if (S.Equals(DBNull.Value))
                {
                    S = String.Empty;
                }
            }
            rs.Close();
            return S;
        }

        static public string GetSqlXml(String Sql, string xslTranformFile)
        {
            if (Sql.ToUpper(CultureInfo.InvariantCulture).IndexOf("FOR XML") < 1)
            {
                Sql += " FOR XML AUTO";
            }
            StringBuilder s = new StringBuilder(4096);
            IDataReader rs = DB.GetRS(Sql);
            while (rs.Read())
            {
                s.Append(rs.GetString(0));
            }
            rs.Close();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml("<root>" + s.ToString() + "</root>");
            if (xslTranformFile != null && xslTranformFile.Trim() != "")
            {
                XslCompiledTransform xsl = new XslCompiledTransform();
                xsl.Load(xslTranformFile);
                TextWriter tw = new StringWriter();
                xsl.Transform(xdoc, null, tw);
                return tw.ToString();
            }
            else
            {
                return xdoc.DocumentElement.InnerXml;
            }
        }

        static public XmlDocument GetSqlXmlDoc(String Sql, string xslTranformFile)
        {
            if (Sql.ToUpper(CultureInfo.InvariantCulture).IndexOf("FOR XML") < 1)
            {
                Sql += " FOR XML AUTO";
            }
            StringBuilder s = new StringBuilder(4096);
            IDataReader rs = DB.GetRS(Sql);
            while (rs.Read())
            {
                s.Append(rs.GetString(0));
            }
            rs.Close();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml("<root>" + s.ToString() + "</root>");
            if (xslTranformFile != null && xslTranformFile.Trim() != "")
            {
                XslCompiledTransform xsl = new XslCompiledTransform();
                xsl.Load(xslTranformFile);
                TextWriter tw = new StringWriter();
                xsl.Transform(xdoc, null, tw);
                xdoc.LoadXml(tw.ToString());
            }
            return xdoc;
        }

        static public string GetSqlXml(String Sql, XslCompiledTransform xsl, XsltArgumentList xslArgs)
        {
            if (Sql.ToLower().IndexOf("for xml") < 1)
            {
                Sql += " for xml auto";
            }
            StringBuilder s = new StringBuilder(4096);
            IDataReader rs = DB.GetRS(Sql);
            while (rs.Read())
            {
                s.Append(rs.GetString(0));
            }
            rs.Close();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml("<root>" + s.ToString() + "</root>");
            if (xsl != null)
            {
                TextWriter tw = new StringWriter();
                xsl.Transform(xdoc, xslArgs, tw);
                return tw.ToString();
            }
            else
            {
                return xdoc.DocumentElement.InnerXml;
            }
        }

        static public int GetXml(SqlDataReader dr, string rootEl, string rowEl, bool IsPagingProc, ref StringBuilder Xml)
        {
            int rows = 0;
            if (rootEl.Length != 0)
            {
                Xml.Append("<" + rootEl + ">");
            }
            while (dr.Read())
            {
                ++rows;
                if (rowEl.Length == 0)
                {
                    Xml.Append("<row>");
                }
                else
                {
                    Xml.Append("<" + rowEl + ">");
                }
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string elname = dr.GetName(i).Replace(" ", "_");
                    if (dr.IsDBNull(i))
                    {
                        Xml.Append("<" + elname + "/>");
                    }
                    else
                    {
                        if (System.Convert.ToString(dr.GetValue(i)).StartsWith("<ml>"))
                        {
                            Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)) + "</" + elname + ">");
                        }
                        else
                        {
                            Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</" + elname + ">");
                        }
                    }
                }
                if (rowEl.Length == 0)
                {
                    Xml.Append("</row>");
                }
                else
                {
                    Xml.Append("</" + rowEl + ">");
                }

            }
            if (rootEl.Length != 0)
            {
                Xml.Append("</" + rootEl + ">");
            }
            int ResultSet = 1;
            while (dr.NextResult())
            {
                ResultSet++;
                if (IsPagingProc)
                {
                    if (rootEl.Length != 0)
                    {
                        Xml.Append("<" + rootEl + "Paging>");
                    }
                    else
                    {
                        Xml.Append("<Paging>");
                    }
                }
                else
                {
                    if (rootEl.Length != 0)
                    {
                        Xml.Append("<" + rootEl + ResultSet.ToString() + ">");
                    }
                }
                while (dr.Read())
                {
                    if (!IsPagingProc)
                    {
                        if (rowEl.Length == 0)
                        {
                            Xml.Append("<row>");
                        }
                        else
                        {
                            Xml.Append("<" + rowEl + ">");
                        }
                    }
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string elname = dr.GetName(i).Replace(" ", "_");
                        if (dr.IsDBNull(i))
                        {
                            Xml.Append("<" + elname + "/>");
                        }
                        else
                        {
                            if (System.Convert.ToString(dr.GetValue(i)).StartsWith("<ml>"))
                            {
                                Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)) + "</" + elname + ">");
                            }
                            else
                            {
                                Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</" + elname + ">");
                            }
                        }
                    }
                    if (!IsPagingProc)
                    {
                        if (rowEl.Length == 0)
                        {
                            Xml.Append("</row>");
                        }
                        else
                        {
                            Xml.Append("</" + rowEl + ">");
                        }
                    }
                }
                if (IsPagingProc)
                {
                    if (rootEl.Length != 0)
                    {
                        Xml.Append("</" + rootEl + "Paging>");
                    }
                    else
                    {
                        Xml.Append("</Paging>");
                    }
                }
                else
                {
                    if (rootEl.Length != 0)
                    {
                        Xml.Append("</" + rootEl + ResultSet.ToString() + ">");
                    }
                }
            }
            dr.Close();
            return rows;
        }


        // assumes a 2nd result set is the PAGING INFO back from the aspdnsf_PageQuery proc!!!
        // looks for aspdnsf_PageQuery in the Sql input to determine this!
        static public int GetXml(string Sql, string rootEl, string rowEl, ref string Xml)
        {
            bool IsPagingProc = (Sql.IndexOf("aspdnsf_PageQuery") != -1);
            StringBuilder s = new StringBuilder(4096);
            IDataReader rs = DB.GetRS(Sql);
            int rows = 0;
            if (rootEl.Length != 0)
            {
                s.Append("<" + rootEl + ">");
            }
            while (rs.Read())
            {
                ++rows;
                if (rowEl.Length == 0)
                {
                    s.Append("<row>");
                }
                else
                {
                    s.Append("<" + rowEl + ">");
                }
                for (int i = 0; i < rs.FieldCount; i++)
                {
                    string elname = rs.GetName(i).Replace(" ", "_");
                    if (rs.IsDBNull(i))
                    {
                        s.Append("<" + elname + "/>");
                    }
                    else
                    {
                        if (System.Convert.ToString(rs.GetValue(i)).StartsWith("<ml>"))
                        {
                            s.Append("<" + elname + ">" + System.Convert.ToString(rs.GetValue(i)) + "</" + elname + ">");
                        }
                        else
                        {
                            s.Append("<" + elname + ">" + System.Convert.ToString(rs.GetValue(i)).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</" + elname + ">");
                        }
                    }
                }
                if (rowEl.Length == 0)
                {
                    s.Append("</row>");
                }
                else
                {
                    s.Append("</" + rowEl + ">");
                }

            }
            if (rootEl.Length != 0)
            {
                s.Append("</" + rootEl + ">");
            }
            int ResultSet = 1;
            while (rs.NextResult())
            {
                ResultSet++;
                if (IsPagingProc)
                {
                    if (rootEl.Length != 0)
                    {
                        s.Append("<" + rootEl + "Paging>");
                    }
                    else
                    {
                        s.Append("<Paging>");
                    }
                }
                else
                {
                    if (rootEl.Length != 0)
                    {
                        s.Append("<" + rootEl + ResultSet.ToString() + ">");
                    }
                }
                while (rs.Read())
                {
                    if (!IsPagingProc)
                    {
                        if (rowEl.Length == 0)
                        {
                            s.Append("<row>");
                        }
                        else
                        {
                            s.Append("<" + rowEl + ">");
                        }
                    }
                    for (int i = 0; i < rs.FieldCount; i++)
                    {
                        string elname = rs.GetName(i).Replace(" ", "_");
                        if (rs.IsDBNull(i))
                        {
                            s.Append("<" + elname + "/>");
                        }
                        else
                        {
                            if (System.Convert.ToString(rs.GetValue(i)).StartsWith("<ml>"))
                            {
                                s.Append("<" + elname + ">" + System.Convert.ToString(rs.GetValue(i)) + "</" + elname + ">");
                            }
                            else
                            {
                                s.Append("<" + elname + ">" + System.Convert.ToString(rs.GetValue(i)).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</" + elname + ">");
                            }
                        }
                    }
                    if (!IsPagingProc)
                    {
                        if (rowEl.Length == 0)
                        {
                            s.Append("</row>");
                        }
                        else
                        {
                            s.Append("</" + rowEl + ">");
                        }
                    }
                }
                if (IsPagingProc)
                {
                    if (rootEl.Length != 0)
                    {
                        s.Append("</" + rootEl + "Paging>");
                    }
                    else
                    {
                        s.Append("</Paging>");
                    }
                }
                else
                {
                    if (rootEl.Length != 0)
                    {
                        s.Append("</" + rootEl + ResultSet.ToString() + ">");
                    }
                }
            }
            rs.Close();
            Xml = s.ToString();
            return rows;
        }

        static public int GetENLocaleXml(SqlDataReader dr, string rootEl, string rowEl, ref StringBuilder Xml)
        {
            int rows = 0;
            if (rootEl.Length != 0)
            {
                Xml.Append("<" + rootEl + ">");
            }
            while (dr.Read())
            {
                ++rows;
                if (rowEl.Length == 0)
                {
                    Xml.Append("<row>");
                }
                else
                {
                    Xml.Append("<" + rowEl + ">");
                }
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string elname = dr.GetName(i).Replace(" ", "_");
                    if (dr.IsDBNull(i))
                    {
                        Xml.Append("<" + elname + "/>");
                    }
                    else
                    {
                        if (System.Convert.ToString(dr.GetValue(i)).StartsWith("<ml>"))
                        {
                            Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)) + "</" + elname + ">");
                        }
                        else
                        {
                            string FieldVal = string.Empty;
                            string FieldDataType = dr.GetDataTypeName(i).ToLower();
                            if ((FieldDataType == "decimal" || FieldDataType == "money") && CommonLogic.Application("DBSQLServerLocaleSetting") != "en-US")
                            {
                                FieldVal = Localization.ParseLocaleDecimal(dr.GetDecimal(i).ToString(), "en-US").ToString();
                            }
                            else if (dr.GetDataTypeName(i).ToLower() == "datetime" && CommonLogic.Application("DBSQLServerLocaleSetting") != "en-US")
                            {
                                FieldVal = Localization.ParseLocaleDateTime(dr.GetDateTime(i).ToString(), "en-US").ToString();
                            }
                            else
                            {
                                FieldVal = dr.GetValue(i).ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            }
                            Xml.Append("<" + elname + ">" + FieldVal + "</" + elname + ">");
                        }
                    }
                }
                if (rowEl.Length == 0)
                {
                    Xml.Append("</row>");
                }
                else
                {
                    Xml.Append("</" + rowEl + ">");
                }

            }
            if (rootEl.Length != 0)
            {
                Xml.Append("</" + rootEl + ">");
            }
            int ResultSet = 1;
            while (dr.NextResult())
            {
                ResultSet++;
                if (rootEl.Length != 0)
                {
                    Xml.Append("<" + rootEl + ResultSet.ToString() + ">");
                }
                while (dr.Read())
                {
                    if (rowEl.Length == 0)
                    {
                        Xml.Append("<row>");
                    }
                    else
                    {
                        Xml.Append("<" + rowEl + ">");
                    }
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string elname = dr.GetName(i).Replace(" ", "_");
                        if (dr.IsDBNull(i))
                        {
                            Xml.Append("<" + elname + "/>");
                        }
                        else
                        {
                            if (System.Convert.ToString(dr.GetValue(i)).StartsWith("<ml>"))
                            {
                                Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)) + "</" + elname + ">");
                            }
                            else
                            {
                                string FieldVal = string.Empty;
                                if (dr.GetDataTypeName(i).ToLower() == "decimal" && CommonLogic.Application("DBSQLServerLocaleSetting") != "en-US")
                                {
                                    FieldVal = Localization.ParseLocaleDecimal(dr.GetDecimal(i).ToString(), "en-US").ToString();
                                }
                                else if (dr.GetDataTypeName(i).ToLower() == "datetime" && CommonLogic.Application("DBSQLServerLocaleSetting") != "en-US")
                                {
                                    FieldVal = Localization.ParseLocaleDateTime(dr.GetDateTime(i).ToString(), "en-US").ToString();
                                }
                                else
                                {
                                    FieldVal = dr.GetValue(i).ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                }
                                Xml.Append("<" + elname + ">" + System.Convert.ToString(dr.GetValue(i)).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</" + elname + ">");
                            }
                        }
                    }
                    if (rowEl.Length == 0)
                    {
                        Xml.Append("</row>");
                    }
                    else
                    {
                        Xml.Append("</" + rowEl + ">");
                    }
                }
                if (rootEl.Length != 0)
                {
                    Xml.Append("</" + rootEl + ResultSet.ToString() + ">");
                }
            }
            dr.Close();
            return rows;
        }

        static public void ExecuteLongTimeSQL(String Sql, String DBConnString, int TimeoutSecs)
        {
            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SQL=" + Sql + "<br/>\n");
            }
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
#pragma warning disable SG0026 // Potential SQL injection with MsSQL Data Provider
            SqlCommand cmd = new SqlCommand(Sql, dbconn)
            {
                CommandTimeout = TimeoutSecs
            };
#pragma warning restore SG0026 // Potential SQL injection with MsSQL Data Provider
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
                throw (ex);
            }
        }

        static public Object ExecuteStoredProcedureWithOutputParm(String ProcName, SqlParameter[] SPParmCollection, String OutputParmName, SqlParameter sqlParameter1)
        {

            object objResult;
            if (CommonLogic.ApplicationBool("DumpSQL"))
            {
                HttpContext.Current.Response.Write("SP=" + ProcName + "<br/>\n");
            }
            SqlConnection dbconn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            dbconn.Open();
#pragma warning disable SG0026 // Potential SQL injection with MsSQL Data Provider
            SqlCommand cmd = new SqlCommand(ProcName, dbconn)
            {
                CommandType = CommandType.StoredProcedure
            };
#pragma warning restore SG0026 // Potential SQL injection with MsSQL Data Provider

            foreach (SqlParameter spParm in SPParmCollection)
            {
                cmd.Parameters.AddWithValue(spParm.ParameterName, spParm.Value);
            }
            cmd.Parameters.Add(sqlParameter1);
            try
            {
                cmd.ExecuteNonQuery();
                objResult = cmd.Parameters[OutputParmName].Value;
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                dbconn.Close();
                dbconn.Dispose();
                throw (ex);
            }

            return objResult;
        }

    }

    // currently only supported for SQL Server:
    public class DBTransaction
    {
        private ArrayList sqlCommands = new ArrayList(10);

        public DBTransaction() { }

        public void AddCommand(String Sql)
        {
            sqlCommands.Add(Sql);
        }

        public void ClearCommands()
        {
            sqlCommands.Clear();
        }

        // returns true if no errors, or false if ANY Exception is found:
        public bool Commit()
        {
            SqlConnection conn = new SqlConnection
            {
                ConnectionString = DB.GetDBConn()
            };
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            bool status = false;
            try
            {

                foreach (String s in sqlCommands)
                {
#pragma warning disable SG0026 // Potential SQL injection with MsSQL Data Provider
                    SqlCommand comm = new SqlCommand(s, conn)
                    {
                        Transaction = trans
                    };
#pragma warning restore SG0026 // Potential SQL injection with MsSQL Data Provider
                    comm.ExecuteNonQuery();
                }
                trans.Commit();
                status = true;
            }
            catch //(SqlException ex)
            {
                trans.Rollback();
                status = false;
            }
            finally
            {
                conn.Close();
            }
            return status;
        }

    }


}


