using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.DataAccess
{
    public class PalletDAL : BaseDAL, Interfaces.IPalletDAL
    {
        private string _ClassCode = string.Empty;

        public PalletDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.PalletDAL);
        }

        public Location GetPalletByID(int PalletID)
        {
            throw new NotImplementedException();
        }

        public List<Pallet> GetPallets(SearchCriteria Criteria)
        {
            try
            {
                List<Pallet> _lPalletList = new List<Pallet>();

                StringBuilder _sbSQL = new StringBuilder();

                List<SqlParameter> lSQLParams = new List<SqlParameter>();

                _sbSQL.Append("EXEC [dbo].[sp_API_INV_FetchCartonList] ");

                if (Criteria.ContainerID > 0)
                {
                    SqlParameter oParam = new SqlParameter("@CartonID", SqlDbType.Int);
                    oParam.Value = Criteria.ContainerID;

                    lSQLParams.Add(oParam);
                }

                if (Criteria.ContainerCode != null)
                {
                    SqlParameter oParam = new SqlParameter("@CartonCode", SqlDbType.Int);
                    oParam.Value = Criteria.ContainerCode;

                    lSQLParams.Add(oParam);
                }

                DataSet _dsResults = base.FillDataSet(_sbSQL.ToString(), lSQLParams);

                if (_dsResults != null)

                    if (_dsResults.Tables.Count > 0)
                    {
                        foreach (DataRow _drLocation in _dsResults.Tables[0].Rows)
                        {
                            Pallet _TempLocation;
                            _TempLocation = BindPalletData(_drLocation);

                            _lPalletList.Add(_TempLocation);

                        }
                    }
                return _lPalletList;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception excp)
            {
                ExceptionData oExcpData = new ExceptionData();
                oExcpData.AddInputs("Criteria", Criteria);

                ExceptionHandling.LogException(excp, _ClassCode + "001", oExcpData);
                throw new WMSExceptionMessage() { WMSExceptionCode = ErrorMessages.WMSExceptionCode, WMSMessage = ErrorMessages.WMSExceptionMessage, ShowAsCriticalError = true };
            }
        }

        private Pallet BindPalletData(DataRow _drLocation)
        {
            Pallet _oPallet = new Pallet()
            {
                PalletID = ConversionUtility.ConvertToInt(_drLocation["CARTONID"].ToString()),
                PalletCode = _drLocation["CartonCode"].ToString(),

            };

            return _oPallet;

        }
    }
}
