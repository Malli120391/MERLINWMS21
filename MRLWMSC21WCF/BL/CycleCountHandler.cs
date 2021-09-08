using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using System.Text;

namespace MRLWMSC21WCF.BL
{
    public class CycleCountHandler
    {

        public UserDataTable GetCCNames(BO.CycleCount cycleCount)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_CCName_HHT] @AcoountId=" + cycleCount.AccountId + ",@UserID=" + cycleCount.UserId, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }

        public BO.CycleCount IsBlockedLocation(BO.CycleCount cycleCount)
        {
            cycleCount = CheckLocationForCycleCount(cycleCount);
            if (cycleCount.Result == "")
            {
                //string sql = "select convert(int,IsBlockedForCycleCount) AS N from INV_Location where Location=" + DB.SQuote(cycleCount.Location) + " and IsActive=1 and ZoneId in(select LocationZoneID from INV_LocationZone where WarehouseID=" + cycleCount.WarehouseID + ")";
                string sql = "EXEC IsBlockedLocationForCC @CCName=" + DB.SQuote(cycleCount.CCName) + " ,@WarehouseID=" + cycleCount.WarehouseID + " ,@Location=" + DB.SQuote(cycleCount.Location) + ",@UserID=" + cycleCount.UserId; // Added By M.D.Prasad On 17-Apr-2020 For UserID
                int result = DB.GetSqlN(sql);
                if (result == 1)
                {
                    cycleCount = GetBinCount(cycleCount);
                    cycleCount.Result = "-1";

                }
                else
                {
                    cycleCount.Result = "-4";
                }
                //{
                //    cycleCount.Result = "0";
                //}
            }
            return cycleCount;

        }
        public BO.CycleCount CheckLocationForCycleCount(BO.CycleCount cycleCount)
        {
            int result = DB.GetSqlN("EXEC [dbo].[CheckLocationForCycleCount_HHT]  @Location=" + DB.SQuote(cycleCount.Location) + ",@UserId ="+cycleCount.UserId + ",@CCNAME =" + DB.SQuote(cycleCount.CCName) + ",@WarehouseID=" + cycleCount.WarehouseID);
            if (result ==0)
            {
                cycleCount.Result = "This Location is not configured to the user";
            }
            else
            {
                cycleCount.Result = "";
            }
            return cycleCount;
        }
        public BO.CycleCount BlockLocationForCycleCount(BO.CycleCount cycleCount)
        {
            int result = DB.GetSqlN("EXEC [dbo].[BlockLocationForCycelCount_HHT]  @Location=" + DB.SQuote(cycleCount.Location) );
            if(result==0)
            {
                cycleCount.Result = "This Location already in Transaction, Scan another Location";
                return cycleCount;
            }
            else
            {
                
                cycleCount = GetBinCount(cycleCount);
                cycleCount.Result = "";
            }
            return cycleCount;
        }
        public List<BO.CycleCount> GetCycleCountInformation(BO.CycleCount cycleCount)
        {
            List<BO.CycleCount> lstCCStock = new List<BO.CycleCount>();
            

            try
            {
                DataSet ds = DB.GetDS("exec [dbo].[GetCycleCountConsolidatedData_HHT]   @Location=" + DB.SQuote(cycleCount.Location) + ",@CCNAME =" +DB.SQuote(cycleCount.CCName)+ ",@WarehouseID=" + cycleCount.WarehouseID, false);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BO.CycleCount ccinformation = new BO.CycleCount();
                        ccinformation.CCName = dr["CCHeader"].ToString();
                        ccinformation.Location = dr["Location"].ToString();
                        ccinformation.SKU = dr["MCode"].ToString();
                        ccinformation.MfgDate = dr["Mfgdate"].ToString();
                        ccinformation.ExpDate = dr["expdate"].ToString();
                        ccinformation.SerialNo = dr["serialno"].ToString();
                        ccinformation.BatchNo = dr["Batchno"].ToString();
                        ccinformation.ProjectRefNo = dr["projectrefno"].ToString();
                        ccinformation.CCQty = Convert.ToDecimal(dr["Qty"].ToString());
                        lstCCStock.Add(ccinformation);
                    }
                }

                return lstCCStock;
            }
            catch (Exception ex)
            {
                return lstCCStock;
            }
        }
        public BO.CycleCount ReleaseCycleCountLocation(BO.CycleCount cyclecountinfo)

        {
          
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append(" EXEC [dbo].[CloseBinForCylceCount_HHT]");
            sbSqlString.Append(" @CCName=" + DB.SQuote(cyclecountinfo.CCName));
            sbSqlString.Append(" ,@Location=" + DB.SQuote(cyclecountinfo.Location));
            sbSqlString.Append(" ,@WarehouseID=" + cyclecountinfo.WarehouseID);

            int result = MRLWMSC21Common.DB.GetSqlN(sbSqlString.ToString());
            if (result == 0)
            {
                cyclecountinfo.Result = "Scanned Location already closed";
            }
            else
            {
                cyclecountinfo.Result = "Closed successfully";
            }
            return cyclecountinfo;

           
        }
        public BO.CycleCount CheckMaterialAvailablilty(BO.CycleCount cycleCount)
        {

            DataSet ds = DB.GetDS("exec [dbo].[CheckMaterialAvailablity_HHT]  @MCode=" + DB.SQuote(cycleCount.SKU)+ ",@TenantID="+cycleCount.TenantID, false);
            if (ds != null)
            {
                if(ds.Tables[0].Rows[0]["MATERIALMASTERID"].ToString()=="0")
                {
                    cycleCount.Result = "Material is not belongs to this user";
                }
                else if (ds.Tables[1].Rows[0]["UOMID"].ToString() == "0")
                {
                    cycleCount.Result = "UoM is not Configured to this Material";
                }
                else
                {
                    cycleCount=IsMaterialMappedToCycleCount(cycleCount);
                }
            }
                return cycleCount;
            
        }

        public BO.CycleCount IsMaterialMappedToCycleCount(BO.CycleCount cycleCount)
        {
            int result = DB.GetSqlN("EXEC [dbo].[IsMaterialMappedToCycleCount_HHT] @MCode=" + DB.SQuote(cycleCount.SKU) + ",@AccountCycleCountName =" + DB.SQuote(cycleCount.CCName) + ",@UserID=" + cycleCount.UserId + ",@AccountID=" + cycleCount.AccountId); // ========= AccountID Added By M.D.Prasad =======
            if (result == 0)
            {
                cycleCount.Result = "This Material is Not Configured in this CC";
            }
            else
            {
                cycleCount.Result = "";
            }
            return cycleCount;
        }
        public BO.CycleCount UpsertCycleCount(BO.CycleCount cyclecountinfo)
        {
            DataSet ds = DB.GetDS("exec [dbo].[Get_CCDetails_HHT] @CCName=" + DB.SQuote(cyclecountinfo.CCName) , false);
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    cyclecountinfo.CycleCountID = Convert.ToInt32(ds.Tables[0].Rows[0]["CCM_TRN_CycleCount_ID"].ToString());
                    cyclecountinfo.AccountCycleCountID = Convert.ToInt32(ds.Tables[0].Rows[0]["CCM_CNF_AccountCycleCount_ID"].ToString());
                    cyclecountinfo.MSTCycleCountID = Convert.ToInt32(ds.Tables[0].Rows[0]["CCM_MST_CycleCount_ID"].ToString());
                    cyclecountinfo.CycleCountEntityID = Convert.ToInt32(ds.Tables[0].Rows[0]["CCM_MST_CycleCountEntity_ID"].ToString());
                    cyclecountinfo.EntityID = Convert.ToInt32(ds.Tables[0].Rows[0]["Entity_ID"].ToString());
                }
            }
            else
            {
                cyclecountinfo.Result = "Error while inserting";
                return cyclecountinfo;
            }


            string strQuery = "EXEC [dbo].[UpsertCycleCountDetails_HHT] @CCM_TRN_CycleCount_ID=" + cyclecountinfo.CycleCountID ;
            strQuery += ",@CCM_CNF_AccountCycleCount_ID=" + cyclecountinfo.AccountCycleCountID;
            strQuery += ",@CCM_MST_CycleCount_ID=" + cyclecountinfo.MSTCycleCountID;
            strQuery += ",@CCM_MST_CycleCountEntity_ID=" + cyclecountinfo.CycleCountEntityID;
            strQuery += ",@Entity_ID=" + cyclecountinfo.EntityID;
            strQuery += ",@Location=" + DB.SQuote(cyclecountinfo.Location);
            strQuery += ",@Container=" + DB.SQuote(cyclecountinfo.Container);
            strQuery += ",@Mcode=" + DB.SQuote(cyclecountinfo.SKU);
            strQuery += ",@Qty=" + cyclecountinfo.CCQty;
            strQuery += ",@UserId =" +cyclecountinfo.UserId;
            strQuery += ",@TenantID =" + cyclecountinfo.TenantID;
            strQuery += ",@Mfgdate =" + DB.SQuote(cyclecountinfo.MfgDate != null ? cyclecountinfo.MfgDate: "NULL");
            strQuery += ",@Expdate =" + DB.SQuote(cyclecountinfo.ExpDate != null ? cyclecountinfo.ExpDate : "NULL");
            strQuery += ",@StorageLocation =" + DB.SQuote(cyclecountinfo.StorageLocation != null ? cyclecountinfo.StorageLocation : "NULL");
            if (cyclecountinfo.SerialNo != "")
            {
                strQuery += ",@SerialNo =" + DB.SQuote(cyclecountinfo.SerialNo != null ? cyclecountinfo.SerialNo : "NULL");
            }
            if (cyclecountinfo.BatchNo != "")
            {
                strQuery += ",@BatchNo =" + DB.SQuote(cyclecountinfo.BatchNo != null ? cyclecountinfo.BatchNo : "NULL");
            }
            if (cyclecountinfo.ProjectRefNo != "")
            {
                strQuery += ",@ProjectRefNo =" + DB.SQuote(cyclecountinfo.ProjectRefNo != null ? cyclecountinfo.ProjectRefNo : "NULL");
            }
            if (cyclecountinfo.ProjectRefNo != "")
            {
                strQuery += ",@MRP =" + DB.SQuote(cyclecountinfo.MRP != null ? cyclecountinfo.MRP : "NULL");
            }
            strQuery += " ,@WarehouseID=" + cyclecountinfo.WarehouseID;
            int result = DB.GetSqlN(strQuery);
            if (result == 0)
            {
                cyclecountinfo.Result = "Error while inserting";
            }
            else if (result == -1)
            {
                cyclecountinfo.Result = "Invalid Location";
            }
            else
            {

                cyclecountinfo = GetBinCount(cyclecountinfo);
                cyclecountinfo.Result = "Confirmed successfully";
            }

            return cyclecountinfo;
        }
        public BO.CycleCount GetBinCount(BO.CycleCount cyclecountinfo)
        {
           
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append(" EXEC [dbo].[GetBincount_HHT]");
            sbSqlString.Append(" @Location=" + DB.SQuote(cyclecountinfo.Location));
            sbSqlString.Append(" ,@CycleCountName=" + DB.SQuote(cyclecountinfo.CCName));
            sbSqlString.Append(" ,@WarehouseID=" +cyclecountinfo.WarehouseID);
            int result = MRLWMSC21Common.DB.GetSqlN(sbSqlString.ToString());
            if (result !=0)
            {
                cyclecountinfo.Result = "";
                cyclecountinfo.Count = result.ToString();
            }
            else
            {
                cyclecountinfo.Count = "0";
            }
           
            return cyclecountinfo;
        }
    }
}