using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common;

namespace SAPIntegration.INOUT
{
    public class CommonDAO
    {
        public static int GetCurrencyID(string CurrencyCode)
        {
            try
            {
                

                return DB.GetSqlN("select CurrencyID AS N from GEN_Currency where Code='" + CurrencyCode + "'");
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int GetCountryID(string CountryCode)
        {
            try
            {
                return DB.GetSqlN("select CountryMasterID AS N from GEN_CountryMaster where CountryCode='" + CountryCode + "'");
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int GetMMID(string PartNumber)
        {
            try
            {
                return DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MCode='" + PartNumber + "'");
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public static int GetMM_UoMID(int MMID, string UoM, String UoMType)
        {
            try
            {
                return DB.GetSqlN("select MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where   UoMTypeID=" + (UoMType != "1" ? "2" : UoMType) + " AND isactive=1 AND isdeleted=0 AND UoMID=" + GetUoMID(UoM) + "  AND  MaterialMasterID=" + MMID);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int GetUoMID(string UoM)
        {
            try
            {
                return DB.GetSqlN("select UoMID AS N from GEN_UoM where IsActive=1 AND IsDeleted=0 AND UoM='" + UoM + "'");
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
