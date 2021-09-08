using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.MasterDataDAO
{
    delegate object CustomerDataAccess(object input);

    internal class CustomerDAO : DBServiceController
    {
       
        public object processDBCall(string DB_CALL_CODE, object input,int IN_MSG_TYPE)
        {
            try
            {
                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(CustomerDAO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    CustomerDataAccess dataAccess = (CustomerDataAccess)Delegate.CreateDelegate(typeof(CustomerDataAccess), null, methodInfo);
                    return dataAccess(input);
                }
                else
                {
                    throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "] " + ex.Message);
            }
        }



        private object GetCustomerDataById(object input)
        {
            return null;
        }

        public object GetCustomerDataByCode(object input)
        {
            return null;
        }

        public object GetCustomerListByTenant(object input)
        {
            return null;
        }

        public object GetCustomerList(object input)
        {
            return null;
        }


      
    }

}
