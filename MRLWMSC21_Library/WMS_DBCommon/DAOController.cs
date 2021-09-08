using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon
{
    public class DAOController
    {
        public static Object GetData(DRLExecuteCode DB_DRL_CALL, Object input, int IN_MSG_TYPE = 1)
        {
            try
            {
                return transferControlToPullData(DB_DRL_CALL.ToString(), input, IN_MSG_TYPE);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while execution DAOController for FUNC_EXEC_CALL [" + DB_DRL_CALL + "] " + ex.Message);
            }
        }

        //there may be a chance to do intermediate mapping 
        public static Object GetDataFromStoredProcedure(DRLExecuteCode DB_DRL_CALL, IDictionary<string,object> input, int IN_MSG_TYPE = 2)
        {
            try
            {
                return transferControlToPullData(DB_DRL_CALL.ToString(), input, IN_MSG_TYPE);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while execution DAOController for FUNC_EXEC_CALL [" + DB_DRL_CALL + "] " + ex.Message);
            }
        }

        //there may be a chance to do intermediate mapping 
        public static Object GetDataByCustomWhereClause(DRLExecuteCode DB_DRL_CALL, Object input, int IN_MSG_TYPE = 3)
        {
            try
            {
                return transferControlToPullData(DB_DRL_CALL.ToString(), input, IN_MSG_TYPE);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while execution DAOController for FUNC_EXEC_CALL [" + DB_DRL_CALL + "] " + ex.Message);
            }
        }


        private static Object transferControlToPullData(string DB_DRL_CALL, Object input,int ACTION_MSG) 
        {
            
            if (DBServicePool.DBServicePoolList.ContainsKey(DB_DRL_CALL.Trim()))
            {
                DBServiceController rootControl = DBServicePool.DBServicePoolList[DB_DRL_CALL.Trim()];
                return rootControl.processDBCall(DB_DRL_CALL, input,ACTION_MSG);
            }
            else
            {
                return null;
            }

        }

    }
}
