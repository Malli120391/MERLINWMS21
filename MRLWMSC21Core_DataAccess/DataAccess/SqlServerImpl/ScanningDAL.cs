using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WMSCore_BusinessEntities.Entities;
using WMSCore_DataAccess.DataAccess.Interfaces;

namespace WMSCore_DataAccess.DataAccess.SqlServerImpl
{
    public class ScanningDAL : BaseDAL, iScanDAL
    {
        private string _ClassCode = "WMSCore_DAL_0012_";
        public ScanningDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.CycleCountDAL);
        }
        public ScannedItem ValidateItem(ScannedItem obj)
        {
            try
            {
                // Method block to Fetch the Entity List from Database.
                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_Validate_Material]");

                SqlParameter oParam = new SqlParameter("@Mcode", SqlDbType.NVarChar, 100);
                oParam.Value = obj.SkuCode;
                lSQLParams.Add(oParam);


                if (obj.TenantID != null && obj.TenantID != string.Empty)
                {
                    SqlParameter oParam1 = new SqlParameter("@TenantID", SqlDbType.Int);
                    oParam1.Value = obj.TenantID;
                    lSQLParams.Add(oParam1);

                }


                if (obj.InboundID != null && obj.InboundID != string.Empty)
                {
                    SqlParameter oParam1 = new SqlParameter("@InboundID", SqlDbType.Int);
                    oParam1.Value = obj.InboundID;
                    lSQLParams.Add(oParam1);

                }

                if (obj.ObdNumber != null && obj.ObdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@OBDNumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ObdNumber;
                    lSQLParams.Add(oParam2);

                }

                if (obj.VlpdNumber != null && obj.VlpdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@VLPDNumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.VlpdNumber;
                    lSQLParams.Add(oParam2);

                }


                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {
                    obj.ScanResult = true;

                }
                else
                {
                    obj.ScanResult = false;
                    obj.Message = "Invalid Material scanned";
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0003", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0003, ShowAsError = true };
                }

                return obj;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public ScannedItem ValidateLocation(ScannedItem obj)
        {
            try
            {
                // Method block to Fetch the Entity List from Database.
                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_Validate_Location]");

                SqlParameter oParam = new SqlParameter("@LocationCode", SqlDbType.NVarChar, 100);
                oParam.Value = obj.ScanInput;
                lSQLParams.Add(oParam);

                if (obj.WarehouseID != null && obj.WarehouseID != string.Empty)
                {
                    SqlParameter oParam1 = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam1.Value = obj.WarehouseID;
                    lSQLParams.Add(oParam1);

                }

                if (obj.InboundID != null && obj.InboundID != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@InboundID", SqlDbType.Int);
                    oParam2.Value = obj.InboundID;
                    lSQLParams.Add(oParam2);

                }

                if (obj.ObdNumber != null && obj.ObdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@OBDNumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ObdNumber;
                    lSQLParams.Add(oParam2);

                }

                if (obj.VlpdNumber != null && obj.VlpdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@VLPDNumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.VlpdNumber;
                    lSQLParams.Add(oParam2);

                }

                if (obj.IsCycleCount != 0)
                {

                    SqlParameter oParam2 = new SqlParameter("@IsCycleCount", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.IsCycleCount;
                    lSQLParams.Add(oParam2);

                }

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    if (_dsResults.Tables.Count == 2)
                    {
                        obj.ScanResult = false;
                        obj.Message = "Scanned Location is Blocked For Cycle Count";
                        throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0004", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0004, ShowAsError = true };
                    }

                    else
                    {
                        obj.ScanResult = true;
                    }
                    


                }
                else
                {
                    obj.ScanResult = false;
                    obj.Message = "Invalid Location scanned";
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0002", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0002, ShowAsError = true };


                }

                return obj;


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public ScannedItem ValidatePallet(ScannedItem obj)
        {

            try
            {
                // Method block to Fetch the Entity List from Database.
                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_Validate_Container]");

                SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.NVarChar, 100);
                oParam.Value = obj.ScanInput;
                lSQLParams.Add(oParam);

                if (obj.WarehouseID != null && obj.WarehouseID != string.Empty)
                {
                    SqlParameter oParam1 = new SqlParameter("@WarehouseID", SqlDbType.Int);
                    oParam1.Value = obj.WarehouseID;
                    lSQLParams.Add(oParam1);
                }

                if (obj.InboundID != null && obj.InboundID != string.Empty)
                {
                    SqlParameter oParam1 = new SqlParameter("@InboundID", SqlDbType.Int);
                    oParam1.Value = obj.InboundID;
                    lSQLParams.Add(oParam1);
                }

                if (obj.ObdNumber != null && obj.ObdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@OBDNumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ObdNumber;
                    lSQLParams.Add(oParam2);

                }

                if (obj.VlpdNumber != null && obj.VlpdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@VLPDNumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.VlpdNumber;
                    lSQLParams.Add(oParam2);

                }



                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    obj.ScanResult = true;


                }
                else
                {
                    obj.ScanResult = false;
                    obj.Message = "Invalid Pallet scanned";
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0001", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0001, ShowAsError = true };


                }

                return obj;


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "003", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }


        }

        public ScannedItem ValidateSO(ScannedItem obj)
        {

            try
            {
                // Method block to Fetch the Entity List from Database.
                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[USP_Validate_SalesOrder]");

                SqlParameter oParam = new SqlParameter("@SOOrderNo", SqlDbType.NVarChar, 100);
                oParam.Value = obj.ScanInput;
                lSQLParams.Add(oParam);


                SqlParameter oParam1 = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100); //============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
                oParam1.Value = obj.AccountID;
                lSQLParams.Add(oParam1);

                SqlParameter oParam2 = new SqlParameter("@UserId", SqlDbType.NVarChar, 100);
                oParam2.Value = obj.UserID;
                lSQLParams.Add(oParam2);


                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    obj.ScanResult = true;


                }
                else
                {
                    obj.ScanResult = false;
                    obj.Message = "Packing Not Yet Started for SO Number.";
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0001", WMSMessage = obj.Message, ShowAsError = true };


                }

                return obj;


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }


        }


        public ScannedItem ValidateCarton(ScannedItem obj)
        {

            try
            {
                // Method block to Fetch the Entity List from Database.
                StringBuilder _sbSQL = new StringBuilder();
                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[USP_API_ValidateCarton]");

                SqlParameter oParam = new SqlParameter("@CartonNumber", SqlDbType.NVarChar, 100);
                oParam.Value = obj.ScanInput;
                lSQLParams.Add(oParam);

                if (obj.ObdNumber != null && obj.ObdNumber != string.Empty)
                {

                    SqlParameter oParam2 = new SqlParameter("@SONumber", SqlDbType.NVarChar, 100);
                    oParam2.Value = obj.ObdNumber;
                    lSQLParams.Add(oParam2);
                }
                if (obj.AccountID != 0)
                {

                    SqlParameter oParam3 = new SqlParameter("@AccountID", SqlDbType.NVarChar, 100);
                    oParam3.Value = obj.AccountID;
                    lSQLParams.Add(oParam3);
                }



                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);
                if (_dsResults != null && _dsResults.Tables.Count != 0 && _dsResults.Tables[0].Rows.Count != 0)
                {

                    obj.ScanResult = true;


                }
                else
                {
                    obj.ScanResult = false;
                    obj.Message = "Invalid Carton scanned";
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_DAL_SCAN_0001", WMSMessage = ErrorMessages.WMC_DAL_SCAN_0001, ShowAsError = true };


                }

                return obj;


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("ScannedItem", obj);

                ExceptionHandling.LogException(excp, _ClassCode + "004", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }


        }
    }
}
