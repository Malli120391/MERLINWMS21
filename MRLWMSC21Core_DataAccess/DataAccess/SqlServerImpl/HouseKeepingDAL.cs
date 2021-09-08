using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using System.Data;
using System.Data.SqlClient;

namespace MRLWMSC21Core.DataAccess
{
    public class HouseKeepingDAL : BaseDAL
    {

        private string _ClassCode = string.Empty;// "WMSCore_DAL_011_";

        public HouseKeepingDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.HouseKeepingDAL);
        }


        public decimal GetMRPValuesForSKU(Inventory oInventory,out decimal CurrentMRP)
        {
            try
            {
                decimal _NewMRP = 0;
                CurrentMRP = 0;

                if (oInventory.RSN == null)
                    throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_HK_BL_0003", WMSMessage = ErrorMessages.WMC_HK_BL_0003, ShowAsError = true };

                string _sSQL = "EXEC [dbo].[USP_API_FetchMaterialMRPInfoForMRPChange] @RSN = '" + oInventory.RSN + "'";

                DataSet _dsMRPData = FillDataSet(_sSQL);

                if (_dsMRPData == null)
                    throw new Exception("_dsMRPData is NULL");
                else if (_dsMRPData.Tables.Count == 0)
                    throw new Exception("Tables Count in _dsMRPData is 0");
                else if (_dsMRPData.Tables[0].Rows.Count == 0)
                    throw new Exception("Rows Count in _dsMRPData is 0");
                else
                {
                    if (_dsMRPData.Tables[0].Rows.Count > 0)
                    {
                        _NewMRP = ConversionUtility.ConvertToDecimal(_dsMRPData.Tables[0].Rows[0]["MasterMRP"].ToString());
                        CurrentMRP = ConversionUtility.ConvertToDecimal(_dsMRPData.Tables[0].Rows[0]["CurrentMRP"].ToString());
                        oInventory.BatchNumber = _dsMRPData.Tables[0].Rows[0]["BatchNumber"].ToString();
                        oInventory.MonthOfMfg = ConversionUtility.ConvertToInt(_dsMRPData.Tables[0].Rows[0]["MFGMonth"].ToString());
                        oInventory.YearOfMfg = ConversionUtility.ConvertToInt(_dsMRPData.Tables[0].Rows[0]["MFGYear"].ToString());
                        oInventory.MOP = ConversionUtility.ConvertToInt(_dsMRPData.Tables[0].Rows[0]["AvailableQty"].ToString());
                        oInventory.StorageLocation = _dsMRPData.Tables[0].Rows[0]["SLoc"].ToString();
                        oInventory.Color = _dsMRPData.Tables[0].Rows[0]["Color"].ToString();
                        oInventory.MaterialShortDescription = _dsMRPData.Tables[0].Rows[0]["MDescr"].ToString();
                    }
                }

                return _NewMRP;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch(Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oInventory", oInventory);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public List<HouseKeepingJob> FetchJobsList(SearchCriteria oCriteria)
        {

            try
            {
                List<HouseKeepingJob> _oHouseKeepingJobs = new List<HouseKeepingJob>();

                string _sSQL = "";


                return _oHouseKeepingJobs;
            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("oCriteria", oCriteria);

                ExceptionHandling.LogException(excp, _ClassCode + "002", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        public string  GetBatchNumberByDenestingJO(string BatchNumber)
        {

            try
            {
                //Method block to Fetch the Entity List from Database.
                string _sSQL = "select top 1 BatchNo from MMT_KitJobOrderHeader where JobOrderRefNo='"+ BatchNumber + "'";

                DataSet _dsResults = FillDataSet(_sSQL, null);

                if (Convert.ToInt32(_dsResults.Tables[0].Rows[0][0]) > 0)
                {

                    return _dsResults.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return ""; ;
                }


            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("RSNNumber", BatchNumber);

                ExceptionHandling.LogException(ex, _ClassCode + "003", oExcpData);

                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }


        }




    }
}
