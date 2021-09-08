using MRLWMSC21Common;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.MasterDataDAO
{
    delegate object MasterDataInfo(object input, int IN_MSG_TYPE);

    internal class MasterConfigDAO : DBServiceController
    {
        public object processDBCall(string DB_CALL_CODE, object input, int IN_MSG_TYPE)
        {
            try
            {
                
                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(MasterConfigDAO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    MasterDataInfo dataAccess = (MasterDataInfo)Delegate.CreateDelegate(typeof(MasterDataInfo), null, methodInfo);
                    return dataAccess(input,IN_MSG_TYPE);
                }
                else
                {
                    throw new NotImplementedException("the requested DB execution call not implemented under MasterDataDAO. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "] " + ex.Message);
            }
        }

        public object InitConfigurationInformation(object input, int IN_MSG_TYPE) 
        {
            WMSConfig wmsConfig = null;
            if(IN_MSG_TYPE ==1)
            {
                //getting data through query;
                int funcId = (input != null) ? (int)input : 0;
                string drlStatement = "SELECT CF.ConfigurableFunctionID,CF.FuncExecCode,CFS.FunctionalStrategyID,CFS.StrategyCode,CFS.DataType,CFS.Value FROM GEN_Configurable_Functionality CF JOIN GEN_Configurable_Functional_Strategy CFS ON CFS.ConfigurableFunctionID = CF.ConfigurableFunctionID WHERE CF.IsDeleted=0 AND CFS.IsDeleted=0 AND (0=" + funcId + " OR CF.ConfigurableFunctionID=" + funcId + ")  order by ConfigurableFunctionID ";

                DataSet configSet =  DB.GetDS(drlStatement,false);
                if(configSet != null  && configSet.Tables != null)
                {
                    IDictionary<string, IDictionary<string, object>> configInfo = new Dictionary<string, IDictionary<string, object>>();
                    foreach(DataRow row in configSet.Tables[0].Rows)
                    {
                        if (!configInfo.ContainsKey(row["FuncExecCode"].ToString()))
                        {
                            configInfo.Add((row["FuncExecCode"]).ToString(), new Dictionary<string, object>() { { row["StrategyCode"].ToString(), row["Value"].ToString() } });
                        }
                        else 
                        {
                            configInfo[row["FuncExecCode"].ToString()].Add(row["StrategyCode"].ToString(), row["Value"].ToString());
                        }   
                    }

                    if (configInfo != null)
                    {
                        wmsConfig = new WMSConfig();
                        wmsConfig.SetPutawayConfig(configInfo.ContainsKey("SUGGESTED_PUTAWAY") ? configInfo["SUGGESTED_PUTAWAY"] : null);
                        wmsConfig.SetPickingConfig(configInfo.ContainsKey("SUGGESTED_PICKING") ? configInfo["SUGGESTED_PICKING"] : null);
                    }
                }
            
            }else if(IN_MSG_TYPE == 2)
            {
                //getting data through procedure call.
            
            }
            return wmsConfig;
        }


    }
}
