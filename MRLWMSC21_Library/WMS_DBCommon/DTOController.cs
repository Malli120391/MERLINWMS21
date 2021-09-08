using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon
{
    public class DTOController
    {

        public static Object PushData(DMLExecuteCode DB_DML_CALL, Object input,int IN_MSG_TYPE=1)
        {
            try
            {
                return ControlToPushData(DB_DML_CALL.ToString(), input, IN_MSG_TYPE);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while execution DTOController for FUNC_EXEC_CALL [" + DB_DML_CALL + "] " + ex.Message);
            }
        }

        //there may be a chance to do intermediate mapping 
        public static Object DataToStoredProcedure(DMLExecuteCode DB_DML_CALL, Object input)
        {
            try
            {
                return ControlToPushData(DB_DML_CALL.ToString(), input, 2);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while execution DTOController for FUNC_EXEC_CALL [" + DB_DML_CALL + "] " + ex.Message);
            }
        }

        public static Object DataByCustomWhereClause(DMLExecuteCode DB_DML_CALL, Object input, int IN_MSG_TYPE = 0)
        {
            try
            {
                return ControlToPushData(DB_DML_CALL.ToString(), input, IN_MSG_TYPE);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Exception while execution DTOController for FUNC_EXEC_CALL [" + DB_DML_CALL + "] " + ex.Message);
            }
        }

        

        private static Object ControlToPushData(string DB_DML_CALL, Object input, int ACTION_MSG)
        {

            if (DBServicePool.DBServicePoolList.ContainsKey(DB_DML_CALL.Trim()))
            {
                DBServiceController rootControl = DBServicePool.DBServicePoolList[DB_DML_CALL.Trim()];
                return rootControl.processDBCall(DB_DML_CALL, input, ACTION_MSG);
            }
            else
            {
                return null;
            }

        }
    }
}
